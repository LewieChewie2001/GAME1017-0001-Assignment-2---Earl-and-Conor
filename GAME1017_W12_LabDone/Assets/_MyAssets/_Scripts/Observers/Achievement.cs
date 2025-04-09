using UnityEngine;

// TODO: Add to Week 12 lab.
public class Achievement
{
    public string Name { get; private set; }
    public bool IsUnlocked { get; private set; }
    private int progress;
    private int maxProgress;

    public Achievement(string name, int maxprog = 0)
    {
        Name = name;
        IsUnlocked = false;
        progress = 0;
        maxProgress = maxprog;
    }

    public void Progress()
    {
        if (IsUnlocked) return; // Early exit, because why not?
        
        if (++progress >= maxProgress)
        {
            IsUnlocked = true;
            Debug.Log(Name + " achievement unlocked!");
        }
    }
}