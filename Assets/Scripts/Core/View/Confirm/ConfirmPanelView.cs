using Assets.Scripts.Core.Manager.Screen;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core.View.Confirm
{
    public class ConfirmPanelView : AnimatedView, IConfirmPanelView
    {
        /// <summary>
        /// Title object
        /// </summary>
        public TextMeshProUGUI TitleLabel;

        /// <summary>
        /// Description object
        /// </summary>
        public TextMeshProUGUI DescriptionLabel;

        /// <summary>
        /// Cancel button object
        /// </summary>
        public TextMeshProUGUI CancelButtonText;

        /// <summary>
        /// Confirm button object
        /// </summary>
        public TextMeshProUGUI ConfirmButtonText;

        /// <summary>
        /// Confirm panel icon
        /// </summary>
        public Image Icon;

        /// <summary>
        /// Cancel event listener
        /// </summary>
        public void OnCancelClick()
        {
            DispatchDelayed(ConfirmPanelEvent.Cancel);
        }

        /// <summary>
        /// Confirm event listener
        /// </summary>
        public void OnConfirmClick()
        {
            DispatchDelayed(ConfirmPanelEvent.Confirm);
        }

        [UsedImplicitly]
        private void Update()
        {
            //Detection of escape button
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !vo.NotCancellable)
                DispatchDelayed(ConfirmPanelEvent.Cancel);
        }

        /// <summary>
        /// Title of confirm panel
        /// </summary>
        public string Title
        {
            set
            {
                if (TitleLabel != null)
                    TitleLabel.text = value;

            }
        }

        /// <summary>
        /// Description of confirm panel
        /// </summary>
        public string Description
        {
            set
            {
                if (DescriptionLabel != null)
                    DescriptionLabel.text = value;
            }
        }

        /// <summary>
        /// Confirm button text
        /// </summary>
        public string ConfirmButtonLabel
        {
            set
            {
                if (ConfirmButtonText != null)
                    ConfirmButtonText.text = value;
            }
        }

        /// <summary>
        /// Cancel button text
        /// </summary>
        public string CancelButtonLabel
        {
            set
            {
                if (CancelButtonText != null)
                    CancelButtonText.text = value;
            }
        }

        /// <summary>
        /// Icon name in resources folder
        /// </summary>
        public string IconName
        {
            set
            {
                if (Icon != null)
                {
                    Icon.sprite = Resources.Load<Sprite>("GUI/Sprites/" + value);
                    Icon.SetNativeSize();
                }
            }
        }

        /// <summary>
        /// Connected PanelVo
        /// </summary>
        public IPanelVo vo { get; set; }
    }
}
