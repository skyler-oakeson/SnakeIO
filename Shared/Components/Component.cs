namespace Shared.Components
{
    public abstract class Component
    {
        public abstract void Serialize(ref List<byte> data);
    }
}
