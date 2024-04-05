using Microsoft.Xna.Framework;
using Shared.Components;
using Shared.Entities;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Shared.Messages
{
    public class NewEntity : Message
    {
        public NewEntity(Entity entity) : base(Type.NewEntity)
        {
            this.id = entity.id;

            if (entity.ContainsComponent<Shared.Components.Renderable>())
            {
                this.hasRenderable = true;
                this.texture = entity.GetComponent<Renderable>().Texture.Name;
                this.color = entity.GetComponent<Renderable>().Color;
                this.stroke = entity.GetComponent<Renderable>().Stroke;
            }
            else
            {
                this.texture = "";
            }

            // Spawnable and consumable Components

            if (entity.ContainsComponent<Shared.Components.Spawnable>())
            {
                this.hasSpawnable = true;
                this.spawnRate = entity.GetComponent<Spawnable>().spawnRate;
                this.spawnCount = entity.GetComponent<Spawnable>().spawnCount;
                this.spawnType = entity.GetComponent<Spawnable>().type;
            }

            if (entity.ContainsComponent<Shared.Components.Consumable>())
            {
                this.hasConsumable = true;
                this.growth = entity.GetComponent<Consumable>().growth;
                this.consumableType = entity.GetComponent<Consumable>().type;
            }

            // TODO: Add Animatable component
            if (entity.ContainsComponent<Shared.Components.Animatable>())
            {
            }

            if (entity.ContainsComponent<Shared.Components.Positionable>())
            {
                this.hasPosition = true;
                this.position = entity.GetComponent<Positionable>().Pos;
            }

            if (entity.ContainsComponent<Shared.Components.Audible>())
            {
                this.hasAudio = true;
                this.audio = entity.GetComponent<Audible>().Sound.Name;
            }
            else
            {
                this.audio = "";
            }

            if (entity.ContainsComponent<Movable>())
            {
                this.hasMovement = true;
                this.velocity = entity.GetComponent<Movable>().Velocity;
                this.rotation = entity.GetComponent<Movable>().Rotation;
            }

            if (entity.ContainsComponent<Collidable>())
            {
                this.hasCollision = true;
                this.hitBox = entity.GetComponent<Collidable>().HitBox;
                this.collided = entity.GetComponent<Collidable>().Collided;
            }

            if (entity.ContainsComponent<Components.KeyboardControllable>())
            {
                this.hasInput = true;
                foreach ((Shared.Controls.Control con, Shared.Controls.ControlDelegate del) in entity.GetComponent<KeyboardControllable>().actions)
                {
                    this.inputs.Add(con);
                }
            }
            else
            {
                this.inputs = new List<Shared.Controls.Control>();
            }

            if (entity.ContainsComponent<Components.MouseControllable>())
            {
                this.hasMouseInput = true;
                foreach ((Shared.Controls.Control con, Shared.Controls.ControlDelegate del) in entity.GetComponent<KeyboardControllable>().actions)
                {
                    this.mouseInputs.Add(con);
                }
            }
        }
        public NewEntity() : base(Type.NewEntity)
        {
            this.texture = "";
            this.inputs = new List<Shared.Controls.Control>();
            this.audio = "";
        }

        public uint id { get; private set; }

        // Appearance
        public bool hasRenderable { get; private set; } = false;
        public string texture { get; private set; }
        public Color color { get; private set; }
        public Color stroke { get; private set; }

        // Position
        public bool hasPosition { get; private set; } = false;
        public Vector2 position { get; private set; }

        // Audio
        public bool hasAudio { get; private set; } = false;
        public string audio { get; private set; }

        // Movement
        public bool hasMovement { get; private set; } = false;
        public Vector2 velocity { get; private set; }
        public Vector2 rotation { get; private set; }

        // Collision
        public bool hasCollision { get; private set; } = false;
        public bool collided { get; private set; } = false;
        public Vector3 hitBox { get; private set; }

        // Spawnable
        public bool hasSpawnable { get; private set; } = false;
        public TimeSpan spawnRate { get; private set; }
        public int spawnCount { get; private set; }
        public System.Type? spawnType { get; private set; }

        // Consumable
        public bool hasConsumable { get; private set; } = false;
        public float growth { get; private set; }
        public System.Type? consumableType { get; private set; }

        // Keyboard Input
        // May have to change inputs to a dictionary that has the delegate with the control (we will)
        public bool hasInput { get; private set; } = false;
        public List<Shared.Controls.Control> inputs { get; private set; } = new List<Shared.Controls.Control>();

        //Mouse Input
        public bool hasMouseInput { get; private set; } = false;
        public List<Shared.Controls.Control> mouseInputs { get; private set; } = new List<Shared.Controls.Control>();

        public override byte[] serialize()
        {
            List<byte> data = new List<byte>();

            data.AddRange(base.serialize());
            data.AddRange(BitConverter.GetBytes(id));

            data.AddRange(BitConverter.GetBytes(hasRenderable));
            if (hasRenderable)
            {
                data.AddRange(BitConverter.GetBytes(texture.Length));
                data.AddRange(Encoding.UTF8.GetBytes(texture));
                data.AddRange(BitConverter.GetBytes(color.R));
                data.AddRange(BitConverter.GetBytes(color.G));
                data.AddRange(BitConverter.GetBytes(color.B));
                data.AddRange(BitConverter.GetBytes(stroke.R));
                data.AddRange(BitConverter.GetBytes(stroke.G));
                data.AddRange(BitConverter.GetBytes(stroke.B));
            }

            data.AddRange(BitConverter.GetBytes(hasPosition));
            if (hasPosition)
            {
                data.AddRange(BitConverter.GetBytes(position.X));
                data.AddRange(BitConverter.GetBytes(position.Y));
            }

            data.AddRange(BitConverter.GetBytes(hasAudio));
            if (hasAudio)
            {
                data.AddRange(BitConverter.GetBytes(audio.Length));
                data.AddRange(Encoding.UTF8.GetBytes(audio));
            }

            data.AddRange(BitConverter.GetBytes(hasMovement));
            if (hasMovement)
            {
                data.AddRange(BitConverter.GetBytes(velocity.X));
                data.AddRange(BitConverter.GetBytes(velocity.Y));
                data.AddRange(BitConverter.GetBytes(rotation.X));
                data.AddRange(BitConverter.GetBytes(rotation.Y));
            }

            data.AddRange(BitConverter.GetBytes(hasCollision));
            if (hasCollision)
            {
                data.AddRange(BitConverter.GetBytes(collided));
                data.AddRange(BitConverter.GetBytes(hitBox.X));
                data.AddRange(BitConverter.GetBytes(hitBox.Y));
                data.AddRange(BitConverter.GetBytes(hitBox.Z));
            }

            data.AddRange(BitConverter.GetBytes(hasSpawnable));
            if (hasSpawnable)
            {
                data.AddRange(BitConverter.GetBytes(spawnRate.Milliseconds));
                data.AddRange(BitConverter.GetBytes(spawnCount));
                data.AddRange(BitConverter.GetBytes(spawnType.AssemblyQualifiedName.Length));
                data.AddRange(Encoding.UTF8.GetBytes(spawnType.AssemblyQualifiedName));
            }

            data.AddRange(BitConverter.GetBytes(hasConsumable));
            if (hasConsumable)
            {
                data.AddRange(BitConverter.GetBytes(growth));
                data.AddRange(BitConverter.GetBytes(consumableType.AssemblyQualifiedName.Length));
                data.AddRange(Encoding.UTF8.GetBytes(consumableType.AssemblyQualifiedName));
            }

            data.AddRange(BitConverter.GetBytes(hasInput));
            if (hasInput)
            {
                data.AddRange(BitConverter.GetBytes(inputs.Count));
                foreach (Shared.Controls.Control con in inputs)
                {
                    data.AddRange(BitConverter.GetBytes((UInt16)con.sc));
                    data.AddRange(BitConverter.GetBytes((UInt16)con.cc));
                    if (con.key != null)
                    {
                        data.AddRange(BitConverter.GetBytes((UInt16)con.key));
                    }
                    data.AddRange(BitConverter.GetBytes(con.keyPressOnly));
                }
            }

            data.AddRange(BitConverter.GetBytes(hasMouseInput));
            if (hasMouseInput)
            {
                data.AddRange(BitConverter.GetBytes(mouseInputs.Count));
                foreach (Shared.Controls.Control con in mouseInputs)
                {
                    data.AddRange(BitConverter.GetBytes((UInt16)con.sc));
                    data.AddRange(BitConverter.GetBytes((UInt16)con.cc));
                    if (con.mouseEvent != null)
                    {
                        data.AddRange(BitConverter.GetBytes((UInt16)con.mouseEvent));
                    }
                    data.AddRange(BitConverter.GetBytes(con.keyPressOnly));
                }
            }

            return data.ToArray();
        }

        public override int parse(byte[] data)
        {
            int offset = base.parse(data);

            this.id = BitConverter.ToUInt32(data, offset);
            offset += sizeof(uint);

            this.hasRenderable = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasRenderable)
            {
                int textureSize = BitConverter.ToInt32(data, offset);
                offset += sizeof(Int32);
                this.texture = Encoding.UTF8.GetString(data, offset, textureSize);
                offset += textureSize;
                //for color
                byte r = data[offset];
                offset += sizeof(byte);
                byte g = data[offset];
                offset += sizeof(byte);
                byte b = data[offset];
                offset += sizeof(byte);
                this.color = new Color(r, g, b);
                //for stroke
                r = data[offset];
                offset += sizeof(byte);
                g = data[offset];
                offset += sizeof(byte);
                b = data[offset];
                offset += sizeof(byte);
                this.stroke = new Color(r, g, b);
            }

            this.hasPosition = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasPosition)
            {
                float positionX = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
                float positionY = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
                this.position = new Vector2(positionX, positionY);
                offset += sizeof(Single);
            }

            this.hasAudio = BitConverter.ToBoolean(data, offset);
            if (hasAudio)
            {
                int audioSize = BitConverter.ToInt32(data, offset);
                offset += sizeof(Int32);
                this.audio = Encoding.UTF8.GetString(data, offset, audioSize);
                offset += audioSize;
            }

            this.hasMovement = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasMovement)
            {
                float velocityX = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
                float velocityY = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
                float rotationX = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
                float rotationY = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
                this.velocity = new Vector2(velocityX, velocityY);
                offset += sizeof(Single);
                this.rotation = new Vector2(rotationX, rotationY);
                offset += sizeof(Single);
            }

            this.hasCollision = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasCollision)
            {
                this.collided = BitConverter.ToBoolean(data, offset);
                offset += sizeof(bool);
                float hitBoxX = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
                float hitBoxY = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
                float hitBoxZ = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
                this.hitBox = new Vector3(hitBoxX, hitBoxY, hitBoxZ);
                offset += sizeof(Single);
            }

            this.hasSpawnable = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasSpawnable)
            {
                this.spawnRate = new TimeSpan(BitConverter.ToInt32(data, offset));
                offset += sizeof(long);
                this.spawnCount = BitConverter.ToInt32(data, offset);
                offset += sizeof(UInt32);
                int spawnTypeSize = BitConverter.ToInt32(data, offset);
                offset += sizeof(Int32);
                this.spawnType = System.Type.GetType(Encoding.UTF8.GetString(data, offset, spawnTypeSize));
                offset += spawnTypeSize;
            }

            this.hasInput = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasInput)
            {
                int inputCount = BitConverter.ToInt32(data, offset);
                offset += sizeof(UInt32);
                for (int i = 0; i < inputCount; i++)
                {
                    Scenes.SceneContext sc = (Scenes.SceneContext)BitConverter.ToUInt16(data, offset); //scene context comes first
                    offset += sizeof(UInt16);
                    Shared.Controls.ControlContext cc = (Shared.Controls.ControlContext)BitConverter.ToUInt16(data, offset); //control context comes second
                    offset += sizeof(UInt16);
                    Keys key = (Keys) BitConverter.ToUInt16(data, offset);
                    offset += sizeof(UInt16);
                    bool keyPressOnly = BitConverter.ToBoolean(data, offset);
                    offset += sizeof(bool);
                    this.inputs.Add(new Shared.Controls.Control(sc, cc, key, null, keyPressOnly));
                }
            }

            this.hasMouseInput = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasMouseInput) {
                int mouseInputCount = BitConverter.ToInt32(data, offset);
                offset += sizeof(UInt32);
                for (int i = 0; i < mouseInputCount; i++)
                {
                    Scenes.SceneContext sc = (Scenes.SceneContext)BitConverter.ToUInt16(data, offset); //scene context comes first
                    offset += sizeof(UInt16);
                    Shared.Controls.ControlContext cc = (Shared.Controls.ControlContext)BitConverter.ToUInt16(data, offset); //control context comes second
                    offset += sizeof(UInt16);
                    Shared.Controls.MouseEvent mouseEvent = (Shared.Controls.MouseEvent)BitConverter.ToUInt16(data, offset);
                    offset += sizeof(UInt16);
                    bool keyPressOnly = BitConverter.ToBoolean(data, offset);
                    offset += sizeof(bool);
                    this.mouseInputs.Add(new Shared.Controls.Control(sc, cc, null, mouseEvent, keyPressOnly));
                }
            }

            return offset;
        }
    }
}
