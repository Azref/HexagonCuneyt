using System;
using Assets.Scripts.Core.Model.Game;
using Assets.Scripts.Project.Event;
using strange.extensions.mediation.impl;

namespace Assets.Scripts.Project.View.Home
{
    
    public class HomeScreenMediator : EventMediator
    {
        [Inject] public HomeScreenView view { get; set; }

        [Inject] public IGameModel Game { get; set; }

        public override void OnRegister()
        {
            dispatcher.AddListener(GameEvent.RefreshScore, OnRefreshScore);
        }

        public void OnRefreshScore()
        {
            Game.Score.value += Game.Info.MatchList.Count * 5;

            view.ScoreTxt.text = Game.Score.value.ToString();
        }

        public override void OnRemove()
        {
            dispatcher.RemoveListener(GameEvent.RefreshScore, OnRefreshScore);
        }
    }
}
