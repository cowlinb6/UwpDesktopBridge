namespace DesktopApp.HotKeys
{
    /// <summary>
    /// Represents a hot key combination
    /// </summary>
    public class HotKeyCombination
    {
        public string Description { get; set; }

        public KeyModifiers Modifiers { get; set; }

        public Keys Key { get; set; }

        public bool Enabled { get; set; }
    }
}
