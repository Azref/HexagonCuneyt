using strange.extensions.mediation.impl;
using Assets.Scripts.Core.Manager.Screen;
using UnityEngine;
using Assets.Scripts.Core.View;
using Assets.Scripts.Project.Enums;
using Assets.Scripts.Project.View.Hexagon;
using System;

namespace Assets.Scripts.Project.Manager.Game
{
    public class GameManager : PoolView, IPanelView
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

        /// //////////////////////////////////////////////////////////////
        /// <summary>////////////////// INPUT //////////////////</summary>
        /// //////////////////////////////////////////////////////////////
        void Update()
        {
            //if (Input.touchCount > 0)
            //{
            //    Touch touch = Input.GetTouch(0);

            //    if(touch.phase == TouchPhase.)
            //        Debug.Log("-");

            //    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            //    {

            //    }

            //}
            if (Input.GetMouseButtonDown(0))
                FindClosestCorner();
        }

        /// //////////////////////////////////////////////////////////////
        /// ///////////////////// Find CornerId //////////////////////////
        /// //////////////////////////////////////////////////////////////
        /// 
        ///              CornerId representation figure 
        ///
        ///                         5 ----- 0
        ///                        -         -
        ///                      -             -
        ///                    4-               -1
        ///                      -             -
        ///                        -         -
        ///                         3 ----- 2
        ///         
        /// <summary>
        /// When we touch ona hexagon this method finds the Id of the closest corner;
        /// </summary>
        private void FindClosestCorner()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.transform.CompareTag("Hexagon"))
            {
                Vector2 aimVector = hit.point - (Vector2)hit.transform.position;
                float aim = Vector2.SignedAngle(aimVector, Vector2.down) + 180;
                int cornerId = Mathf.FloorToInt(aim / 60);
                var hex = hit.transform.GetComponent<HexView>();

                //Debug.Log("----------------------");
                //Debug.Log(hit.transform.name);
                //Debug.Log("Target Distance  : " + aimVector);
                //Debug.Log("Aim              : " + Aim);
                //Debug.Log("CornerId         : " + CornerId);

                string param = hex.x.ToString() + "-" + hex.y.ToString() + "/" +  cornerId.ToString();

                if (Hex.SelectedHexs[0] != null)
                    Hex.SelectedHexs[0].transform.SetParent(transform);

                if (Hex.SelectedHexs[1] != null)
                    Hex.SelectedHexs[1].transform.SetParent(transform);

                if (Hex.SelectedHexs[2] != null)
                    Hex.SelectedHexs[2].transform.SetParent(transform);

                dispatcher.Dispatch(GameManagerEvent.MakeSelection, param);
            }
        }


        /// //////////////////////////////////////////////////////////////
        /// <summary>//////////////// BuildGrid ////////////////</summary>
        /// //////////////////////////////////////////////////////////////
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

			dispatcher.Dispatch(GameManagerEvent.GridReady);

        }
    }
}
