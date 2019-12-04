using Assets.Scripts.Project.Event;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

namespace Assets.Scripts.Project.Manager.Selection

{
    public enum LineManagerEvent
    {
    }

    public class SelectionManagerMediator : EventMediator
    {
        [Inject] public SelectionManager view { get; set; }

        //[Inject] public IGameModel Game { get; set; }

        public override void OnRegister()
        {
            dispatcher.AddListener(GameEvent.MakeSelection, OnMakeSelection);
        }

        private void OnMakeSelection(IEvent payload)
        {
            view.SelectHex((string)payload.data);
        }

        public override void OnRemove()
        {
            dispatcher.RemoveListener(GameEvent.MakeSelection, OnMakeSelection);
        }
    }
}
