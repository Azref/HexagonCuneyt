using Assets.Scripts.Core.Manager.Pool;
using Assets.Scripts.Core.View;
using Assets.Scripts.Project.Enums;
using Assets.Scripts.Project.Extension;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Project.Manager.Hexagon
{
    public class HexView : PoolView, IPoolable
    {
        public RV_Grid GridInfo;

        public RV_Hexagon HexInfo;

        public HexagonColor color;

        public int x;

        public int y;

        [ShowInInspector]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        public Dictionary<HexNeighbor, HexView> Neighbors = new Dictionary<HexNeighbor, HexView>();

        /// ██████████████████████████████████████████████████████████████████████████
        /// <summary>████████████████████████ Setup ████████████████████████</summary>
        /// ██████████████████████████████████████████████████████████████████████████
        public void Setup(int hexX, int hexY, HexagonColor hexC = HexagonColor.White)
        {
            x = hexX;

            y = hexY;
 
            color = hexC;

            transform.name = "Hexagon_" + x + "_" + y;

            transform.localPosition = new Vector3(x * HexInfo.DistX, y * HexInfo.Height);

            if (x % 2 == 1) transform.Translate(0, HexInfo.Height *.5f, 0);

            if (hexC == HexagonColor.White)
                color = (HexagonColor)UnityEngine.Random.Range(1, Enum.GetValues(typeof(HexagonColor)).Length);

            GetComponent<SpriteRenderer>().color = HexInfo.Colors[color];

            FixNeighbors();
        }

        private void FixNeighbors()
        {

            Debug.Log("Hex"+x+"-"+y+ ": "+(x % 2 == 0));

            GridInfo.HexDict.CheckAndAssign(this, HexNeighbor.TopHex, x + "-" + (y + 1));
            GridInfo.HexDict.CheckAndAssign(this, HexNeighbor.BotHex, x + "-" + (y - 1));

            if (x % 2 == 0)
            {
                GridInfo.HexDict.CheckAndAssign(this, HexNeighbor.RTHex, (x + 1) + "-" + y);
                GridInfo.HexDict.CheckAndAssign(this, HexNeighbor.RBHex, (x + 1) + "-" + (y - 1));

                GridInfo.HexDict.CheckAndAssign(this, HexNeighbor.LTHex, (x - 1) + "-" + y);
                GridInfo.HexDict.CheckAndAssign(this, HexNeighbor.LBHex, (x - 1) + "-" + (y - 1));
            }
            else
            {
                GridInfo.HexDict.CheckAndAssign(this, HexNeighbor.LTHex, (x - 1) + "-" + (y + 1));
                GridInfo.HexDict.CheckAndAssign(this, HexNeighbor.LBHex, (x - 1) + "-" + y);

                GridInfo.HexDict.CheckAndAssign(this, HexNeighbor.RTHex, (x + 1) + "-" + (y + 1));
                GridInfo.HexDict.CheckAndAssign(this, HexNeighbor.RBHex, (x + 1) + "-" + y);
            }
        }


        /// ██████████████████████████████████████████████████████████████████████████
        /// <summary>███████████████████████ OnPress ███████████████████████</summary>
        /// ██████████████████████████████████████████████████████████████████████████
        public void OnPress()
        {
            dispatcher.Dispatch(HexagonEvent.Press);
        }

        #region Pool Methods
        public string PoolKey { get; set; }

        public void OnGetFromPool()
        {

        }

        public void OnReturnFromPool()
        {
            transform.localScale = Vector3.one;
        }
        #endregion
    }
}
