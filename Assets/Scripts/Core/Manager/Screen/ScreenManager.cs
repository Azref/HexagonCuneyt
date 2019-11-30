//using Project.Enums;
using Sirenix.OdinInspector;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Scripts.Core.Manager.Screen
{
    public class ScreenManager : EventView
    {
        /// <summary>
        /// Loading screen as gameobject
        /// </summary>
        public GameObject LoadingLayer;

        /// <summary>
        /// Block screen as empty gameobject with raycast receiver
        /// </summary>
        public GameObject BlockLayer;

        /// <summary>
        /// All layers inside on screenmanager gameobject on hierarchy
        /// </summary>
        public Transform[] Layers;

        /// <summary>
        /// Type of the prefab that is going to load
        /// </summary>
        public PrefabLoadType LoadType;

        /// <summary>
        /// Path of bundle if PrefabLoadType is Bundle
        /// </summary>
        [ShowIf("LoadType", PrefabLoadType.Bundle)]
        public string BundlePath;

        /// <summary>
        /// Disable loading screen
        /// </summary>
        public void HideLoader()
        {
            if (LoadingLayer == null)
                return;

            LoadingLayer.SetActive(false);
        }

        /// <summary>
        /// Enable loading screen
        /// </summary>
        public void ShowLoader()
        {
            if (LoadingLayer == null)
                return;

            LoadingLayer.SetActive(true);
        }

        /// <summary>
        /// Block user input, disable UI
        /// </summary>
        public void Block()
        {
            if (BlockLayer == null)
                return;

            BlockLayer.SetActive(true);
        }

        /// <summary>
        /// Unblock user input, enable UI
        /// </summary>
        public void UnBlock()
        {
            if (BlockLayer == null)
                return;

            BlockLayer.SetActive(false);
        }

        public void OnClickLogButton()
        {
            //dispatcher.Dispatch(ScreenManagerEvent.OpenLogPanel,
            //    new PanelVo {Name = GameElement.ExceptionScreen, RemoveAll = false, LayerIndex = 1});
        }
    }
}