using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
//using Service.Localization;

namespace Assets.Scripts.Core.View.Confirm
{
    public class ConfirmPanelMediator : EventMediator
    {
        /// <summary>
        /// View injection
        /// </summary>
        [Inject]
        public IConfirmPanelView view { get; set; }

        //[Inject]
        //public ILocalizationService localizationService { get; set; }

        /// <summary>
        /// Panel details
        /// </summary>
        private ConfirmPanelVo vo
        {
            get { return view.vo as ConfirmPanelVo; }
        }

        /// <summary>
        /// Works after all bindings are completed. 
        /// Useful to attach listeners
        /// After Awake 
        /// Before Start. 
        /// </summary>
        public override void OnRegister()
        {
            // add view listeners
            view.dispatcher.AddListener(ConfirmPanelEvent.Confirm, OnConfirm);
            view.dispatcher.AddListener(ConfirmPanelEvent.Cancel, OnCancel);

            //view.Title = localizationService.GetText(vo.Title);
            //view.Description = localizationService.GetText(vo.Description);
            //view.ConfirmButtonLabel = localizationService.GetText(vo.ButtonLabel);
            //view.CancelButtonLabel = localizationService.GetText(vo.CancelButtonLabel);
            view.IconName = vo.Icon;
        }

        /// <summary>
        /// Confirm event callback
        /// </summary>
        private void OnConfirm(IEvent payload)
        {
            if (vo.OnConfirm != null)
                vo.OnConfirm();
            Destroy(gameObject);
        }

        /// <summary>
        /// Cancel event callback
        /// </summary>
        private void OnCancel(IEvent payload)
        {
            if (vo.OnCancel != null)
                vo.OnCancel();
            Destroy(gameObject);
        }

        /// <summary>
        /// Works when connected gameobject is destroyed. 
        /// Useful to remove listeners
        /// Before OnDestroy method
        /// </summary>
        public override void OnRemove()
        {
            // remove view listeners
            view.dispatcher.RemoveListener(ConfirmPanelEvent.Confirm, OnConfirm);
            view.dispatcher.RemoveListener(ConfirmPanelEvent.Cancel, OnCancel);
        }
    }
}