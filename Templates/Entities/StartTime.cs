namespace Sparky.Templates.Entities
{
    internal readonly struct StartTime
    {
        public int Hour { get; }
        public int Minute { get; }

        public StartTime(int hour, int minute)
        {
            Hour = hour;
            Minute = minute;
        }
    }
}
