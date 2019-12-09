using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Model.Game;
using Assets.Scripts.Project.Event;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

namespace Assets.Scripts.Project.View.Hexagon
{
    public enum HexEvent
    {
        BuildAnimationCompleted,
        MatchComplete
    }

    public class HexMediator : EventMediator
    {
        [Inject] public HexView view { get; set; }

        [Inject] public IGameModel Game { get; set; }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(HexEvent.BuildAnimationCompleted, OnBuildAnimationCompleted);

            view.dispatcher.AddListener(HexEvent.MatchComplete, OnMatchComplete);

        }

        private void OnMatchComplete(IEvent payload)
        {

            Game.Status.value &= ~GameStatus.MatchAnimation;

            Game.Status.value &= ~GameStatus.Blocked;

            dispatcher.Dispatch(GameEvent.MatchComplete);
        }

        public void OnBuildAnimationCompleted(IEvent payload)
        {
            Game.Status.value |= Core.Enums.GameStatus.GameIsPlaying;
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(HexEvent.BuildAnimationCompleted, OnBuildAnimationCompleted);

            view.dispatcher.RemoveListener(HexEvent.MatchComplete, OnMatchComplete);

        }
    }
}
