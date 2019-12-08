using UnityEngine;

namespace Assets.Scripts.Core.Model.Game
{
    public class GameModel : IGameModel
    {
        private RV_GameStatus _status;

        private RV_GameInfo _info;

        [PostConstruct]
        public void OnPostConstruct()
        {
            _status = Resources.Load<RV_GameStatus>("RuntimeVariables/GameStatus");
            _info = Resources.Load<RV_GameInfo>("RuntimeVariables/GameInfo");
            _info.HexDict.Clear();
            _info.HexList.Clear();
            _info.SelectedHexs.Clear();
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

        public RV_GameInfo Info
        {
            get
            {
                if (_info == null)
                    OnPostConstruct();

                return _info;
            }
        }

        public void Clear()
        {
            _info.HexDict.Clear();
            _info.HexList.Clear();
            _info.SelectedHexs.Clear();
        }
    }
}