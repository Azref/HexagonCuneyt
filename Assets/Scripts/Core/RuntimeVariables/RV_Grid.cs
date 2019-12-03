using Assets.Scripts.Project.View.Hexagon;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Variables/Int Width-Height", order = 9)]
public class RV_Grid : SerializedScriptableObject
{
    public int width;

    public int height;

    public List<Transform> HexList = new List<Transform>();

    public Dictionary<string, HexView> HexDict = new Dictionary<string, HexView>();

}