// -------------------------------------------------------------------------------------------------
// CodingTrackerApplication.Models.Goal
// -------------------------------------------------------------------------------------------------
// Represents a coding goal with properties for the goal amount (in minutes), start date, and end date. 
// This model is used for setting and tracking user-specific coding goals over a defined period.
// -------------------------------------------------------------------------------------------------

namespace CodingTrackerApplication.Models;
public class Goal
{
    public int UserId { get; set; }
    public int GoalAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
