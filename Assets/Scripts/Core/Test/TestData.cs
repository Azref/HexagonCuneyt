#if UNITY_EDITOR || DEBUG
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class TestData : MonoBehaviour
{
    //[Title("Enter Data Path")]
    //public string DataPath = "/Tests/Data/";

    //[AssetList(Path = "$DataPath")]
    //public List<ScriptableObject> AssetList;

    /// <summary>
    /// Test datas
    /// </summary>
    [AssetList(Path = "/Tests/Data/")]
    public List<ScriptableObject> AssetList;

}
#endif

