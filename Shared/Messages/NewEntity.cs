using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shared.Components;
using Shared.Entities;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Shared.Messages
{
    public class NewEntity : Message
    {
        public NewEntity(Entity entity) : base(Type.NewEntity)
        {
            this.id = entity.id;

            if (entity.ContainsComponent<Shared.Components.Appearance>())
            {
                this.appearance = entity.GetComponent<Appearance>();
            }

            if (entity.ContainsComponent<Shared.Components.Camera>())
            {
                this.camera = entity.GetComponent<Shared.Components.Camera>();
            }

            if (entity.ContainsComponent<Shared.Components.Positionable>())
            {
                this.positionable = entity.GetComponent<Positionable>();
            }

            // Spawnable and consumable Components
            if (entity.ContainsComponent<Shared.Components.Spawnable>())
            {
                this.spawnable = entity.GetComponent<Spawnable>();
            }

            if (entity.ContainsComponent<Shared.Components.Consumable>())
            {
                this.consumable = entity.GetComponent<Consumable>();
            }

            if (entity.ContainsComponent<Shared.Components.Growable>())
            {
                this.growth = entity.GetComponent<Shared.Components.Growable>();
            }

            if (entity.ContainsComponent<Shared.Components.Animatable>())
            {
                this.animatable = entity.GetComponent<Animatable>();
            }

            if (entity.ContainsComponent<Shared.Components.Audible>())
            {
                this.audible = entity.GetComponent<Audible>();
            }

            if (entity.ContainsComponent<Shared.Components.Sound>())
            {
                this.sound = entity.GetComponent<Sound>();
            }

            if (entity.ContainsComponent<Movable>())
            {
                this.movable = entity.GetComponent<Movable>();
            }

            if (entity.ContainsComponent<Collidable>())
            {
                this.collidable = entity.GetComponent<Collidable>();
            }

            if (entity.ContainsComponent<Components.Linkable>())
            {
                this.linkable = entity.GetComponent<Linkable>();
            }

            if (entity.ContainsComponent<Components.KeyboardControllable>())
            {
                this.keyboardControllable = entity.GetComponent<Components.KeyboardControllable>();
            }

            if (entity.ContainsComponent<Components.MouseControllable>())
            {
                this.mouseControllable = entity.GetComponent<Components.MouseControllable>();
            }
        }

        public NewEntity() : base(Type.NewEntity)
        {
        }

        public uint id { get; private set; }

        // Appearance
        public bool hasAppearance { get; private set; }
        public Components.Appearance? appearance { get; private set; } = null;
        public Parsers.AppearanceParser.AppearanceMessage appearanceMessage { get; private set; }

        // Camera
        public bool hasCamera { get; private set; }
        public Components.Camera? camera { get; private set; } = null;
        public Parsers.CameraParser.CameraMessage cameraMessage { get; private set; }

        // Position
        public bool hasPosition { get; private set; }
        public Components.Positionable? positionable { get; private set; } = null;
        public Parsers.PositionableParser.PositionableMessage positionableMessage { get; private set; }

        // Audio
        public bool hasAudio { get; private set; }
        public Components.Audible? audible { get; private set; } = null;
        public Parsers.AudibleParser.AudibleMessage audibleMessage { get; private set; }

        // Sound
        public bool hasSound { get; private set; }
        public Components.Sound? sound { get; private set; } = null;
        public Parsers.SoundParser.SoundMessage soundMessage { get; private set; }

        // Movement
        public bool hasMovement { get; private set; }
        public Components.Movable? movable { get; private set; } = null;
        public Parsers.MovableParser.MovableMessage movableMessage { get; private set; }

        // Collision
        public bool hasCollidable { get; private set; }
        public Components.Collidable? collidable { get; private set; } = null;
        public Parsers.CollidableParser.CollidableMessage collidableMessage { get; private set; }

        // Spawnable
        public bool hasSpawnable { get; private set; }
        public Components.Spawnable? spawnable { get; private set; } = null;
        public Parsers.SpawnableParser.SpawnableMessage spawnableMessage { get; private set; }

        // Consumable
        public bool hasConsumable { get; private set; }
        public Components.Consumable? consumable { get; private set; } = null;
        public Parsers.ConsumableParser.ConsumableMessage consumableMessage { get; private set; }

        // Growable
        public bool hasGrowable { get; private set; }
        public Components.Growable? growth { get; private set; } = null;
        public Parsers.GrowableParser.GrowableMessage growthMessage { get; private set; }

        // Animatable
        public bool hasAnimatable { get; private set; }
        public Components.Animatable? animatable { get; private set; } = null;
        public Parsers.AnimatableParser.AnimatableMessage animatableMessage { get; private set; }

        // Linkable
        public bool hasLinkable { get; private set; }
        public Components.Linkable? linkable { get; private set; } = null;
        public Parsers.LinkableParser.LinkableMessage linkableMessage { get; private set; }

        // Keyboard Input
        // TODO: Fix this when new input
        public bool hasKeyboardControllable { get; private set; }
        public Components.KeyboardControllable? keyboardControllable { get; private set; } = null;
        public Parsers.KeyboardControllableParser.KeyboardControllableMessage keyboardControllableMessage { get; private set; }

        // Mouse Input
        // TODO: fix this when new input
        public bool hasMouseControllable { get; private set; }
        public Components.MouseControllable? mouseControllable { get; private set; } = null;
        public Parsers.MouseControllableParser.MouseControllableMessage mouseControllableMessage { get; private set; }

        public override byte[] serialize()
        {
            List<byte> data = new List<byte>();

            data.AddRange(base.serialize());
            data.AddRange(BitConverter.GetBytes(id));

            // Appearance
            data.AddRange(BitConverter.GetBytes(appearance != null));
            if (appearance != null)
            {
                appearance.Serialize(ref data);
            }

            data.AddRange(BitConverter.GetBytes(animatable != null));
            if (animatable != null)
            {
                animatable.Serialize(ref data);
            }

            // Camera
            data.AddRange(BitConverter.GetBytes(camera != null));
            if (camera != null)
            {
                camera.Serialize(ref data);
            }

            // Positionable
            data.AddRange(BitConverter.GetBytes(positionable != null));
            if (positionable != null)
            {
                positionable.Serialize(ref data);
            }

            // Audible
            data.AddRange(BitConverter.GetBytes(audible != null));
            if (audible != null)
            {
                audible.Serialize(ref data);
            }

            // Sound
            data.AddRange(BitConverter.GetBytes(sound != null));
            if (sound != null)
            {
                sound.Serialize(ref data);
            }

            // Movable
            data.AddRange(BitConverter.GetBytes(movable != null));
            if (movable != null)
            {
                movable.Serialize(ref data);
            }

            // Collidable
            data.AddRange(BitConverter.GetBytes(collidable != null));
            if (collidable != null)
            {
                collidable.Serialize(ref data);
            }

            // Spawnable
            data.AddRange(BitConverter.GetBytes(spawnable != null));
            if (spawnable != null)
            {
                spawnable.Serialize(ref data);
            }

            // Consumable
            data.AddRange(BitConverter.GetBytes(consumable != null));
            if (consumable != null)
            {
                consumable.Serialize(ref data);
            }

            // Growable
            data.AddRange(BitConverter.GetBytes(growth != null));
            if (growth != null)
            {
                growth.Serialize(ref data);
            }

            // // Linkable
            data.AddRange(BitConverter.GetBytes(linkable != null));
            if (linkable != null)
            {
                linkable.Serialize(ref data);
            }

            // Keyboard
            data.AddRange(BitConverter.GetBytes(keyboardControllable != null));
            if (keyboardControllable != null)
            {
                keyboardControllable.Serialize(ref data);
            }

            // Mouse
            data.AddRange(BitConverter.GetBytes(mouseControllable != null));
            if (mouseControllable != null)
            {
                mouseControllable.Serialize(ref data);
            }

            return data.ToArray();
        }

        public override int parse(byte[] data)
        {
            int offset = base.parse(data);

            this.id = BitConverter.ToUInt32(data, offset);
            offset += sizeof(uint);

            // Appearance
            this.hasAppearance = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasAppearance)
            {
                Parsers.AppearanceParser parser = new Parsers.AppearanceParser();
                parser.Parse(ref data, ref offset);
                this.appearanceMessage = parser.GetMessage();
            }

            this.hasAnimatable = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasAnimatable)
            {
                Parsers.AnimatableParser parser = new Parsers.AnimatableParser();
                parser.Parse(ref data, ref offset);
                this.animatableMessage = parser.GetMessage();
            }

            // Camera
            this.hasCamera = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasCamera)
            {
                Parsers.CameraParser parser = new Parsers.CameraParser();
                parser.Parse(ref data, ref offset);
                this.cameraMessage = parser.GetMessage();
            }

            // Positionable
            this.hasPosition = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasPosition)
            {
                Parsers.PositionableParser parser = new Parsers.PositionableParser();
                parser.Parse(ref data, ref offset);
                this.positionableMessage = parser.GetMessage();
            }

            // Audio
            this.hasAudio = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasAudio)
            {
                Parsers.AudibleParser parser = new Parsers.AudibleParser();
                parser.Parse(ref data, ref offset);
                this.audibleMessage = parser.GetMessage();
            }

            // Sound
            this.hasSound = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasSound)
            {
                Parsers.SoundParser parser = new Parsers.SoundParser();
                parser.Parse(ref data, ref offset);
                this.soundMessage = parser.GetMessage();
            }

            // Movable
            this.hasMovement = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasMovement)
            {
                Parsers.MovableParser parser = new Parsers.MovableParser();
                parser.Parse(ref data, ref offset);
                this.movableMessage = parser.GetMessage();
            }

            // Currently being handled elsewhere, but it would be nice to do here.
            // Collidable
            this.hasCollidable = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasCollidable)
            {
                Parsers.CollidableParser parser = new Parsers.CollidableParser();
                parser.Parse(ref data, ref offset);
                this.collidableMessage = parser.GetMessage();
            }

            // Spawnable
            this.hasSpawnable = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasSpawnable)
            {
                Parsers.SpawnableParser parser = new Parsers.SpawnableParser();
                parser.Parse(ref data, ref offset);
                this.spawnableMessage = parser.GetMessage();
            }

            // Consumable
            this.hasConsumable = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasConsumable)
            {
                Parsers.ConsumableParser parser = new Parsers.ConsumableParser();
                parser.Parse(ref data, ref offset);
                this.consumableMessage = parser.GetMessage();
            }

            this.hasGrowable = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasGrowable)
            {
                Parsers.GrowableParser parser = new Parsers.Growable();
                parser.Parse(ref data, ref offset);
                this.growthMessage = parser.GetMessage();
            }

            // Linkable
            this.hasLinkable = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasLinkable)
            {
                Parsers.LinkableParser parser = new Parsers.LinkableParser();
                parser.Parse(ref data, ref offset);
                this.linkableMessage = parser.GetMessage();
            }

            // Keyboard
            this.hasKeyboardControllable = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasKeyboardControllable)
            {
                Parsers.KeyboardControllableParser parser = new Parsers.KeyboardControllableParser();
                parser.Parse(ref data, ref offset);
                this.keyboardControllableMessage = parser.GetMessage();
            }

            // Mouse
            this.hasMouseControllable = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasMouseControllable)
            {
                Parsers.MouseControllableParser parser = new Parsers.MouseControllableParser();
                parser.Parse(ref data, ref offset);
                this.mouseControllableMessage = parser.GetMessage();
            }

            return offset;
        }
    }
}
