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
        public RV_GameInfo Info;

        public Transform Mesh;

        public int color;

        public int x;

        public int y;

        public bool isStar = false;

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

            transform.name = "Hexagon_" + x + "-" + y;

            PlaceIt();

            FixNeighbors();

            ColorIt(true);

            BuildAnimation();
        }

        public void Replace(int hexX, int hexY)
        {
            x = hexX;

            y = hexY;

            transform.name = "Hexagon_" + x + "-" + y;

            PlaceIt();

            Info.HexDict[x + "-" + y] = this;
        }
        /// //////////////////////////////////////////////////////////////
        /// <summary>//////////////// PlaceIt //////////////////</summary>
        /// //////////////////////////////////////////////////////////////
        private void PlaceIt()
        {
            transform.position = new Vector3(x * Info.DistX, y * Info.Height);

            if (x % 2 == 1) transform.Translate(0, Info.Height * .5f, 0);

            _pos = transform.position;
        }

        /// //////////////////////////////////////////////////////////////
        /// <summary>////////////// FixNeighbors ///////////////</summary>
        /// //////////////////////////////////////////////////////////////
        public void FixNeighbors()
        {
            Neighbors.Clear();

            Info.HexDict.CheckAndAssign(this, HexNeighbor.TopHex, x + "-" + (y + 1));
            Info.HexDict.CheckAndAssign(this, HexNeighbor.BotHex, x + "-" + (y - 1));

            if (x % 2 == 0)
            {
                Info.HexDict.CheckAndAssign(this, HexNeighbor.RTHex, (x + 1) + "-" + y);
                Info.HexDict.CheckAndAssign(this, HexNeighbor.RBHex, (x + 1) + "-" + (y - 1));

                Info.HexDict.CheckAndAssign(this, HexNeighbor.LTHex, (x - 1) + "-" + y);
                Info.HexDict.CheckAndAssign(this, HexNeighbor.LBHex, (x - 1) + "-" + (y - 1));
            }
            else
            {
                Info.HexDict.CheckAndAssign(this, HexNeighbor.LTHex, (x - 1) + "-" + (y + 1));
                Info.HexDict.CheckAndAssign(this, HexNeighbor.LBHex, (x - 1) + "-" + y);

                Info.HexDict.CheckAndAssign(this, HexNeighbor.RTHex, (x + 1) + "-" + (y + 1));
                Info.HexDict.CheckAndAssign(this, HexNeighbor.RBHex, (x + 1) + "-" + y);
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
                color = UnityEngine.Random.Range(1, Info.HexColors.Count);

                if (CheckColorMatchForBuildSetup())
                {
                    ColorIt(buildSetup);
                    return;
                }
            }
            else
                color = specificColor;

            Mesh.GetComponent<SpriteRenderer>().color = Info.HexColors[color];
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
        public void BuildAnimation()
        {
            gameObject.SetActive(false);

            Invoke("BuildAnimationResume", .2f);
        }

        private void BuildAnimationResume()
        {
            transform.position = _pos + Vector3.up * (Info.GridHeight+5) * Info.Height;
            gameObject.SetActive(true);

            if(x == Info.GridWidth-1 && y == Info.GridHeight-1)
                transform.DOMove(_pos, .7f).SetEase(Ease.OutCirc).SetDelay(y * .1f + x * .2f).OnComplete(()=> {
                    dispatcher.Dispatch(HexEvent.BuildAnimationCompleted);
                });
            else
                transform.DOMove(_pos, .7f).SetEase(Ease.OutCirc).SetDelay(y * .1f + x * .2f);
        }

        public void MatchAnimation()
        {
            //particleFX
            pool.Return(gameObject);

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
