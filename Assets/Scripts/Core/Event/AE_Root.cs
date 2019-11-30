using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class AE_Root : ScriptableObject, IAppEvent
{
    private List<AE_Listener> _listeners = new List<AE_Listener>();

    [Button(ButtonSizes.Large)]
    public void Trigger()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
            _listeners[i].OnEventTriggered();
    }

    public void RegisterListener(AE_Listener listener)
    {
        if (_listeners.Contains(listener))
            Debug.LogWarning("Already Registered AppEventListener !");
        else
            _listeners.Add(listener);
    }

    public void UnregisterListener(AE_Listener listener)
    {
        _listeners.Remove(listener);
    }
}