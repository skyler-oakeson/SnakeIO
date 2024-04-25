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
        public delegate void GameOverHandler(GameOver message);
        public delegate void CollisionHandler(Shared.Messages.Collision message);
        public delegate void ScoresHandler(Shared.Messages.Scores message);

        private Dictionary<Shared.Messages.Type, Handler> commandMap = new Dictionary<Shared.Messages.Type, Handler>();
        private RemoveEntityHandler removeEntityHandler;
        private NewEntityHandler newEntityHandler;
        private GameOverHandler gameOverHandler;
        private CollisionHandler collisionHandler;
        private ScoresHandler scoresHandler;
        private uint lastMessageId = 0;
        private HashSet<uint> updatedEntities = new HashSet<uint>();

        /// <summary>
        /// Primary activity in the constructor is to setup the command map
        // that maps from message types to their handlers.
        /// </summary>
        public Network(string name) :
            base(typeof(Shared.Components.Positionable))
        {

            registerHandler(Shared.Messages.Type.ConnectAck, (TimeSpan elapsedTime, Message message) =>
            {
                handleConnectAck(elapsedTime, (ConnectAck)message, name);
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

            registerHandler(Shared.Messages.Type.GameOver, (TimeSpan elapsedTime, Message message) =>
            {
                gameOverHandler((GameOver)message);
            });

            registerHandler(Shared.Messages.Type.Collision, (TimeSpan elapsedTime, Message message) =>
            {
                collisionHandler((Shared.Messages.Collision)message);
            });

            registerHandler(Shared.Messages.Type.Scores, (TimeSpan elapsedTime, Shared.Messages.Message message) =>
            {
                scoresHandler((Shared.Messages.Scores)message);
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
                if (message.type == Shared.Messages.Type.Input && entities.ContainsKey(message.entityId))
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

        public void registerGameOverHandler(GameOverHandler handler)
        {
            gameOverHandler = handler;
        }

        public void registerCollisionHandler(CollisionHandler handler)
        {
            collisionHandler = handler;
        }

        public void registerScoreshandler(ScoresHandler handler)
        {
            scoresHandler = handler;
        }

        /// <summary>
        /// Handler for the ConnectAck message.  This records the clientId
        /// assigned to it by the server, it also sends a request to the server
        /// to join the game.
        /// </summary>
        private void handleConnectAck(TimeSpan elapsedTime, ConnectAck message, string name)
        {
            SnakeIO.MessageQueueClient.instance.sendMessage(new Join(name));
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
                    Shared.Components.Growable growable = entity.GetComponent<Growable>();
                    positionable.pos = message.position;
                    positionable.prevPos = message.prevPosition;
                    positionable.orientation = message.orientation;
                    growable.growth = message.growth;
                    updatedEntities.Add(entity.id);
                }
                if (entity.ContainsComponent<ParticleComponent>() && message.hasParticle)
                {
                    Shared.Components.ParticleComponent pComponent = entity.GetComponent<ParticleComponent>();
                    pComponent.type = message.particleMessage.type;
                    pComponent.size = message.particleMessage.size;
                    pComponent.direction = message.particleMessage.direction;
                    pComponent.speed = message.particleMessage.speed;
                    pComponent.center = message.particleMessage.center;
                    pComponent.lifetime = message.particleMessage.lifetime;
                    pComponent.shouldCreate = message.particleMessage.shouldCreate;
                }
            }
        }
    }
}
