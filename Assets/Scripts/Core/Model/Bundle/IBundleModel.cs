using Assets.Scripts.Core.Promise;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Model.Bundle
{
    public interface IBundleModel
    {
        void AddBundleData(string key, BundleDataVo vo);

        Dictionary<string, BundleDataVo> GetBundleDatas();


        IPromise<BundleLoadData> LoadBundle(string name, string path, bool load = false);

        IPromise<BundleLoadData> LoadBundle(BundleLoadData loadData);

        void Clear(string name, bool clearAll = true);

        void ClearLayers(string[] names);

        BundleLoadData GetBundleByName(string name);

        GameObject GetPrefabByAssetName(string bundleKey, string assetKey);

        void SetBundleData(Dictionary<string, BundleDataVo> dir);
    }
}