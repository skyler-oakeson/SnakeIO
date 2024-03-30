using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Systems
{
    class Spawner : System
    {
        private Dictionary<Type, TimeSpan> spawnTimes = new Dictionary<Type, TimeSpan>();
        private List<Entities.Entity> entitiesToSpawn = new List<Entities.Entity>();
        private Random random = new Random();

        public Spawner()
            : base(
                    typeof(Components.Spawnable),
                    typeof(Components.Positionable))
        {
        }

        public override void Update(GameTime gameTime)
        {

            foreach (var entity in entities.Values)
            {
                if (entity.ContainsComponent<Components.Consumable>())
                {
                    Components.Consumable consumable = entity.GetComponent<Components.Consumable>();
                    Components.Spawnable spawnable = entity.GetComponent<Components.Spawnable>();
                    if (!spawnTimes.ContainsKey(consumable.type))
                    {
                        spawnTimes[consumable.type] = gameTime.TotalGameTime;
                    }
                    if (gameTime.TotalGameTime - spawnTimes[consumable.type] >= spawnable.spawnRate)
                    {
                        SpawnEntity(entity);
                        spawnTimes[consumable.type] = gameTime.TotalGameTime;
                    }
                }
            }
            foreach (var entity in entitiesToSpawn)
            {
                if (!entities.ContainsKey(entity.id))
                {
                    this.Add(entity);
                }
            }
            entitiesToSpawn.Clear();
        }

        private void SpawnEntity(Entities.Entity entity)
        {
            Components.Spawnable spawnable = entity.GetComponent<Components.Spawnable>();
            Components.Renderable renderable = entity.GetComponent<Components.Renderable>();
            Components.Positionable positionable = entity.GetComponent<Components.Positionable>();
            for (int i = 0; i < spawnable.spawnCount; i++)
            {
                entitiesToSpawn.Add(Entities.Food.Create(renderable.Texture, new Vector2(100 + (10 * i), 100 + (10 * i))));
            }
        }
    }
}
