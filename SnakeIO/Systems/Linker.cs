using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Systems
{
    public class Linker : System
    {
        private Dictionary<string, List<uint>> chainHooks;

        public Linker()
            : base(
                    typeof(Components.Linkable)
                    )
        {
            this.chainHooks = new Dictionary<string, List<uint>>();
        }

        /// <summary>
        /// Overrides the system add function.
        /// Takes an entity that is interested in the Linkable component.
        /// Links it to the proper place in the proper chain depending on its properties.
        /// </summary>
        override public bool Add(Entities.Entity entity)
        {
            bool interested = IsInterested(entity);
            if (interested)
            {
                entities.Add(entity.id, entity);
                Link(entity);
            }

            return interested;
        }

        /// <summary>
        /// Links entity to its proper spot in its designated chain.
        /// </summary>
        private void Link(Entities.Entity entity)
        {
            Components.Linkable currLink = entity.GetComponent<Components.Linkable>();

            // Create chain
            if (!chainHooks.ContainsKey(currLink.chain))
            {
                List<uint> newChain = new List<uint>();
                chainHooks.Add(currLink.chain, newChain);
            }


            List<uint> chain = chainHooks[currLink.chain];

            // Link is Head
            if (currLink.linkPos == Components.LinkPosition.Head)
            {
                Console.WriteLine("HEAD");
                chain.Add(entity.id);
                currLink.nextEntity = entity;
                currLink.prevEntity = entity;
            }

            // Link is Tail 
            else if (currLink.linkPos == Components.LinkPosition.Tail)
            {
                Console.WriteLine("TAIL");
                Entities.Entity head = entities[chain[0]];
                Components.Linkable headLink = head.GetComponent<Components.Linkable>();
                headLink.prevEntity = entity;
                currLink.nextEntity = head;
                currLink.prevEntity = entities[chain[chain.Count - 1]];
                chain.Add(entity.id);
            }

            // Link is Body
            else
            {
                Entities.Entity lastEntity = entities[chain[chain.Count - 1]];
                Components.Linkable lastLink = lastEntity.GetComponent<Components.Linkable>();

                // Tail is on the chain
                if (lastLink.linkPos == Components.LinkPosition.Tail)
                {
                    Components.Linkable prevLink = lastLink.prevEntity.GetComponent<Components.Linkable>();
                    Entities.Entity prevEntity = lastLink.prevEntity;
                    currLink.nextEntity = lastEntity;
                    currLink.prevEntity = prevEntity;
                    prevLink.nextEntity = entity;
                    lastLink.prevEntity = entity;
                    chain.Insert(chain.Count-1, entity.id);
                }
                else
                {
                    lastLink.nextEntity = entity;
                    currLink.prevEntity = lastEntity;
                    chain.Add(entity.id);
                }
            }

        }

        //TODO: Linker Remove link function

        override public void Update(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                LinkDelegate(entity);
            }
        }

        /// <summary>
        /// Runs the LinkDelegate function with itself and it's linked entity.
        /// </summary>
        private void LinkDelegate(Entities.Entity root)
        {
            Components.Linkable rootLink = root.GetComponent<Components.Linkable>();
            if (rootLink.linkDelegate != null)
            {
                rootLink.linkDelegate(root);
            }
        }
    }
}
