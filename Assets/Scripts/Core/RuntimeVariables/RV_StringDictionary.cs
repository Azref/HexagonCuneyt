using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Variables/String Dictionary", order = 9)]
public class RV_StringDictionary : SerializedScriptableObject
{
    public Dictionary<string, string> value;
}