using Assets.Scripts.Project.Enums;
using Assets.Scripts.Project.View.Hexagon;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Project.Extension
{
    public static class HexagonExtension
    {

        public static HexView CheckAndAssign(this Dictionary<string, HexView> Dictionary, string val)
        {
            if (Dictionary.ContainsKey(val))
                return Dictionary[val];
            else
                return null;
        }

        public static void CheckAndAssign(this Dictionary<string, HexView> Dictionary, HexView hex, HexNeighbor NeighborId, string val)
        {
            if (!Dictionary.ContainsKey(val))
                return;

            hex.Neighbors[NeighborId] = Dictionary[val];

            Dictionary[val].Neighbors[(HexNeighbor)(((int)NeighborId + 3)% Enum.GetValues(typeof(HexNeighbor)).Length)] = hex;
        }

        public static void Copy(this HexView hex, HexView hexCopy)
        {
            hex.isStar = hexCopy.isStar;

            hex.isBomb = hexCopy.isBomb;

            hex.ColorIt(false, hexCopy.color);
        }

        public static void Rotate(this List<HexView> list, Dictionary<string, HexView> hexDict, bool cw)
        {
            int temp_0__x = list[0].x;
            int temp_0__y = list[0].y;

            if (!cw)
            {
                list[0].Replace(list[2].x, list[2].y);
                list[2].Replace(list[1].x, list[1].y);
                list[1].Replace(temp_0__x, temp_0__y);
            }
            else
            {
                list[0].Replace(list[1].x, list[1].y);
                list[1].Replace(list[2].x, list[2].y);
                list[2].Replace(temp_0__x, temp_0__y);
            }

            list[0].FixNeighbors();
            list[1].FixNeighbors();
            list[2].FixNeighbors();

        }

        public static List<HexView> FindBottomHexes(this List<HexView> list)
        {
            List<HexView> FxList = new List<HexView>();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Neighbors.ContainsKey(HexNeighbor.BotHex) &&
                    list.Contains(list[i].Neighbors[HexNeighbor.BotHex]))
                    continue;

                FxList.Add(list[i]);
            }
            return FxList;
        }
    }

}