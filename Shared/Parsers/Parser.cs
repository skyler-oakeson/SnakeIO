namespace Shared.Parsers
{
    public abstract class Parser
    {
        public abstract void Parse(ref byte[] data, ref int offset);
    }
}
