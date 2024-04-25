using Shared.Messages;

namespace Systems
{
    public class Network : Shared.Systems.System

    {
        public delegate void Handler(int clientId, TimeSpan elapsedTime, Shared.Messages.Message message);
        public delegate void ScoresHandler(int clientId, TimeSpan elapsedTime, Shared.Messages.Scores message);
        public delegate void JoinHandler(int clientId, Shared.Messages.Join message);
        public delegate void DisconnectHandler(int clientId);

        private Dictionary<Shared.Messages.Type, Handler> commandMap = new Dictionary<Shared.Messages.Type, Handler>();
        private JoinHandler joinHandler;
        private DisconnectHandler disconnectHandler;
        private ScoresHandler scoresHandler;

        private HashSet<uint> reportThese = new HashSet<uint>();

        /// <summary>
        /// Primary activity in the constructor is to setup the command map
        /// that maps from message types to their handlers.
        /// </summary>
        public Network() :
            base(
                typeof(Shared.Components.Movable),
                typeof(Shared.Components.Positionable)
            )
        {
            // Register our own join handler
            registerHandler(Shared.Messages.Type.Join, (int clientId, TimeSpan elapsedTime, Shared.Messages.Message message) =>
            {
                if (joinHandler != null)
                {
                    joinHandler(clientId, (Shared.Messages.Join)message);
                }
            });

            // Register our own disconnect handler
            registerHandler(Shared.Messages.Type.Disconnect, (int clientId, TimeSpan elapsedTime, Shared.Messages.Message message) =>
            {
                if (disconnectHandler != null)
                {
                    disconnectHandler(clientId);
                }
            });

            registerHandler(Shared.Messages.Type.Input, (int clientId, TimeSpan elapsedTime, Shared.Messages.Message message) =>
            {
                handleInput((Shared.Messages.Input)message);
            });

            registerHandler(Shared.Messages.Type.Scores, (int clientId, TimeSpan elapsedTime, Shared.Messages.Message message) =>
            {
                if (scoresHandler != null)
                {
                    scoresHandler(clientId, elapsedTime, (Shared.Messages.Scores)message);
                }
            });
        }

        // Have to implement this because it is abstract in the base class
        public override void Update(TimeSpan elapsedTime) { }

        /// <summary>
        /// Have our own version of update, because we need a list of messages to work with, and
        /// messages aren't entities.
        /// </summary>
        public void update(TimeSpan elapsedTime, Queue<Tuple<int, Message>> messages)
        {
            if (messages != null)
            {
                while (messages.Count > 0)
                {
                    var message = messages.Dequeue();
                    if (commandMap.ContainsKey(message.Item2.type))
                    {
                        commandMap[message.Item2.type](message.Item1, elapsedTime, message.Item2);
                    }
                }
            }

            // Send updated game state updates back out to connected clients
            updateClients(elapsedTime);
        }

        public void registerJoinHandler(JoinHandler handler)
        {
            joinHandler = handler;
        }

        public void registerDisconnectHandler(DisconnectHandler handler)
        {
            disconnectHandler = handler;
        }

        private void registerHandler(Shared.Messages.Type type, Handler handler)
        {
            commandMap[type] = handler;
        }

        /// <summary>
        /// Handler for the Input message.  This simply passes the responsibility
        /// to the registered input handler.
        /// </summary>
        /// <param name="message"></param>
        private void handleInput(Shared.Messages.Input message)
        {
            if (entities.ContainsKey(message.entityId))
            {
                Shared.Entities.Entity entity = entities[message.entityId];
                foreach (Shared.Controls.ControlContext input in message.inputs)
                {
                    Shared.Entities.Player.PlayerKeyboardControls[input].Invoke(entity, message.elapsedTime);
                }
                reportThese.Add(entity.id);
            }
        }

        /// <summary>
        /// For the entities that have updates, send those updates to all
        /// connected clients.
        /// </summary>
        private void updateClients(TimeSpan elapsedTime)
        {
            foreach (var entityId in reportThese)
            {
                var entity = entities[entityId];
                var message = new Shared.Messages.UpdateEntity(entity, elapsedTime);
                Server.MessageQueueServer.instance.broadcastMessageWithLastId(message);
            }

            reportThese.Clear();
        }
    }
}
