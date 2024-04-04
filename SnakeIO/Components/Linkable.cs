#nullable enable
using System;

namespace Components 
{
    /// <summary>
    /// This component provides a one way link to some other component with the linkable component.
    /// The type of components that get linked will update the same type of component on the linked.
    /// If the linkedId is null that entity is linkable but not directly linked and affecting some other entity.
    /// This allows for other entities to link to it.
    /// </summary>
    public class Linkable : Component
    {
        public uint? linkId = null;
        public LinkDelegate? linkDelegate = null;
        public LinkPosition linkPos;
        public string chain;

        public Linkable(string chain)
        {
            this.linkPos = LinkPosition.Head;
            this.chain = chain;
        }

        public Linkable(string chain, LinkPosition linkPos, LinkDelegate? linkDelegate)
        {
            this.linkPos = linkPos;
            this.chain = chain;
            this.linkDelegate = linkDelegate;
        }
    }

    public enum LinkPosition
    {
        Head,
        Body,
        Tail
    }

    public delegate void LinkDelegate(Entities.Entity e1, Entities.Entity e2);
}
