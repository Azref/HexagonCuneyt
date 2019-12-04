using Assets.Scripts.Project.Enums;
using Assets.Scripts.Project.View.Hexagon;
using strange.extensions.mediation.impl;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Project.Manager.Selection
{
    public class SelectionManager : EventView
    {
        public RV_Hexagon HexInfo;

        public RV_Grid Grid;

        private LineRenderer _liner;

        protected override void Awake()
        {
            base.Awake();
            _liner = GetComponent<LineRenderer>();
        }

        /// //////////////////////////////////////////////////////////////
        /// ///////////////////////// Selection //////////////////////////
        /// //////////////////////////////////////////////////////////////
        /// 
        /// <summary>
        /// We find clockwise neigbors according to CornerId.
        /// For example: 
        /// 
        /// touchedhexagon cornerId 0, 
        /// Clockwise Neighbors: HexTop (HexNeighbor)0 - RTHex (HexNeighbor)1
        /// 
        /// </summary>
        public void SelectHex(string val)
        {
            string[] split = val.Split('/');

            /// We have touched Hex and closest corner to that touch point as a string. So we need to parse it.
            HexView hex1 = Grid.HexDict[split[0]];

            int cornerId = int.Parse(split[1]);

            SelectHex(hex1, cornerId);
        }

        private void SelectHex(HexView hex1, int cornerId)
        {
            /// We need 3 hex to select. if nearest neigbour is empty we will search for suitable one
            HexView hex2, hex3 = null;

            if (hex1.Neighbors.ContainsKey((HexNeighbor)(cornerId + 0)))
                hex2 = hex1.Neighbors[(HexNeighbor)(cornerId + 0)];
            else
            {
                FindClosestNeighbor(hex1, cornerId);
                return;
            }

            if (hex1.Neighbors.ContainsKey((HexNeighbor)((cornerId + 1) % 6)))
                hex3 = hex1.Neighbors[(HexNeighbor)((cornerId + 1) % 6)];
            else
            {
                FindClosestNeighbor(hex1, cornerId);
                return;
            }
            ////////////////////////////////////////////////////////////
            // fix hex array to draw outline easily
            if (cornerId == 0)
            {
                SelectHex(hex3, 4);
                return;
            }
            else if (cornerId == 2)
            {
                SelectHex(hex2, 4);
                return;
            }
            else if (cornerId == 3)
            {
                SelectHex(hex3, 1);
                return;
            }
            else if (cornerId == 5)
            {
                SelectHex(hex2, 1);
                return;
            }
            //////////////////////////////////////////////////////////////
            HexInfo.SelectedHexs.Clear();

            HexInfo.SelectedHexs.Add(hex1);
            HexInfo.SelectedHexs.Add(hex2);
            HexInfo.SelectedHexs.Add(hex3);

            Draw();
            //dispatcher.Dispatch(GameManagerEvent.DrawOutline);
        }

        private void FindClosestNeighbor(HexView hex1, int cornerId)
        {
            if (hex1.x == 0)
            {
                if (hex1.y == 0)
                    SelectHex(hex1, 0);

                else if (hex1.y == Grid.height - 1 && cornerId == 4)
                    SelectHex(hex1, 2);

                else if (hex1.y == Grid.height - 1 && cornerId == 0)
                    SelectHex(hex1, 1);

                else if (cornerId == 5)
                    SelectHex(hex1, 0);

                else
                    SelectHex(hex1, 2);
            }
            else if (hex1.x == Grid.width - 1)
            {
                if (hex1.y == 0 && cornerId > 1)
                    SelectHex(hex1, cornerId + 1);

                else if (hex1.y == 0 && cornerId > 1)
                    SelectHex(hex1, cornerId + 1);

                else if (hex1.y == Grid.height - 1 && cornerId == 0)
                    SelectHex(hex1, 5);

                else if (hex1.y == Grid.height - 1 && cornerId < 3)
                    SelectHex(hex1, cornerId + 1);

                else if (hex1.y == Grid.height - 1 && cornerId <= 5)
                    SelectHex(hex1, cornerId-1);

                else if (cornerId == 0)
                    SelectHex(hex1, 5);

                else
                    SelectHex(hex1, 3);
            }
            else if (hex1.y == 0)
            {
                if (cornerId < 3)
                    SelectHex(hex1, cornerId - 1);

                else 
                    SelectHex(hex1, cornerId + 1);
            }
            else if (hex1.y == Grid.height - 1)
            {
                if (cornerId < 2)
                    SelectHex(hex1, cornerId + 1);

                else
                    SelectHex(hex1, cornerId - 1);
            }
        }

        /// ///////////////////////////////////////////////<summary>
        /// ///////////////////////// Draw /////////////////////////
        /// </summary>//////////////////////////////////////////////
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
        ///

        internal void Draw()
        {
            var hexs = HexInfo.SelectedHexs;
            transform.position = new Vector3(
                (hexs[0].transform.position.x + hexs[1].transform.position.x + hexs[2].transform.position.x) / 3,
                (hexs[0].transform.position.y + hexs[1].transform.position.y + hexs[2].transform.position.y) / 3,
                0);
            hexs[0].transform.SetParent(transform);
            hexs[1].transform.SetParent(transform);
            hexs[2].transform.SetParent(transform);

            _liner.positionCount = 12;

            if (hexs[0].x < hexs[1].x)
            {
                _liner.SetPosition(0, hexs[0].transform.localPosition + new Vector3(+HexInfo.Edge * .5f, -HexInfo.Height * .5f, 0)); //Corer 2
                _liner.SetPosition(1, hexs[0].transform.localPosition + new Vector3(-HexInfo.Edge * .5f, -HexInfo.Height * .5f, 0)); //Corer 3
                _liner.SetPosition(2, hexs[0].transform.localPosition + new Vector3(-HexInfo.Edge, 0, 0));                           //Corer 4
                _liner.SetPosition(3, hexs[0].transform.localPosition + new Vector3(-HexInfo.Edge * .5f, +HexInfo.Height * .5f, 0)); //Corer 5

                _liner.SetPosition(4, hexs[1].transform.localPosition + new Vector3(-HexInfo.Edge, 0, 0));                           //Corer 4
                _liner.SetPosition(5, hexs[1].transform.localPosition + new Vector3(-HexInfo.Edge * .5f, +HexInfo.Height * .5f, 0)); //Corer 5
                _liner.SetPosition(6, hexs[1].transform.localPosition + new Vector3(+HexInfo.Edge * .5f, +HexInfo.Height * .5f, 0)); //Corer 0
                _liner.SetPosition(7, hexs[1].transform.localPosition + new Vector3(+HexInfo.Edge, 0, 0));                           //Corer 1

                _liner.SetPosition(8, hexs[2].transform.localPosition + new Vector3(+HexInfo.Edge * .5f, +HexInfo.Height * .5f, 0)); //Corer 0
                _liner.SetPosition(9, hexs[2].transform.localPosition + new Vector3(+HexInfo.Edge, 0, 0));                           //Corer 1
                _liner.SetPosition(10, hexs[2].transform.localPosition + new Vector3(+HexInfo.Edge * .5f, -HexInfo.Height * .5f, 0)); //Corer 2
                _liner.SetPosition(11, hexs[2].transform.localPosition + new Vector3(-HexInfo.Edge * .5f, -HexInfo.Height * .5f, 0)); //Corer 3
            }
            else
            {
                _liner.SetPosition(0, hexs[0].transform.localPosition + new Vector3(-HexInfo.Edge * .5f, +HexInfo.Height * .5f, 0)); //Corer 5
                _liner.SetPosition(1, hexs[0].transform.localPosition + new Vector3(+HexInfo.Edge * .5f, +HexInfo.Height * .5f, 0)); //Corer 0
                _liner.SetPosition(2, hexs[0].transform.localPosition + new Vector3(+HexInfo.Edge, 0, 0));                           //Corer 1
                _liner.SetPosition(3, hexs[0].transform.localPosition + new Vector3(+HexInfo.Edge * .5f, -HexInfo.Height * .5f, 0)); //Corer 2

                _liner.SetPosition(4, hexs[1].transform.localPosition + new Vector3(+HexInfo.Edge, 0, 0));                           //Corer 1
                _liner.SetPosition(5, hexs[1].transform.localPosition + new Vector3(+HexInfo.Edge * .5f, -HexInfo.Height * .5f, 0)); //Corer 2
                _liner.SetPosition(6, hexs[1].transform.localPosition + new Vector3(-HexInfo.Edge * .5f, -HexInfo.Height * .5f, 0)); //Corer 3
                _liner.SetPosition(7, hexs[1].transform.localPosition + new Vector3(-HexInfo.Edge, 0, 0));                           //Corer 4

                _liner.SetPosition(8, hexs[2].transform.localPosition + new Vector3(-HexInfo.Edge * .5f, -HexInfo.Height * .5f, 0)); //Corer 3
                _liner.SetPosition(9, hexs[2].transform.localPosition + new Vector3(-HexInfo.Edge, 0, 0));                           //Corer 4
                _liner.SetPosition(10, hexs[2].transform.localPosition + new Vector3(-HexInfo.Edge * .5f, +HexInfo.Height * .5f, 0)); //Corer 5
                _liner.SetPosition(11, hexs[2].transform.localPosition + new Vector3(+HexInfo.Edge * .5f, +HexInfo.Height * .5f, 0)); //Corer 0

            }

        }
    }
}
