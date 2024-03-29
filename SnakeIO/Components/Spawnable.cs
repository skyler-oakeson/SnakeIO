namespace Components
{
    class Spawnable : Component
    {
        public float spawnRate;

        public Spawnable(float spawnRate)
        {
            this.spawnRate = spawnRate;
        }
    }
}
