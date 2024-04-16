using Microsoft.Xna.Framework;
using Shared.Components;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.InteropServices;

namespace Systems
{
    public class Network : Shared.Systems.System
    {
        public delegate void Handler(TimeSpan elapsedTime, Shared.Messages.Message message);
        public delegate void RemoveEntityHandler(RemoveEntity message);
        public delegate void NewEntityHandler(NewEntity message);

        private Dictionary<Shared.Messages.Type, Handler> commandMap = new Dictionary<Shared.Messages.Type, Handler>();
        private RemoveEntityHandler removeEntityHandler;
        private NewEntityHandler newEntityHandler;
        private uint lastMessageId = 0;
        private HashSet<uint> updatedEntities = new HashSet<uint>();

        /// <summary>
        /// Primary activity in the constructor is to setup the command map
        // that maps from message types to their handlers.
        /// </summary>
        public Network() :
            base(typeof(Shared.Components.Positionable))
        {

            registerHandler(Shared.Messages.Type.ConnectAck, (TimeSpan elapsedTime, Message message) =>
            { 
                handleConnectAck(elapsedTime, (ConnectAck)message);
            });

            registerHandler(Shared.Messages.Type.NewEntity, (TimeSpan elapsedTime, Message message) =>
            {
                newEntityHandler((NewEntity)message);
            });

            registerHandler(Shared.Messages.Type.UpdateEntity, (TimeSpan elapsedTime, Message message) =>
            {
                handleUpdateEntity(elapsedTime, (UpdateEntity)message);
            });

            registerHandler(Shared.Messages.Type.RemoveEntity, (TimeSpan elapsedTime, Message message) =>
            {
                removeEntityHandler((RemoveEntity)message);
            });
        }

        // Have to implement this because it is abstract in the base class
        public override void Update(TimeSpan elapsedTime) { }

        /// <summary>
        /// Have our own version of update, because we need a list of messages to work with, and
        /// messages aren't entities.
        /// </summary>
        public void update(TimeSpan elapsedTime, Queue<Message> messages)
        {
            updatedEntities.Clear();

            if (messages != null)
            {
                while (messages.Count > 0)
                {
                    var message = messages.Dequeue();
                    if (commandMap.ContainsKey(message.type))
                    {
                        commandMap[message.type](elapsedTime, message);
                    }

                    if (message.messageId.HasValue)
                    {
                        lastMessageId = message.messageId.Value;
                    }
                }
            }

            // After processing all the messages, perform server reconciliation by
            // resimulating the inputs from any sent messages not yet acknowledged by the server.
            var sent = SnakeIO.MessageQueueClient.instance.getSendMessageHistory(lastMessageId);
            while (sent.Count > 0)
            {
                var message = (Shared.Messages.Input)sent.Dequeue();
                if (message.type == Shared.Messages.Type.Input)
                {
                    var entity = entities[message.entityId];
                    if (updatedEntities.Contains(entity.id))
                    {
                        Shared.Components.KeyboardControllable con = entity.GetComponent<Shared.Components.KeyboardControllable>();
                        foreach (var input in message.inputs)
                        {
                            con.controls[input].Invoke(entity, message.elapsedTime);
                        }
                    }
                }
            }
        }

        private void registerHandler(Shared.Messages.Type type, Handler handler)
        {
            commandMap[type] = handler;
        }

        public void registerNewEntityHandler(NewEntityHandler handler)
        {
            newEntityHandler = handler;
        }

        public void registerRemoveEntityHandler(RemoveEntityHandler handler)
        {
            removeEntityHandler = handler;
        }

        /// <summary>
        /// Handler for the ConnectAck message.  This records the clientId
        /// assigned to it by the server, it also sends a request to the server
        /// to join the game.
        /// </summary>
        private void handleConnectAck(TimeSpan elapsedTime, ConnectAck message) 
        {
            SnakeIO.MessageQueueClient.instance.sendMessage(new Join());
        }

        /// <summary>
        /// Handler for the UpdateEntity message.  It checks to see if the client
        /// actually has the entity, and if it does, updates the components
        /// that are in common between the message and the entity.
        /// </summary>
        private void handleUpdateEntity(TimeSpan elapsedTime, UpdateEntity message) 
        { 
            if (entities.ContainsKey(message.id))
            {
                var entity = entities[message.id];
                if (entity.ContainsComponent<Positionable>() && message.hasPosition)
                {
                    Shared.Components.Positionable positionable = entity.GetComponent<Positionable>();
                    positionable.pos = message.position;
                    positionable.prevPos = message.prevPosition;
                    positionable.orientation = message.orientation;
                    updatedEntities.Add(entity.id);
                }
            }
        }
    }
}
