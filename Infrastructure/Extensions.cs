namespace Sparky.Infrastructure
{
    internal sealed class Extensions
    {
        public static double Lerp(double targetValue, double startValue, double multiplier)
        {
            return targetValue + (startValue - targetValue) * multiplier;
        }
    }
}
