using System;
using Assets.Scripts.Core.Enums;
using Assets.Scripts.Project.Enums;
using Assets.Scripts.Project.Extension;
using Assets.Scripts.Project.View.Hexagon;
using DG.Tweening;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Scripts.Project.Manager.Selection
{
    public class SelectionManager : EventView
    {
        public RV_GameStatus Status;

        public RV_GameInfo Info;

        private LineRenderer _liner;

        private int _rotCount = 0;

        private bool _cw = true;

        protected override void Start()
        {
            base.Start();

            _liner = GetComponent<LineRenderer>();

            SwipeManager.OnSwipeDetected += OnSwipeDetected;
        }

        internal void Clear()
        {
            _liner.positionCount = 0;

            transform.position = new Vector3(-100, 0, 0);
        }

        /// //////////////////////////////////////////////////////////////
        /// //////////////////////// SWIPE INPUT /////////////////////////
        /// //////////////////////////////////////////////////////////////
        private void OnSwipeDetected(Swipe direction, Vector2 swipeVelocity, Vector2 firstPressPos, Vector2 secondPressPos)
        {
            if (!Status.value.HasFlag(GameStatus.SelectedHexes))
                return;

            if (Status.value.HasFlag(GameStatus.HexIsRotating))
                return;

            if (Status.value.HasFlag(GameStatus.Blocked))
                return;

            var center = Camera.main.WorldToScreenPoint(transform.position);

            _cw = Vector3.Cross((Vector3)secondPressPos - (Vector3)firstPressPos, center - (Vector3)firstPressPos).z < 0 ? true : false;

            _rotCount = 0;

            MakeRotation();
        }

        /// //////////////////////////////////////////////////////////////
        /// ////////////////////////// ROTATION //////////////////////////
        /// //////////////////////////////////////////////////////////////
        public void MakeRotation()
        {
            Status.value |= GameStatus.HexIsRotating;

            _rotCount++;

            transform.DORotate(new Vector3(0, 0, (_cw ? -1 : 1) * 120), .15f).OnComplete(RotCompleted).SetDelay(_rotCount == 1 ? 0 : .1f);
        }

        private void RotCompleted()
        {
            transform.localEulerAngles = Vector3.zero;

            Info.SelectedHexs.Rotate(Info.HexDict, _cw);

            Status.value &= ~GameStatus.HexIsRotating;

            if (_rotCount == 3)
                _rotCount = 0;
            else
                dispatcher.Dispatch(SelectionManagerEvent.RotComplete);
        }

        /// //////////////////////////////////////////////////////////////
        /// ///////////////////////// Selection //////////////////////////
        /// //////////////////////////////////////////////////////////////
        /// 
        /// <summary>
        /// We find clockwise neigbors according to CornerId.
        /// For example: 
        /// 
        /// this is the figure when we touch to the right corner of the hexagon (cornerId 1)
        /// (the touch point is shown in the figure with "X")
        /// Clockwise Neighbors: RTHex (HexNeighbor)0 - RBHex (HexNeighbor)1
        /// </summary>
        ///                            5 - - - 0
        ///                           -         -
        ///                         -    RTHex    -
        ///                       4-(HexNeighbor)1 -1
        ///            5 - - - 0    -             -
        ///           -         -     -         -
        ///         -             -    3 - - - 2
        ///       4-   touched   X -1
        ///         -  hexagon    -    5 - - - 0
        ///           -         -     -         -
        ///            3 - - - 2    -    RBHex    -
        ///                       4-(HexNeighbor)2 -1
        ///                         -             -
        ///                           -         -
        ///                            3 - - - 2
        ///  
        public void SelectHex(string val)
        {
            string[] split = val.Split('/');

            /// We have a string: touched Hex and closest corner to that touch point. So we need to parse it.
            HexView hex1 = Info.HexDict[split[0]];

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
            Info.SelectedHexs.Clear();

            Info.SelectedHexs.Add(hex1);
            Info.SelectedHexs.Add(hex2);
            Info.SelectedHexs.Add(hex3);

            Status.value |= GameStatus.SelectedHexes;

            DrawOtline();
        }

        private void FindClosestNeighbor(HexView hex1, int cornerId)
        {
            if (hex1.x == 0)
            {
                if (hex1.y == 0)
                    SelectHex(hex1, 0);

                else if (hex1.y == Info.GridHeight - 1 && cornerId == 4)
                    SelectHex(hex1, 2);

                else if (hex1.y == Info.GridHeight - 1 && cornerId == 0)
                    SelectHex(hex1, 1);

                else if (cornerId == 5)
                    SelectHex(hex1, 0);

                else
                    SelectHex(hex1, 2);
            }
            else if (hex1.x == Info.GridWidth - 1)
            {
                if (hex1.y == 0 && cornerId > 1)
                    SelectHex(hex1, cornerId + 1);

                else if (hex1.y == 0 && cornerId > 1)
                    SelectHex(hex1, cornerId + 1);

                else if (hex1.y == Info.GridHeight - 1 && cornerId == 0)
                    SelectHex(hex1, 5);

                else if (hex1.y == Info.GridHeight - 1 && cornerId < 3)
                    SelectHex(hex1, cornerId + 1);

                else if (hex1.y == Info.GridHeight - 1 && cornerId <= 5)
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
            else if (hex1.y == Info.GridHeight - 1)
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
        ///                         5 - - - 0
        ///                        -         -
        ///                      -             -
        ///                    4-               -1
        ///                      -             -
        ///                        -         -
        ///                         3 - - - 2
        ///    
        ///
        internal void DrawOtline()
        {
            var hexs = Info.SelectedHexs;
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
                ///  
                ///                            5 - - - 0
                ///                           -         -
                ///                         -             -
                ///                       4-     hex1      -1
                ///            5 - - - 0    -             -
                ///           -         -     -         -
                ///         -             -    3 - - - 2
                ///       4-     hex0      -1
                ///         -             -    5 - - - 0
                ///           -         -     -         -
                ///            3 - - - 2    -             -
                ///                       4-     hex3      -1
                ///                         -             -
                ///                           -         -
                ///                            3 - - - 2
                ///                                 
                _liner.SetPosition(0, hexs[0].transform.localPosition + new Vector3(+Info.Edge * .5f, -Info.Height * .5f, 0)); //Corer 2
                _liner.SetPosition(1, hexs[0].transform.localPosition + new Vector3(-Info.Edge * .5f, -Info.Height * .5f, 0)); //Corer 3
                _liner.SetPosition(2, hexs[0].transform.localPosition + new Vector3(-Info.Edge, 0, 0));                           //Corer 4
                _liner.SetPosition(3, hexs[0].transform.localPosition + new Vector3(-Info.Edge * .5f, +Info.Height * .5f, 0)); //Corer 5

                _liner.SetPosition(4, hexs[1].transform.localPosition + new Vector3(-Info.Edge, 0, 0));                           //Corer 4
                _liner.SetPosition(5, hexs[1].transform.localPosition + new Vector3(-Info.Edge * .5f, +Info.Height * .5f, 0)); //Corer 5
                _liner.SetPosition(6, hexs[1].transform.localPosition + new Vector3(+Info.Edge * .5f, +Info.Height * .5f, 0)); //Corer 0
                _liner.SetPosition(7, hexs[1].transform.localPosition + new Vector3(+Info.Edge, 0, 0));                           //Corer 1

                _liner.SetPosition(8, hexs[2].transform.localPosition + new Vector3(+Info.Edge * .5f, +Info.Height * .5f, 0)); //Corer 0
                _liner.SetPosition(9, hexs[2].transform.localPosition + new Vector3(+Info.Edge, 0, 0));                           //Corer 1
                _liner.SetPosition(10, hexs[2].transform.localPosition + new Vector3(+Info.Edge * .5f, -Info.Height * .5f, 0)); //Corer 2
                _liner.SetPosition(11, hexs[2].transform.localPosition + new Vector3(-Info.Edge * .5f, -Info.Height * .5f, 0)); //Corer 3
            }
            else
            {
                ///
                ///             5 - - - 0
                ///            -         -
                ///          -             -
                ///        4-     hex2      -1
                ///          -             -    5 - - - 0 
                ///            -         -     -         - 
                ///             3 - - - 2    -             - 
                ///                        4-     hex0      -1
                ///             5 - - - 0    -             - 
                ///            -         -     -         -  
                ///          -             -    3 - - - 2
                ///        4-     hex1      -1
                ///          -             -
                ///            -         -
                ///             3 - - - 2
                ///  
                _liner.SetPosition(0, hexs[0].transform.localPosition + new Vector3(-Info.Edge * .5f, +Info.Height * .5f, 0)); //Corer 5
                _liner.SetPosition(1, hexs[0].transform.localPosition + new Vector3(+Info.Edge * .5f, +Info.Height * .5f, 0)); //Corer 0
                _liner.SetPosition(2, hexs[0].transform.localPosition + new Vector3(+Info.Edge, 0, 0));                           //Corer 1
                _liner.SetPosition(3, hexs[0].transform.localPosition + new Vector3(+Info.Edge * .5f, -Info.Height * .5f, 0)); //Corer 2

                _liner.SetPosition(4, hexs[1].transform.localPosition + new Vector3(+Info.Edge, 0, 0));                           //Corer 1
                _liner.SetPosition(5, hexs[1].transform.localPosition + new Vector3(+Info.Edge * .5f, -Info.Height * .5f, 0)); //Corer 2
                _liner.SetPosition(6, hexs[1].transform.localPosition + new Vector3(-Info.Edge * .5f, -Info.Height * .5f, 0)); //Corer 3
                _liner.SetPosition(7, hexs[1].transform.localPosition + new Vector3(-Info.Edge, 0, 0));                           //Corer 4

                _liner.SetPosition(8, hexs[2].transform.localPosition + new Vector3(-Info.Edge * .5f, -Info.Height * .5f, 0)); //Corer 3
                _liner.SetPosition(9, hexs[2].transform.localPosition + new Vector3(-Info.Edge, 0, 0));                           //Corer 4
                _liner.SetPosition(10, hexs[2].transform.localPosition + new Vector3(-Info.Edge * .5f, +Info.Height * .5f, 0)); //Corer 5
                _liner.SetPosition(11, hexs[2].transform.localPosition + new Vector3(+Info.Edge * .5f, +Info.Height * .5f, 0)); //Corer 0

            }

        }
    }
}
