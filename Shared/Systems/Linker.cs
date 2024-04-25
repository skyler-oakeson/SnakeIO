using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Shared.Systems
{
    public class Linker : Shared.Systems.System
    {
        private Dictionary<string, List<uint>> chainHooks;

        public Linker()
            : base(
                    typeof(Shared.Components.Linkable)
                    )
        {
            this.chainHooks = new Dictionary<string, List<uint>>();
        }

        /// <summary>
        /// Overrides the system add function.
        /// Takes an entity that is interested in the Linkable component.
        /// Links it to the proper place in the proper chain depending on its properties.
        /// </summary>
        override public bool Add(Shared.Entities.Entity entity)
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
        private void Link(Shared.Entities.Entity entity)
        {
            Shared.Components.Linkable entityLink = entity.GetComponent<Shared.Components.Linkable>();

            // Create chain
            if (!chainHooks.ContainsKey(entityLink.chain))
            {
                List<uint> newChain = new List<uint>();
                chainHooks.Add(entityLink.chain, newChain);
            }

            List<uint> chain = chainHooks[entityLink.chain];

            if (chain.Count <= 0)
            {
                chain.Add(entity.id);
            }
            else if (entityLink.linkPos == Shared.Components.LinkPosition.Body)
            {
                Shared.Entities.Entity end = entities[chain[chain.Count-1]];
                // Insert before Tail
                if(end.GetComponent<Shared.Components.Linkable>().linkPos == Shared.Components.LinkPosition.Tail )
                {
                    Shared.Entities.Entity tail = end;
                    Shared.Components.Linkable tailLink = end.GetComponent<Shared.Components.Linkable>();
                    entityLink.prevEntity = tailLink.prevEntity;
                    entityLink.nextEntity = tail;
                    tailLink.prevEntity = entity;
                    chain.Insert(chain.Count-2, entity.id);
                }
                else
                {
                    Shared.Components.Linkable endLink = end.GetComponent<Shared.Components.Linkable>();
                    endLink.nextEntity = entity;
                    entityLink.prevEntity = end;
                    chain.Add(entity.id);
                }
            }

            else if (entityLink.linkPos == Shared.Components.LinkPosition.Head)
            {
                Debug.Assert(entities[chain[0]].GetComponent<Shared.Components.Linkable>().linkPos != Shared.Components.LinkPosition.Head, "Head already attatched");
                Shared.Entities.Entity end = entities[chain[chain.Count-1]];
                // Link Head to Tail
                if (end.GetComponent<Shared.Components.Linkable>().linkPos == Shared.Components.LinkPosition.Tail)
                {
                    Shared.Entities.Entity next = entities[chain[0]];
                    Shared.Components.Linkable nextLink = next.GetComponent<Shared.Components.Linkable>();
                    Shared.Components.Linkable endLink = end.GetComponent<Shared.Components.Linkable>();
                    nextLink.prevEntity = entity;
                    entityLink.nextEntity = next;
                    entityLink.prevEntity = end;
                    endLink.nextEntity = entity;
                }
                else
                {
                    Shared.Entities.Entity next = entities[chain[0]];
                    Shared.Components.Linkable nextLink = next.GetComponent<Shared.Components.Linkable>();
                    nextLink.prevEntity = entity;
                    entityLink.nextEntity = next;
                }
                chain.Insert(0, entity.id);
            }

            else if (entityLink.linkPos == Shared.Components.LinkPosition.Tail)
            {
                Debug.Assert(entities[chain[chain.Count-1]].GetComponent<Shared.Components.Linkable>().linkPos != Shared.Components.LinkPosition.Tail, "Tail already attatched");
                Shared.Entities.Entity start = entities[chain[0]];
                // Link Tail to Head
                if (start.GetComponent<Shared.Components.Linkable>().linkPos == Shared.Components.LinkPosition.Head)
                {
                    Shared.Entities.Entity prev = entities[chain[chain.Count-1]];
                    Shared.Components.Linkable prevLink = prev.GetComponent<Shared.Components.Linkable>();
                    Shared.Components.Linkable startLink = start.GetComponent<Shared.Components.Linkable>();
                    entityLink.nextEntity = start;
                    entityLink.prevEntity = prev;
                    prevLink.nextEntity = entity;
                    startLink.prevEntity = entity;
                    chain.Add(entity.id);
                }
                else
                {
                    Shared.Entities.Entity prev = entities[chain[chain.Count-1]];
                    Shared.Components.Linkable prevLink = prev.GetComponent<Shared.Components.Linkable>();
                    prevLink.nextEntity = entity;
                    entityLink.prevEntity = prev;
                    chain.Add(entity.id);
                }
            }
        }

        public Shared.Entities.Entity GetHeadOfChain(string chain)
        {
            Debug.Assert(chainHooks.ContainsKey(chain), "Chain does not exist.");
            Debug.Assert(chainHooks[chain].Count() > 0, "Chain is empty.");
            return (entities[chainHooks[chain][0]]);
        }

        //TODO: Linker Remove link function
        public List<int> ChainDeletion(Shared.Entities.Entity entity)
        {
            Debug.Assert(entity.ContainsComponent<Shared.Components.Linkable>(), "Entity has no links");
            List<int> toRemove = new List<int>();
            string chain = entity.GetComponent<Shared.Components.Linkable>().chain;
            foreach (int id in chainHooks[chain])
            {
                toRemove.Add(id);
            }
            chainHooks.Remove(chain);
            return toRemove;
        }

        override public void Update(TimeSpan elapsedTime)
        {
            foreach (var entity in entities.Values)
            {
                LinkDelegate(entity);
            }
        }

        /// <summary>
        /// Runs the LinkDelegate function with itself and it's linked entity.
        /// </summary>
        private void LinkDelegate(Shared.Entities.Entity root)
        {
            Shared.Components.Linkable rootLink = root.GetComponent<Shared.Components.Linkable>();
            if (rootLink.linkDelegate != null)
            {
                rootLink.linkDelegate(root);
            }
        }
    }
}
