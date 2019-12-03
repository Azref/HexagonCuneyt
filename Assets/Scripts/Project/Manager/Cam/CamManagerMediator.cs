using System;
using Assets.Scripts.Core.Model.Game;
using Assets.Scripts.Project.Event;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Scripts.Project.View.Cam
{
    public enum CamManagerEvent
    {
        Press
    }

    public class CamManagerMediator : EventMediator
    {
        [Inject] public CamManager view { get; set; }

        [Inject] public IGameModel Game { get; set; }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(CamManagerEvent.Press, OnPress);
            dispatcher.AddListener(GameEvent.FixCamera, OnFixCamera);
        }

        private void OnFixCamera(IEvent payload)
        {
            view.FixCam();
            Debug.Log("CamManagerMediator / OnFixCamera");
        }

        public void OnPress(IEvent payload)
        {

        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(CamManagerEvent.Press, OnPress);
            dispatcher.RemoveListener(GameEvent.FixCamera, OnFixCamera);
        }
    }
}
