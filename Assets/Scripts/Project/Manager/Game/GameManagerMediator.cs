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
        MakeSelection
    }

    public class GameManagerMediator : EventMediator
    {
        [Inject] public GameManager view { get; set; }

        [Inject] public IObjectPoolModel poolModel { get; set; }

        [Inject] public IGameModel Game { get; set; }

        public override void OnRegister()
        {
			view.dispatcher.AddListener(GameManagerEvent.GridReady,OnGridReady);

			view.dispatcher.AddListener(GameManagerEvent.MakeSelection, OnMakeSelection);

            dispatcher.AddListener(GameEvent.BuildGrid, OnBuildGrid);

            dispatcher.AddListener(GameEvent.CheckSelectionMatch, OnCheckSelectionMatch);
        }

        public void OnGridReady()
		{
            Game.Status.value |= Core.Enums.GameStatus.GameIsPlaying;
            dispatcher.Dispatch(GameEvent.FixCamera);
        }

        private void OnMakeSelection(IEvent payload)
        {
            dispatcher.Dispatch(GameEvent.MakeSelection, (string)payload.data);
        }

        private void OnBuildGrid(IEvent payload)
        {
            poolModel.Pool(PoolKey.Hexagon.ToString(), view.HexPrefab, Game.Grid.width * Game.Grid.height + 5);

            view.BuildGrid();
        }

        private void OnCheckSelectionMatch(IEvent payload)
        {
            view.CheckSelectionMatch();
        }

        public override void OnRemove()
        {
			view.dispatcher.RemoveListener(GameManagerEvent.GridReady,OnGridReady);

            view.dispatcher.RemoveListener(GameManagerEvent.MakeSelection, OnMakeSelection);

            dispatcher.RemoveListener(GameEvent.BuildGrid, OnBuildGrid);

            dispatcher.RemoveListener(GameEvent.CheckSelectionMatch, OnCheckSelectionMatch);
        }
    }
}
