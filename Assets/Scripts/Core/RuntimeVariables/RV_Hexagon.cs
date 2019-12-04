using Assets.Scripts.Project.Enums;
using Assets.Scripts.Project.View.Hexagon;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Variables/Hexagon", order = 10)]
public class RV_Hexagon : SerializedScriptableObject
{
    [ShowInInspector] public const float HexHeight = 2;

    [ShowInInspector] public const float HexEdge = HexHeight * (1 / 1.732051f); // (1 / Mathf.Sqrt(3));

    [ShowInInspector] public const float DistHorizontal = HexEdge * 1.5f;

    public float Height { get { return HexHeight; } }

    public float Edge { get { return HexEdge; } }

    public float DistX { get { return DistHorizontal; } }

    public Dictionary<HexagonColor, Color> Colors;

    public List<HexView> SelectedHexs = new List<HexView>();

}