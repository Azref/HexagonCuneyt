using System;
using Assets.Scripts.Project.Event;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

namespace Assets.Scripts.Project.Manager.Selection
{
    public enum SelectionManagerEvent
    {
        RotComplete
    }

    public class SelectionManagerMediator : EventMediator
    {
        [Inject] public SelectionManager view { get; set; }

        //[Inject] public IGameModel Game { get; set; }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(SelectionManagerEvent.RotComplete, OnRotComplete);

            dispatcher.AddListener(GameEvent.MakeSelection, OnMakeSelection);

            dispatcher.AddListener(GameEvent.NoMatcheRotateAgain, OnNoMatcheRotateAgain);

            dispatcher.AddListener(GameEvent.ResetSelection, OnResetSelection);
        }

        private void OnRotComplete(IEvent payload)
        {
            dispatcher.Dispatch(GameEvent.CheckSelectionMatch);
        }

        private void OnMakeSelection(IEvent payload)
        {
            view.SelectHex((string)payload.data);
        }

        private void OnNoMatcheRotateAgain(IEvent payload)
        {
            view.MakeRotation();
        }

        private void OnResetSelection(IEvent payload)
        {
            view.ResetSelection();
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(SelectionManagerEvent.RotComplete, OnRotComplete);

            dispatcher.RemoveListener(GameEvent.MakeSelection, OnMakeSelection);

            dispatcher.RemoveListener(GameEvent.NoMatcheRotateAgain, OnNoMatcheRotateAgain);

            dispatcher.AddListener(GameEvent.ResetSelection, OnResetSelection);
        }
    }
}
