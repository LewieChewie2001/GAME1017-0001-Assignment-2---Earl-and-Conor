using System.Collections.Generic;

// TODO: Add to Week 12 lab.
public class AchievementObserver : IObserver
{
    private Dictionary<Event, Achievement> achievements;

    public AchievementObserver()
    {
        achievements = new Dictionary<Event, Achievement>();

        achievements[Event.PlayerJumped] = new Achievement("First Jump");
        achievements[Event.FiveRolls] = new Achievement("Five Rolls", 5);
    }

    public void OnNotify(Event gameEvent)
    {
        if (achievements.ContainsKey(gameEvent) && !achievements[gameEvent].IsUnlocked)
        {
            achievements[gameEvent].Progress();
        }
    }
}

