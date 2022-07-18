using System;

namespace LandingAssistant
{
    public readonly struct Area
    {
        public int Height { get; }
        public int Width { get; }
        public Area(int height, int width)
        {
            if (height < 1 || width < 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            Height = height;
            Width = width;
        }
    }
}
