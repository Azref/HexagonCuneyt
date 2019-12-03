using System;
using Assets.Scripts.Core.Model.Game;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Scripts.Project.View.Hexagon
{
    public enum HexagonEvent
    {
        Press
    }

    public class HexMediator : EventMediator
    {
        [Inject] public HexView view { get; set; }

        [Inject] public IGameModel Game { get; set; }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(HexagonEvent.Press, OnPress);

        }


        public void OnPress(IEvent payload)
        {

        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(HexagonEvent.Press, OnPress);
        }
    }
}
