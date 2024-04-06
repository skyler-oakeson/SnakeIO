namespace Shared.Components
{
    public class Appearance : Component
    {
        public Appearance(string texturePath)
        {
            this.texturePath = texturePath;
        }

        public string texturePath { get; private set; }
    }
}

