namespace Shared.Components
{
    public class Sound : Component
    {
        public Sound(string soundPath)
        {
            this.soundPath = soundPath;
        }

        public string soundPath { get; private set; }
    }
}

