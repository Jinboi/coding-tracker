// -------------------------------------------------------------------------------------------------
// CodingTrackerApplication.GoalController
// -------------------------------------------------------------------------------------------------
// Manages user inputs and interactions related to goal settings and progress tracking. 
// -------------------------------------------------------------------------------------------------

using CodingTrackerApplication.Helpers.UtilityHelpers;
using CodingTrackerApplication.Models;
using CodingTrackerApplication.Services;
using CodingTrackerApplication.Constants;
using CodingTrackerApplication.Helpers.UserInputHelpers;

namespace CodingTrackerApplication.Controllers;
internal class GoalController
{
    private readonly CodingTrackerService _codingTrackerService = new();
    public void SetGoal()
    {
        var userGoal = getGoalInputs(); // Get user inputs and create a Goal object

        _codingTrackerService.SetGoal(userGoal); // Pass the Goal object

        Console.WriteLine("Goal has been set successfully.");
    }

    private Goal getGoalInputs()
    {
        Console.WriteLine("Enter User ID:");
        int userId = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine("Enter Goal Amount (in minutes):");
        int goalAmount = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine("Enter Start Date (yyyy-MM-dd):");
        DateTime startDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString("yyyy-MM-dd"));

        Console.WriteLine("Enter End Date (yyyy-MM-dd):");
        DateTime endDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString("yyyy-MM-dd"));

        // Create and return a new Goal object with UserId
        return new Goal
        {
            UserId = userId,
            GoalAmount = goalAmount,
            StartDate = startDate,
            EndDate = endDate
        };
    }

    public void ViewGoalProgress()
    {
        int userId = UserInputHelper.getUserIdPrompt();

        var progress = _codingTrackerService.GetGoalProgress(userId);

        if (progress == null)
        {
            Console.WriteLine(Constant.NoGoalFoundMessage);
            return;
        }

        DisplayHelper.DisplayGoalProgress(progress);
    }
}
