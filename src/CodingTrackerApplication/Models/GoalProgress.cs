// -------------------------------------------------------------------------------------------------
// CodingTrackerApplication.Models.GoalProgress
// -------------------------------------------------------------------------------------------------
// Represents the progress of a coding goal. Includes the total duration logged, the goal amount, 
// progress percentage, and the daily goal required to meet the target. This model is used for 
// monitoring and reporting a user's progress towards their coding goals.
// -------------------------------------------------------------------------------------------------

namespace CodingTrackerApplication.Models;
public class GoalProgress
{
    public int TotalDuration { get; set; }
    public int GoalAmount { get; set; }
    public double ProgressPercentage { get; set; }
    public double DailyGoal { get; set; }
}