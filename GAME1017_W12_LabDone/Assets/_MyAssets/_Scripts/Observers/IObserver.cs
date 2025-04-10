// TODO: Add to Week 12 lab.
public enum Event
{
    PlayerJumped,
    FiveRolls,
    JumpedOver10Obstacles,
    RollUnder10Obstacles
}

public interface IObserver
{ 
    void OnNotify(Event gameEvent);
}

