using UnityEngine;
using UnityEngine.Events;

public abstract class GenericEventChannelSO : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}

public abstract class GenericEventChannelSO<T> : ScriptableObject
{
    public UnityAction<T> OnEventRaised;

    public void RaiseEvent(T parameter)
    {
        OnEventRaised?.Invoke(parameter);
    }
}
