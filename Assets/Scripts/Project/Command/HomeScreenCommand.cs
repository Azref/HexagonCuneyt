using Assets.Scripts.Core.Manager.Screen;
using Assets.Scripts.Core.Enums;
using strange.extensions.command.impl;

namespace Assets.Tests.Screen.Home.Scripts.Controller
{
    /// <summary>
    /// Open home screen command
    /// </summary>
    public class HomeScreenCommand : EventCommand
    {
        public override void Execute()
        {
            // When retain is called command will not be finished until release function 
            //Retain();

            dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo
            {
                Name = AppScreens.Home,
                Type = AppStatus.Home
            });

            // Complete the command with Release function
            //Release();
        }
    }
}
