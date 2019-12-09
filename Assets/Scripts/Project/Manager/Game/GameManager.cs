using Assets.Scripts.Core.Manager.Screen;
using UnityEngine;
using Assets.Scripts.Core.View;
using Assets.Scripts.Project.Enums;
using Assets.Scripts.Project.View.Hexagon;
using Assets.Scripts.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Project.Extension;

namespace Assets.Scripts.Project.Manager.Game
{
    public class GameManager : PoolView, IPanelView
    {
	    public IPanelVo vo { get; set; }

        public GameObject HexPrefab;

        public RV_GameStatus Status;

        public RV_GameInfo Info;

        private Vector3 _downPos;

        /// //////////////////////////////////////////////////////////////
        /// <summary>//////////////// BuildGrid ////////////////</summary>
        /// //////////////////////////////////////////////////////////////
        public void BuildGrid()
        {
            //Debug.Log("GridManager building...");
            
            for (int gx = 0; gx < Info.GridWidth; gx++)
            {
                for (int gy = 0; gy < Info.GridHeight; gy++)
                {
                    var hex = pool.Get(PoolKey.Hexagon.ToString()).GetComponent<HexView>();

                    hex.transform.SetParent(transform);

                    string key = gx + "-" + gy;

                    Info.HexDict.Add(key, hex);

                    Info.HexList.Add(hex.transform);

                    hex.Setup(gx, gy);
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
            if (Status.value.HasFlag(GameStatus.Blocked) || Status.value.HasFlag(GameStatus.HexIsRotating) || !Status.value.HasFlag(GameStatus.GameIsPlaying))
                return false;

            if (Input.GetMouseButtonDown(0))
            {
                _downPos = Input.mousePosition;

                return false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                var dist = Vector3.Distance(_downPos, Input.mousePosition);

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
                var hex = hit.transform.GetComponentInParent<HexView>();

                //Debug.Log("----------------------");
                //Debug.Log(hit.transform.name);
                //Debug.Log("Target Distance  : " + aimVector);
                //Debug.Log("Aim              : " + Aim);
                //Debug.Log("CornerId         : " + CornerId);

                string param = hex.x.ToString() + "-" + hex.y.ToString() + "/" + cornerId.ToString();

                SetParentSelectedHexes();

                dispatcher.Dispatch(GameManagerEvent.MakeSelection, param);

            }
        }

        private void SetParentSelectedHexes()
        {
            if (Info.SelectedHexs.Count > 0)
            {
                if (Info.SelectedHexs[0] != null)
                    Info.SelectedHexs[0].transform.SetParent(transform);

                if (Info.SelectedHexs[1] != null)
                    Info.SelectedHexs[1].transform.SetParent(transform);

                if (Info.SelectedHexs[2] != null)
                    Info.SelectedHexs[2].transform.SetParent(transform);
            }

            Info.SelectedHexs.Clear();

            Status.value &= ~GameStatus.SelectedHexes;
        }

        /// //////////////////////////////////////////////////////////////
        /// ///////////////////// !!! MATCHING !!! ///////////////////////
        /// //////////////////////////////////////////////////////////////
        internal void CheckSelectionMatch()
        {
            Info.MatchList = new List<HexView>();

            for (int i = 0; i < Info.SelectedHexs.Count; i++)
                CheckHexMatch(Info.SelectedHexs[i]);

            if (Info.MatchList.Count == 0)
                dispatcher.Dispatch(GameManagerEvent.NoMatch);
            else
                MatchAnimation();
        }

        internal void CheckColumn()
        {
            for (int j = Info.GridHeight-1; j > 0; j--)
            {
                for (int i = Info.GridWidth-1; i > 0; i--)
                {

                    Debug.Log(i + "-" + j);
                    if (CheckHexMatch(Info.HexDict[i + "-" + j]))
                    {
                        break;
                    }
                }
            }
            if (Info.MatchList.Count > 0)
                MatchAnimation();
        }

        private bool CheckHexMatch(HexView hex1, bool justForCheck = false)
        {
            //Debug.Log("Checking for " + hex1.name);

            for (int i = 0; i < 6; i++)
            {
                HexView hex2 = hex1.Neighbors.ContainsKey((HexNeighbor)i) ? hex1.Neighbors[(HexNeighbor)i] : null;
                HexView hex3 = hex1.Neighbors.ContainsKey((HexNeighbor)((i + 1) % 6)) ? hex1.Neighbors[(HexNeighbor)((i + 1) % 6)] : null;

                if (hex2 != null && hex3 != null && hex1.color == hex2.color && hex1.color == hex3.color)
                {
                    if (justForCheck)
                        return true;

                    if (!Info.MatchList.Contains(hex1))
                        Info.MatchList.Add(hex1);

                    if (!Info.MatchList.Contains(hex2))
                    {
                        Info.MatchList.Add(hex2);
                        CheckHexMatch(hex2);
                    }

                    if (!Info.MatchList.Contains(hex3))
                    {
                        Info.MatchList.Add(hex3);
                        CheckHexMatch(hex3);
                    }

                }

            }
            return false;
        }

        /// //////////////////////////////////////////////////////////////
        /// ////////////////// !!! Match Animation !!! ///////////////////
        /// //////////////////////////////////////////////////////////////
        private void MatchAnimation()
        {
            Debug.Log("WE GOT METCHES !!!!!!");

            SetParentSelectedHexes();

            dispatcher.Dispatch(GameManagerEvent.WeGotMatch);

            Status.value |= GameStatus.MatchAnimation;

            Status.value |= GameStatus.Blocked;

            Info.BotHexes = Info.MatchList.FindBottomHexes();

            Info.BotHexes.Sort((hex1, hex2) => hex1.x.CompareTo(hex2.x));

            //Info.MatchList.Sort((hex1, hex2) => (hex1.y * 100 - hex1.x).CompareTo(hex2.y * 100 - hex2.x));

            for (int i = 0; i < Info.BotHexes.Count; i++)
            {
                Info.BotHexes[i].Match(i);
            }

        }

    }
}
