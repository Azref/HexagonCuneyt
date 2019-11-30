using UnityEngine;
using UnityEngine.Events;

public class AE_Listener : MonoBehaviour
{
    public AE_Root Event;

    public UnityEvent Response;

    public void Register()
    {
        Event.RegisterListener(this);
    }

    public void Unregister()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventTriggered()
    {
        Response.Invoke();
    }
    private void OnEnable()
    {
        Register();
    }

    private void OnDisable()
    {
        Unregister();
    }
}