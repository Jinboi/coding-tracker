// -------------------------------------------------------------------------------------------------
// CodingTrackerApplication.Helpers.TimeHelper
// -------------------------------------------------------------------------------------------------
// Offers utility functions for time-based calculations, such as calculating the duration 
// between two `DateTime` values in minutes.
// -------------------------------------------------------------------------------------------------

namespace CodingTrackerApplication.Helpers;
internal class TimeHelper
{
    public static double CalculateDuration(DateTime startTime, DateTime endTime)
    {
        return (endTime - startTime).TotalMinutes;
    }
}