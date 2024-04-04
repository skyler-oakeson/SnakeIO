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
                    var chain = new List<uint>();
                    chain.Add(entity.id);
                    chains.Add(link.chain, chain);
                }

                // If position is Tail add the tail to already created chain
                else if (link.linkPos == Components.LinkPosition.Tail)
                {
                    var chain = chains[link.chain];
                    uint id = chain[chain.Count-1];
                    link.linkId = id;
                    chain.Add(entity.id);
                }

                // If position is Body check if there is a tail or not
                else
                {
                    var chain = chains[link.chain];
                    Components.Linkable prevLink = entities[chain[chain.Count-1]].GetComponent<Components.Linkable>();

                    // If a tail is on the chain, link before tail
                    if (prevLink.linkPos == Components.LinkPosition.Tail)
                    {
                        chain.Insert(chain.Count-2, entity.id);
                        link.linkId = prevLink.linkId;
                        prevLink.linkId = entity.id;
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

        override public void Update(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                Link(entity);
            }
        }

        private void Link(Entities.Entity linkee)
        {
            Components.Linkable link = linkee.GetComponent<Components.Linkable>();
            if (link.linkId.HasValue)
            {
                Entities.Entity linked = entities[(uint)link.linkId];
                link.linkDelegate(linked);
            }
        }
    }
}
