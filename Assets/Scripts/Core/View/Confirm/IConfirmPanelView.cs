using Assets.Scripts.Core.Manager.Screen;

namespace Assets.Scripts.Core.View.Confirm
{
    public enum ConfirmPanelEvent
    {
        Cancel,
        Confirm
    }

    public interface IConfirmPanelView : IPanelView
    {
        /// <summary>
        /// Title of confirm panel
        /// </summary>
        string Title { set; }

        /// <summary>
        /// Description of confirm panel
        /// </summary>
        string Description { set; }

        /// <summary>
        /// Confirm button text
        /// </summary>
        string ConfirmButtonLabel { set; }

        /// <summary>
        /// Cancel button text
        /// </summary>
        string CancelButtonLabel { set; }

        /// <summary>
        /// Icon name on resources folder
        /// </summary>
        string IconName { set; }
    }
}