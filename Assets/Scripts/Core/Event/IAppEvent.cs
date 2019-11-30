using System.Collections.Generic;

public interface IAppEvent
{
    void Trigger();

    void RegisterListener(AE_Listener listener);

    void UnregisterListener(AE_Listener listener);
}