using strange.extensions.mediation.impl;
using Assets.Scripts.Core.Manager.Screen;
using UnityEngine;
using Assets.Scripts.Core.View;
using Assets.Scripts.Project.Enums;

namespace Assets.Scripts.Project.Manager.Hexagon
{
    public class HexManager : PoolView, IPanelView
    {
	    public IPanelVo vo { get; set; }

        public GameObject HexPrefab;

        public RV_Grid Grid;

        public RV_Hexagon Hex;

        protected override void Start()
        {
            base.Start();

            //BuildGrid();
        }

        /// ██████████████████████████████████████████████████████████████
        /// <summary>████████████████ BuildGrid ████████████████</summary>
        /// ██████████████████████████████████████████████████████████████
        public void BuildGrid()
        {
            Debug.Log("GridManager building...");
            
            for (int gx = 0; gx < Grid.width; gx++)
            {
                for (int gy = 0; gy < Grid.height; gy++)
                {
                    var hex = pool.Get(PoolKey.Hexagon.ToString()).GetComponent<HexView>();

                    hex.transform.SetParent(transform);

                    string key = gx + "-" + gy;

                    Grid.HexDict.Add(key, hex);

                    Grid.HexList.Add(hex.transform);

                    hex.Setup(gx, gy);

                }
            }

			dispatcher.Dispatch(HexManagerEvent.GridReady);

        }
    }
}
