namespace CodingTrackerApplication.Helpers;
internal class TimeHelper
{
    public static double CalculateDuration(DateTime startTime, DateTime endTime)
    {
        return (endTime - startTime).TotalMinutes;
    }
}