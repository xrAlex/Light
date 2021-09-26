namespace Light.Templates.Entities
{
    internal readonly struct RGBMask
    {
        public double Red { get; }

        public double Green { get; }

        public double Blue { get; }

        public RGBMask(double red, double green, double blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
}
