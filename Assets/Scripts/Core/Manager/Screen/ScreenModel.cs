using System.Collections.Generic;
using Assets.Scripts.Core.Enums;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Assets.Scripts.Core.Manager.Screen
{
  public class ScreenModel : IScreenModel
  {
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      IgnoreHistory = new List<string>()
      {
        AppScreensCore.LoadingScreen,
        "ConfirmPanel",
        "AlertPanel",
        "InfoPanel"
      };
      History = new List<IPanelVo>();
    }

    public List<string> IgnoreHistory { get; set; }

    public List<IPanelVo> History { get; set; }

    public string GetHistoryData()
    {
      string data = string.Empty;
      foreach (IPanelVo panelVo in History)
      {
        data += panelVo.Name + ",";
      }

      return data;
    }
  }
}