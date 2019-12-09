using strange.extensions.mediation.impl;
using Assets.Scripts.Core.Manager.Screen;
using TMPro;

namespace Assets.Scripts.Project.View.Home
{
    public class HomeScreenView : EventView, IPanelView
    {
        public IPanelVo vo { get; set; }

        public TextMeshProUGUI ScoreTxt;
        
    }
}
