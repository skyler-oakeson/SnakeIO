using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Systems
{
    public class Linker : System
    {
        private Dictionary<string, List<uint>> chains;

        public Linker()
            : base(
                    typeof(Components.Linkable)
                    )
        {
            this.chains = new Dictionary<string, List<uint>>();
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
                Components.Linkable link = entity.GetComponent<Components.Linkable>();

                // If position is Head create a new chain with this entity id as the head.
                if (link.linkPos == Components.LinkPosition.Head)
                {
                    List<uint> chain = new List<uint>();
                    chain.Add(entity.id);
                    chains.Add(link.chain, chain);
                }

                // If position is Tail add the tail to already created chain
                else if (link.linkPos == Components.LinkPosition.Tail)
                {
                    List<uint> chain = chains[link.chain];
                    uint id = chain[chain.Count-1];
                    link.linkId = id;
                    chain.Add(entity.id);
                }

                // If position is Body check if there is a tail or not
                else
                {
                    List<uint> chain = chains[link.chain];

                    // Get last link in chain
                    Components.Linkable lastLink = entities[chain[chain.Count-1]].GetComponent<Components.Linkable>();

                    // If a tail is on the chain, link before tail
                    if (lastLink.linkPos == Components.LinkPosition.Tail)
                    {
                        chain.Insert(chain.Count-1, entity.id);
                        link.linkId = lastLink.linkId;
                        lastLink.linkId = entity.id;
                    }

                    // If no tail is on the chain, link to the end
                    else
                    {
                        uint id = chain[chain.Count-1];
                        link.linkId = id;
                        chain.Add(entity.id);
                    }
                }
            }

            return interested;
        }

        //TODO: Linker Remove link function

        override public void Update(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                Link(entity);
            }
        }

        /// <summary>
        /// Runs the LinkDelegate function with itself and it's linked entity.
        /// </summary>
        private void Link(Entities.Entity linkee)
        {
            Components.Linkable link = linkee.GetComponent<Components.Linkable>();
            if (link.linkId.HasValue)
            {
                Entities.Entity linked = entities[(uint)link.linkId];
                link.linkDelegate(linkee, linked);
            }
        }
    }
}
