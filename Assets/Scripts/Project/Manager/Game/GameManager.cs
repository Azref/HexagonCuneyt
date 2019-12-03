using strange.extensions.mediation.impl;
using Assets.Scripts.Core.Manager.Screen;
using UnityEngine;
using Assets.Scripts.Core.View;
using Assets.Scripts.Project.Enums;
using Assets.Scripts.Project.View.Hexagon;

namespace Assets.Scripts.Project.Manager.Game
{
    public class GameManager : PoolView, IPanelView
    {
	    public IPanelVo vo { get; set; }

        public GameObject HexPrefab;

        public RV_Grid Grid;

        public RV_Hexagon Hex;

        public LineRenderer Liner;


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
            if (Input.GetMouseButtonDown(0))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    if (hit.collider != null && hit.transform.CompareTag("Hexagon"))
                    {
                        Vector2 aimVector = hit.point - (Vector2)hit.transform.position;
                        float Aim = Vector2.SignedAngle(aimVector, Vector2.down) + 180;
                        int RotId = Mathf.FloorToInt(Aim / 60);
                        var RotHex = hit.transform.GetComponent<HexView>();

                        Debug.Log("----------------------");
                        Debug.Log(hit.transform.name);
                        //Debug.Log("Target Distance  : " + aimVector);
                        //Debug.Log("Aim              : " + Aim);
                        //Debug.Log("RotDot           : " + RotId);

                        RotateHex(RotHex, RotId);
                    }
                }
            }
        }

        /// //////////////////////////////////////////////////////////////
        /// <summary>///////////////// Rotate //////////////////</summary>
        /// //////////////////////////////////////////////////////////////
        private void RotateHex(HexView rotHex, int rotId)
        {
            HexView rotHex2, rotHex3 = null;
            if (rotHex.Neighbors.ContainsKey((HexNeighbor)(rotId + 0)))
                rotHex2 = rotHex.Neighbors[(HexNeighbor)(rotId + 0)];
            else
                return;

            if (rotHex.Neighbors.ContainsKey( (HexNeighbor)((rotId + 1) % 6)) )
                rotHex3 = rotHex.Neighbors[(HexNeighbor)((rotId + 1) % 6)];
            else
                return;

            //rotHex.transform.localScale = Vector3.one * .7f;
            //rotHex2.transform.localScale = Vector3.one * .7f;
            //rotHex3.transform.localScale = Vector3.one * .7f;

            Liner.positionCount = 3;
            Liner.SetPosition(0, rotHex.transform.position);
            Liner.SetPosition(1, rotHex2.transform.position);
            Liner.SetPosition(2, rotHex3.transform.position);
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

			dispatcher.Dispatch(HexManagerEvent.GridReady);

        }
    }
}
