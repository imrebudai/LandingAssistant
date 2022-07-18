namespace LandingAssistant
{
    public readonly struct LandingPlatform
    {
        private readonly Area _area;
        public int Height { get => _area.Height; }
        public int Width { get => _area.Width; }
        public LandingPlatform(Area area)
        {
            _area = area;
        }
    }
}
