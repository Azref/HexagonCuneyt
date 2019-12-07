using Assets.Scripts.Core.Manager.Screen;
using UnityEngine;
using Assets.Scripts.Core.View;
using Assets.Scripts.Project.Enums;
using Assets.Scripts.Project.View.Hexagon;
using Assets.Scripts.Core.Enums;
using System;

namespace Assets.Scripts.Project.Manager.Game
{
    public class GameManager : PoolView, IPanelView
    {
	    public IPanelVo vo { get; set; }

        public GameObject HexPrefab;

        public RV_GameStatus Status;

        public RV_Grid Grid;

        public RV_Hexagon Hex;

        private Vector3 _donwPos;

        protected override void Start()
        {
            base.Start();

            //BuildGrid();
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

                    hex.BuildAnimation();
                }
            }

			dispatcher.Dispatch(GameManagerEvent.GridReady);

        }

        /// //////////////////////////////////////////////////////////////
        /// <summary>////////////////// INPUT //////////////////</summary>
        /// //////////////////////////////////////////////////////////////
        void Update()
        {
            if(CheckTouch())
                FindClosestCorner();
        }

        private bool CheckTouch()
        {
            if (Status.value.HasFlag(GameStatus.Blocked | GameStatus.HexIsRotating) || !Status.value.HasFlag(GameStatus.GameIsPlaying))
                return false;

            if (Input.GetMouseButtonDown(0))
            {
                _donwPos = Input.mousePosition;

                return false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                var dist = Vector3.Distance(_donwPos, Input.mousePosition);

                return dist < 10;
            }
            else
                return false;
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

                if (Hex.SelectedHexs.Count > 0)
                {
                    if (Hex.SelectedHexs[0] != null)
                    Hex.SelectedHexs[0].transform.SetParent(transform);

                    if (Hex.SelectedHexs[1] != null)
                        Hex.SelectedHexs[1].transform.SetParent(transform);

                    if (Hex.SelectedHexs[2] != null)
                        Hex.SelectedHexs[2].transform.SetParent(transform);
                }

                dispatcher.Dispatch(GameManagerEvent.MakeSelection, param);
            }
        }

        /// //////////////////////////////////////////////////////////////
        /// ////////////////// Check Selection Match /////////////////////
        /// //////////////////////////////////////////////////////////////

        internal void CheckSelectionMatch()
        {
            
        }
    }
}
