using System.Collections.Generic;

// TODO: Add to Week 12 lab.
public interface IReceiver
{
    List<IObserver> Observers { get; }
}

public static class ReceiverExtensions
{
    public static void AddObserver(this IReceiver receiver, IObserver observer)
    {
        receiver.Observers.Add(observer);
    }

    public static void RemoveObserver(this IReceiver receiver, IObserver observer)
    {
        receiver.Observers.Remove(observer);
    }

    public static void NotifyObservers(this IReceiver receiver, Event gameEvent)
    {
        foreach (var observer in receiver.Observers)
        {
            observer.OnNotify(gameEvent);
        }
    }
}
