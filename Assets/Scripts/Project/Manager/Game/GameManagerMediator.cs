using Assets.Scripts.Core.Manager.Pool;
using Assets.Scripts.Core.Model.Game;
using Assets.Scripts.Project.Enums;
using Assets.Scripts.Project.Event;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

namespace Assets.Scripts.Project.Manager.Game
{
    public enum HexManagerEvent
    {
        GridReady
    }

    public class GameManagerMediator : EventMediator
    {
        [Inject] public GameManager view { get; set; }

        [Inject] public IObjectPoolModel poolModel { get; set; }

        [Inject] public IGameModel Game { get; set; }

        public override void OnRegister()
        {
            view.Grid = Game.Grid;

            view.Hex = Game.Hexagon;

			view.dispatcher.AddListener(HexManagerEvent.GridReady,OnGridReady);

            dispatcher.AddListener(GameEvent.BuildGrid, OnBuildGrid);
        }

        private void OnBuildGrid(IEvent payload)
        {
            poolModel.Pool(PoolKey.Hexagon.ToString(), view.HexPrefab, Game.Grid.width * Game.Grid.height + 5);

            view.BuildGrid();
        }

        public void OnGridReady()
		{
            dispatcher.Dispatch(GameEvent.FixCamera);
        }

        public override void OnRemove()
        {
			view.dispatcher.RemoveListener(HexManagerEvent.GridReady,OnGridReady);

            dispatcher.RemoveListener(GameEvent.BuildGrid, OnBuildGrid);
        }
    }
}
