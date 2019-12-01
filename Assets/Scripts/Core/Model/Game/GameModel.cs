using UnityEngine;

namespace Assets.Scripts.Core.Model.Game
{
    public class GameModel : IGameModel
    {
        private RV_GameStatus _status;

        private RV_Hexagon _hexagon;

        private RV_Grid _grid;

        [PostConstruct]
        public void OnPostConstruct()
        {
            _status = Resources.Load<RV_GameStatus>("RuntimeVariables/GameStatus");
            _hexagon = Resources.Load<RV_Hexagon>("RuntimeVariables/HexagonInfo");
            _grid = Resources.Load<RV_Grid>("RuntimeVariables/GridInfo");
            _grid.HexDict.Clear();
            _grid.HexList.Clear();
        }

        public RV_GameStatus Status
        {
            get
            {
                if (_status == null)
                    OnPostConstruct();

                return _status;
            }
        }

        public RV_Hexagon Hexagon
        {
            get
            {
                if (_hexagon == null)
                    OnPostConstruct();

                return _hexagon;
            }
        }

        public RV_Grid Grid
        {
            get
            {
                if (_grid == null)
                    OnPostConstruct();

                return _grid;
            }
        }

        public void Clear()
        {
            _grid.HexDict.Clear();
            _grid.HexList.Clear();
        }
    }
}