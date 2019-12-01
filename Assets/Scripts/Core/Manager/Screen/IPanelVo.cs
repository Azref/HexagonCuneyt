using Assets.Scripts.Core.Enums;

namespace Assets.Scripts.Core.Manager.Screen
{
    public delegate void PanelCallback();

    public interface IPanelVo
    {
        /// <summary>
        /// Name of the panel
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Type of the panel
        /// </summary>
        GameStatus Type { get; set; }

        /// <summary>
        /// Index of layer to instantiate
        /// </summary>
        int LayerIndex { get; set; }

        /// <summary>
        /// If it is true, all panels in all layers will be removed
        /// </summary>
        bool RemoveAll { get; set; }

        /// <summary>
        /// If it is true, panel cannot be canceled by user. Ex. download screen
        /// </summary>
        bool NotCancellable { get; set; }

        /// <summary>
        /// If it is true, all panels in the layer will be removed
        /// </summary>
        bool RemoveLayer { get; set; }

        /// <summary>
        /// If it is true, It will check the layer whether contains same panel. 
        /// If it has same panel, old one will be deleted and new panel will be added
        /// </summary>
        bool RemoveSamePanels { get; set; }
    }
}