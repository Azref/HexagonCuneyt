using System;
using Assets.Scripts.Core.Model.Game;
using Assets.Scripts.Project.Event;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Scripts.Project.View.Hexagon
{
    public enum HexEvent
    {
        BuildAnimationCompleted
    }

    public class HexMediator : EventMediator
    {
        [Inject] public HexView view { get; set; }

        [Inject] public IGameModel Game { get; set; }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(HexEvent.BuildAnimationCompleted, OnBuildAnimationCompleted);
        }

        public void OnBuildAnimationCompleted(IEvent payload)
        {
            Game.Status.value |= Core.Enums.GameStatus.GameIsPlaying;
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(HexEvent.BuildAnimationCompleted, OnBuildAnimationCompleted);
        }
    }
}
