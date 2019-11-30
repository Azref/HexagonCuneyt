using strange.extensions.mediation.impl;
using Assets.Scripts.Core.Manager.Screen;

namespace Assets.Scripts.Project.View.Home
{
    public class HomeScreenView : EventView, IPanelView
    {
        /// <summary>
        /// Panel info
        /// </summary>
        public IPanelVo vo { get; set; }

        /// <summary>
        /// dispatching event to mediator
        /// </summary>
        public void OnBackClick()
        {
            dispatcher.Dispatch(HomeScreenEvent.Back);
        }
    }
}
