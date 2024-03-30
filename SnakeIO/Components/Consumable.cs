using System;

namespace Components
{
    class Consumable : Component
    {
        public float growth;
        public Type type { get; private set; }

        public Consumable(Type type, float growth)
        {
            this.growth = growth;
            this.type = type;
        }
    }
}
