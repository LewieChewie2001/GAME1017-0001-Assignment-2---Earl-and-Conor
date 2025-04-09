// TODO: Add to Week 12 lab.
public enum Event
{
    PlayerJumped,
    FiveRolls,
}

public interface IObserver
{ 
    void OnNotify(Event gameEvent);
}

