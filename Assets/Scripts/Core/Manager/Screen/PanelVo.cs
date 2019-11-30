using Assets.Scripts.Core.Enums;

namespace Assets.Scripts.Core.Manager.Screen
{
    public class PanelVo : IPanelVo
    {
        public PanelVo()
        {
            LayerIndex = 0;
            RemoveAll = false;
            RemoveLayer = true;
            RemoveSamePanels = true;
        }

        public string Name { get; set; }

        public int LayerIndex { get; set; }

        public bool RemoveAll { get; set; }

        public bool RemoveLayer { get; set; }

        public bool RemoveSamePanels { get; set; }

        public bool NotCancellable { get; set; }

        public PanelCallback OnCancel { get; set; }

        public AppStatus Type { get; set; }
    }
}