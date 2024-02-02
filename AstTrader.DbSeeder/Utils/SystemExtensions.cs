namespace AstTrader.DbSeeder.Utils
{
    public static class SystemExtensions
    {
        public static decimal Percentage(decimal initial, decimal current)
        {
            if (initial == 0) return 0;
            var pct = (current - initial) / initial;
            return Math.Round(pct * 100, 2);
        }

        public static bool IsInRange(decimal value, decimal min, decimal max)
        {
            return value >= min && value <= max;
        }

        public static decimal CaculateCAGR(decimal value, int days)
        {
            return Math.Round(365M / days * value, 2);
        }

        public static DateTime RemoveTime(this DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime).ToDateTime(TimeOnly.MinValue);
        }
    }
}
