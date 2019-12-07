using Assets.Scripts.Core.Manager.Pool;
using Assets.Scripts.Core.View;
using Assets.Scripts.Project.Enums;
using Assets.Scripts.Project.Extension;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Project.View.Hexagon
{
    public class HexView : PoolView, IPoolable
    {
        public RV_Grid GridInfo;

        public RV_Hexagon HexInfo;

        public int color;

        public int x;

        public int y;

        public bool isBomb = false;

        private Vector3 _pos;

        [ShowInInspector]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        public Dictionary<HexNeighbor, HexView> Neighbors = new Dictionary<HexNeighbor, HexView>();

        /// //////////////////////////////////////////////////////////////
        /// <summary>////////////////// Setup //////////////////</summary>
        /// //////////////////////////////////////////////////////////////
        public void Setup(int hexX, int hexY, int hexC = 0)
        {
            x = hexX;

            y = hexY;

            color = hexC;

            transform.name = "Hexagon_" + x + "_" + y;

            PlaceIt();

            FixNeighbors();

            ColorIt(true);

            BuildAnimation();
        }

        /// //////////////////////////////////////////////////////////////
        /// <summary>//////////////// PlaceIt //////////////////</summary>
        /// //////////////////////////////////////////////////////////////
        private void PlaceIt()
        {
            transform.localPosition = new Vector3(x * HexInfo.DistX, y * HexInfo.Height);

            if (x % 2 == 1) transform.Translate(0, HexInfo.Height * .5f, 0);

            _pos = transform.position;
        }

        /// //////////////////////////////////////////////////////////////
        /// <summary>////////////// FixNeighbors ///////////////</summary>
        /// //////////////////////////////////////////////////////////////
        public void FixNeighbors()
        {
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

        /// //////////////////////////////////////////////////////////////
        /// <summary>//////////////// ColorIt //////////////////</summary>
        /// //////////////////////////////////////////////////////////////
        public void ColorIt(bool buildSetup, int specificColor = 0)
        {
            if (buildSetup)
            {
                //color = (HexagonColor)UnityEngine.Random.Range(1, Enum.GetValues(typeof(HexagonColor)).Length);
                color = UnityEngine.Random.Range(1, HexInfo.Colors.Count);

                if (CheckColorMatchForBuildSetup())
                {
                    ColorIt(buildSetup);
                    return;
                }
            }
            else
                color = specificColor;

            GetComponent<SpriteRenderer>().color = HexInfo.Colors[color];
        }

        private bool CheckColorMatchForBuildSetup()
        {
            if (x == 0)
                return false;

            if (Neighbors.ContainsKey(HexNeighbor.LBHex) &&
                Neighbors[HexNeighbor.LBHex].color == color &&
                Neighbors.ContainsKey(HexNeighbor.LTHex) &&
                Neighbors[HexNeighbor.LTHex].color == color)
            {
                color = 0;
                return true;
            }

            if (
                Neighbors.ContainsKey(HexNeighbor.LBHex) &&
                Neighbors[HexNeighbor.LBHex].color == color &&
                Neighbors.ContainsKey(HexNeighbor.BotHex) &&
                Neighbors[HexNeighbor.BotHex].color == color )
            {
                color = 0;
                return true;
            }
            return false;
        }

        /// //////////////////////////////////////////////////////////////
        /// <summary>//////////////// Animate //////////////////</summary>
        /// //////////////////////////////////////////////////////////////
        internal void BuildAnimation()
        {
            gameObject.SetActive(false);

            Invoke("Animate",.2f);
        }

        internal void Animate()
        {
            transform.position = _pos + Vector3.up * (GridInfo.height+5) * HexInfo.Height;
            gameObject.SetActive(true);
            transform.DOMove(_pos, .7f).SetEase(Ease.OutCirc).SetDelay(y * .1f + x * .2f);
        }
        /// //////////////////////////////////////////////////////////////
        /// <summary>////////////////// Pool ///////////////////</summary>
        /// //////////////////////////////////////////////////////////////
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
