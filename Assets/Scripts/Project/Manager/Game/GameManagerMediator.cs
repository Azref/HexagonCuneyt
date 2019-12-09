using System;
using Assets.Scripts.Core.Manager.Pool;
using Assets.Scripts.Core.Model.Game;
using Assets.Scripts.Project.Enums;
using Assets.Scripts.Project.Event;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

namespace Assets.Scripts.Project.Manager.Game
{
    public enum GameManagerEvent
    {
        GridReady,
        MakeSelection,
        NoMatch,
        WeGotMatch
    }

    public class GameManagerMediator : EventMediator
    {
        [Inject] public GameManager view { get; set; }

        [Inject] public IObjectPoolModel poolModel { get; set; }

        [Inject] public IGameModel Game { get; set; }

        public override void OnRegister()
        {
            dispatcher.AddListener(GameEvent.BuildGrid, OnBuildGrid);

			view.dispatcher.AddListener(GameManagerEvent.GridReady,OnGridReady);

			view.dispatcher.AddListener(GameManagerEvent.MakeSelection, OnMakeSelection);

            dispatcher.AddListener(GameEvent.CheckSelectionMatch, OnCheckSelectionMatch);

			view.dispatcher.AddListener(GameManagerEvent.WeGotMatch, OnWeGotMatch);

			view.dispatcher.AddListener(GameManagerEvent.NoMatch, OnNoMatch);

			view.dispatcher.AddListener(GameManagerEvent.NoMatch, OnNoMatch);

            dispatcher.AddListener(GameEvent.MatchComplete, OnMatchComplete);
        }

        private void OnBuildGrid(IEvent payload)
        {
            poolModel.Pool(PoolKey.Hexagon.ToString(), view.HexPrefab, Game.Info.GridWidth * Game.Info.GridHeight + 5);

            view.BuildGrid();
        }

        public void OnGridReady()
		{
            //Game.Status.value |= Core.Enums.GameStatus.GameIsPlaying;
            dispatcher.Dispatch(GameEvent.FixCamera);
        }

        private void OnMakeSelection(IEvent payload)
        {
            dispatcher.Dispatch(GameEvent.MakeSelection, (string)payload.data);
        }

        private void OnCheckSelectionMatch(IEvent payload)
        {
            view.CheckSelectionMatch();
        }

        private void OnWeGotMatch(IEvent payload)
        {
            dispatcher.Dispatch(GameEvent.ResetSelection);

            dispatcher.Dispatch(GameEvent.RefreshScore);
        }

        private void OnNoMatch(IEvent payload)
        {
            dispatcher.Dispatch(GameEvent.NoMatcheRotateAgain);
        }

        private void OnMatchComplete(IEvent payload)
        {
            view.CheckColumn();
        }

        public override void OnRemove()
        {
            dispatcher.RemoveListener(GameEvent.BuildGrid, OnBuildGrid);

			view.dispatcher.RemoveListener(GameManagerEvent.GridReady,OnGridReady);

            view.dispatcher.RemoveListener(GameManagerEvent.MakeSelection, OnMakeSelection);

            dispatcher.RemoveListener(GameEvent.CheckSelectionMatch, OnCheckSelectionMatch);

            view.dispatcher.RemoveListener(GameManagerEvent.WeGotMatch, OnWeGotMatch);

            view.dispatcher.RemoveListener(GameManagerEvent.NoMatch, OnNoMatch);

            dispatcher.RemoveListener(GameEvent.MatchComplete, OnMatchComplete);
        }
    }
}
