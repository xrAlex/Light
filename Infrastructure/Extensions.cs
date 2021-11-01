using System;

namespace Sparky.Infrastructure
{
    public static class MathExtension
    {
        public static double Lerp(this double startValue, double targetValue, double multiplier)
        {
            return targetValue * (1 - multiplier) + startValue * multiplier;
        }

        public static T ClampMin<T>(this T value, T min) where T : IComparable<T>
        {
            // If value is less than min - return min
            if (value.CompareTo(min) <= 0)
                return min;

            // Otherwise - return value
            return value;
        }

        public static T ClampMax<T>(this T value, T max) where T : IComparable<T>
        {
            // If value is greater than max - return max
            if (value.CompareTo(max) >= 0)
                return max;

            // Otherwise - return value
            return value;
        }
    }
}

