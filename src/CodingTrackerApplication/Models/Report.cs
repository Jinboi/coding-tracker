// -------------------------------------------------------------------------------------------------
// CodingTrackerApplication.Models.Report
// -------------------------------------------------------------------------------------------------
// A model for a report about coding sessions. 
// -------------------------------------------------------------------------------------------------

namespace CodingTrackerApplication.Models;
public class Report
{
    public int TotalDuration { get; set; }
    public double AverageDuration { get; set; }

    public Report(int totalDuration, double averageDuration)
    {
        TotalDuration = totalDuration;
        AverageDuration = averageDuration;
    }
}