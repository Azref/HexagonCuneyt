#if UNITY_EDITOR || DEBUG
using Assets.Scripts.Core.Manager.Screen;
using Assets.Scripts.Core.Enums;
using strange.extensions.command.impl;

namespace Assets.Tests.Screen.Home.Scripts.Controller
{
    /// <summary>
    /// Opens test home screen. This class can be used for initializing mock datas
    /// </summary>
    public class HomeScreenTestCommand : EventCommand
    {
        public override void Execute()
        {
            dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo
            {
                Name = GameScreens.Home,
                Type = GameStatus.Home
            });
        }
    }
}
#endif