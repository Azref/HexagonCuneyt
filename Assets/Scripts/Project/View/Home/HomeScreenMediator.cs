using System;
using strange.extensions.mediation.impl;

namespace Assets.Scripts.Project.View.Home
{
    /// <summary>
    /// Events releated to this screen
    /// </summary>
    public enum HomeScreenEvent
    {
        Back
    }

    public class HomeScreenMediator : EventMediator
    {
        [Inject]
        public HomeScreenView view { get; set; }

        /// <summary>
        /// Works after all bindings are completed. 
        /// Useful to attach listeners
        /// After Awake 
        /// Before Start. 
        /// </summary>
        public override void OnRegister()
        {
            // Listen back event from view
            view.dispatcher.AddListener(HomeScreenEvent.Back, OnBack);
        }

        /// <summary>
        /// Works when back event is called
        /// </summary>
        public void OnBack()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Works when connected gameobject is destroyed. 
        /// Useful to remove listeners
        /// Before OnDestroy method
        /// </summary>
        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(HomeScreenEvent.Back, OnBack);
        }
    }
}
