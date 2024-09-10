// -------------------------------------------------------------------------------------------------
// CodingTrackerApplication.Models.CodingSession
// -------------------------------------------------------------------------------------------------
// Represents a coding session record with properties for session ID, start time, end time, and 
// duration. This model is used for database operations involving the tracking of coding sessions.
// -------------------------------------------------------------------------------------------------

namespace CodingTrackerApplication.Models;
public class CodingSession
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Duration { get; set; }
}
