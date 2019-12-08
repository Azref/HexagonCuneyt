using Assets.Scripts.Project.View.Hexagon;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Variables/GameInfo", order = 10)]
public class RV_GameInfo : SerializedScriptableObject
{
    [Title("Grid Properties", "You can change grid size as you wish!", TitleAlignments.Centered)]
    public int GridWidth;

    public int GridHeight;

    [Title("Hexagon Properties", "these properties are static, We use them at calculations.", TitleAlignments.Centered)]

    public List<Color> HexColors = new List<Color>();

    [ShowInInspector] public const float HexagonHeight = 2;

    [ShowInInspector] public const float HexagonEdge = HexagonHeight * (1 / 1.732051f); // (1 / Mathf.Sqrt(3));

    [ShowInInspector] public const float DistHorizontal = HexagonEdge * 1.5f;

    public float HexHeight { get { return HexagonHeight; } }

    public float HexEdge { get { return HexagonEdge; } }

    public float GridDistX { get { return DistHorizontal; } }

    public List<Transform> HexList = new List<Transform>();

    public Dictionary<string, HexView> HexDict = new Dictionary<string, HexView>();

    public List<HexView> SelectedHexs = new List<HexView>();

    public List<HexView> MatchList = new List<HexView>();

}

