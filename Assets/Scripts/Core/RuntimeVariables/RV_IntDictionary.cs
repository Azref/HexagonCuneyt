using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Variables/Int Dictionary", order = 9)]
public class RV_IntDictionary : SerializedScriptableObject
{
    public Dictionary<string, int> value;
}