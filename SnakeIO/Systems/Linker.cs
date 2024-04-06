using System;
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
                chain.Add(entity.id);
            }

            // Link is Tail 
            else if (currLink.linkPos == Components.LinkPosition.Tail)
            {
                Entities.Entity head = entities[chain[0]];
                Components.Linkable headLink = head.GetComponent<Components.Linkable>();
                headLink.prevEntity = entity;
                headLink.nextEntity = entity;
                currLink.nextEntity = head;
                currLink.prevEntity = entities[chain[chain.Count - 1]];
                chain.Add(entity.id);
            }

            // Link is Body
            else
            {
                Components.Linkable nextLink = entities[chain[chain.Count - 1]].GetComponent<Components.Linkable>();
                Components.Linkable prevLink = entities[chain[chain.Count - 2]].GetComponent<Components.Linkable>();

                // Tail is on the chain
                if (nextLink.linkPos == Components.LinkPosition.Tail)
                {
                    chain.Insert(chain.Count - 1, entity.id);
                    currLink.prevEntity = nextLink.prevEntity;
                    currLink.nextEntity = prevLink.nextEntity;
                    nextLink.prevEntity = entity;
                    prevLink.nextEntity = entity;
                }
                else
                {
                    uint prevId = chain[chain.Count - 1];
                    chain.Add(entity.id);
                    currLink.prevEntity = prevLink.nextEntity;
                    nextLink.nextEntity = entity;
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
