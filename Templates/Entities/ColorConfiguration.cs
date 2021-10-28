namespace Sparky.Templates.Entities
{
    internal readonly struct ColorConfiguration
    {
        public double Brightness { get; }
        public double ColorTemperature { get; }

        public ColorConfiguration(double colorTemperature, double brightness)
        {
            Brightness = brightness;
            ColorTemperature = colorTemperature;
        }
    }
}
