using Assets.Scripts.Core.Manager.Screen;
using Assets.Scripts.Core.Enums;
using strange.extensions.command.impl;
using Assets.Scripts.Core.Model.Game;

namespace Assets.Screen.Home.Scripts.Controller
{
    public class BuildGridCommand : EventCommand
    {
        [Inject] public IGameModel Game { get; set; }

        public override void Execute()
        {
            // When retain is called command will not be finished until release function 
            //Retain();

            dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo
            {
                Name = GameScreens.Home,
                Type = GameStatus.Home
            });

            // Complete the command with Release function
            //Release();
        }
    }
}
