using Assets.Scripts.Project.Enums;
using Assets.Scripts.Project.Manager.Hexagon;
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

    }

}