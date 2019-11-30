using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using Assets.Scripts.Core.View.Confirm;
using Assets.Scripts.Core.Model.Bundle;
using Assets.Scripts.Core.Model.App;
using Assets.Scripts.Core.Enums;

#if UNITY_EDITOR //|| DEBUG
using UnityEditor;

#endif

namespace Assets.Scripts.Core.Manager.Screen
{
    public enum ScreenManagerEvent
    {
        OpenLogPanel
    }

    public class ScreenManagerMediator : EventMediator
    {
        [Inject] public ScreenManager view { get; set; }

        [Inject] public IScreenModel screenModel { get; set; }

        [Inject] public IBundleModel bundleModel { get; set; }

        [Inject] public IAppModel App { get; set; }

        private List<GameObject> _panels;

        private AssetBundle _bundle = null;

        /// <summary>
        /// Works after all bindings are completed. 
        /// Useful to attach listeners
        /// After Awake 
        /// Before Start. 
        /// </summary>
        public override void OnRegister()
        {
            App.Status.value = 0;
            view.dispatcher.AddListener(ScreenManagerEvent.OpenLogPanel, OnOpenPanel);

            dispatcher.AddListener(ScreenEvent.OpenPanel, OnOpenPanel);
            dispatcher.AddListener(ScreenEvent.ClearLayer, OnClearLayer);
            dispatcher.AddListener(ScreenEvent.Back, OnBack);
            dispatcher.AddListener(ScreenEvent.ShowLoader, OnShowLoader);
            dispatcher.AddListener(ScreenEvent.HideLoader, OnHideLoader);
            dispatcher.AddListener(ScreenEvent.Block, OnBlock);
            dispatcher.AddListener(ScreenEvent.UnBlock, OnUnBlock);
            dispatcher.AddListener(ScreenEvent.ShowError, OnShowError);
            dispatcher.AddListener(ScreenEvent.Initialize, OnInitialize);

            _panels = new List<GameObject>();
            foreach (Transform layer in view.Layers)
            {
                foreach (Transform panel in layer)
                {
                    _panels.Add(panel.gameObject);
                }
            }
        }

        private void OnInitialize(IEvent payload)
        {
            OnUnBlock(null);

#if !UNITY_EDITOR
            view.LoadType = PrefabLoadType.Resources;
#endif
            //if (view.LoadType == PrefabLoadType.Bundle)
            //    bundleModel.LoadBundle(view.BundlePath).Done(OnBundleLoaded);
        }

        /// <summary>
        /// Automatically shows an alert panel, if panel could not be opened
        /// </summary>
        private void OnShowError(IEvent payload)
        {
            var message = (string)payload.data;

            if (string.IsNullOrEmpty(message) || message == "")
            {
                message = "Something went wrong :(";
            }

            ConfirmPanelVo panelVo = new ConfirmPanelVo
            {
                Name = "AlertPanel",
                Title = "LoginError",
                Description = message,
                ButtonLabel = "Ok",
                LayerIndex = 2,
                Type = AppStatus.Confirm
            };

            OpenPanelInner(panelVo);
        }

        private void OnBundleLoaded(BundleLoadData payload)
        {
            BundleLoadData bundleData = payload;
            if (bundleData != null && bundleData.Name == view.BundlePath)
            {
                _bundle = bundleData.Bundle;
            }
        }

        /// <summary>
        /// Remove the current page. Check the previous page and load it.
        /// </summary>
        private void OnBack(IEvent payload)
        {
            if (screenModel.History.Count < 2)
                return;

            screenModel.History.RemoveAt(screenModel.History.Count - 1);
            IPanelVo prePanelVo = screenModel.History[screenModel.History.Count - 1];
            screenModel.History.RemoveAt(screenModel.History.Count - 1);

            dispatcher.Dispatch(ScreenEvent.OpenPanel, prePanelVo);
        }

        /// <summary>
        /// Receives the open panel request
        /// </summary>
        private void OnOpenPanel(IEvent payload)
        {
            if (payload == null)
                return;

            OpenPanelInner((IPanelVo)payload.data);
        }

        /// <summary>
        /// Checks if the open panel request is valid
        /// </summary>
        private void OpenPanelInner(IPanelVo panelVo)
        {
            if (panelVo == null)
            {
                Debug.LogError("You have to send IPanelVo!!");
                return;
            }

            if (panelVo.LayerIndex >= view.Layers.Length)
            {
                Debug.LogError("There is no layer " + panelVo.LayerIndex);
                return;
            }

            CreateNewPanel(panelVo);
        }

        /// <summary>
        /// Remove the last screen added by name
        /// </summary>
        /// <param name="panelVo"></param>
        private void RemoveLastByName(IPanelVo panelVo)
        {
            for (int i = screenModel.History.Count - 1; i > 0; i--)
            {
                if (panelVo.Name == screenModel.History[i].Name)
                {
                    screenModel.History.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Identify how panel should be loaded according to type 
        /// </summary>
        private void CreateNewPanel(IPanelVo vo)
        {
            switch (view.LoadType)
            {
                case PrefabLoadType.Resources:
                    StartCoroutine(LoadFromResources(vo));
                    break;
                case PrefabLoadType.Bundle:
                    LoadFromBundle(vo);
                    break;
                case PrefabLoadType.AssetDatabase:
                    LoadFromAssetDatabase(vo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Loads panel which is stored outside of resources folder
        /// </summary>
        /// <param name="vo"></param>
        private void LoadFromAssetDatabase(IPanelVo vo)
        {
#if UNITY_EDITOR
            GameObject template =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Screens/" + vo.Name + ".prefab");
            if (template == null)
            {
                Debug.LogError("Panel not found!! " + vo.Name);
                return;
            }

            CreatePanel(vo, template);
#endif
        }

        /// <summary>
        /// Loads panel from resources folder
        /// </summary>
        private IEnumerator LoadFromResources(IPanelVo vo)
        {
            ResourceRequest request = Resources.LoadAsync("Screens/" + vo.Name, typeof(GameObject));
            yield return request;
            if (request.asset == null)
            {
                Debug.LogError("LoadFromResources! Panel not found!! " + vo.Name);
                yield break;
            }

            CreatePanel(vo, request.asset as GameObject);
        }

        /// <summary>
        /// Load panel from bundle
        /// </summary>
        private void LoadFromBundle(IPanelVo vo)
        {
            GameObject template = _bundle.LoadAsset<GameObject>(vo.Name);
            if (template == null)
            {
                Debug.LogError("Panel not found!! " + vo.Name);
                return;
            }

            CreatePanel(vo, template);
        }

        /// <summary>
        /// Create the panel and set the transform of gameobject
        /// </summary>
        /// <param name="vo"> PanelVo which is stored on View objects, if it is a screen </param>
        /// <param name="template"> Prefab to create </param>
        private void CreatePanel(IPanelVo vo, GameObject template)
        {
            if (vo.RemoveSamePanels)
                RemoveSamePanels(vo);

            if (vo.RemoveLayer)
                RemoveLayer(vo.LayerIndex);

            if (vo.RemoveAll)
                RemoveAllPanels();

            if (!screenModel.IgnoreHistory.Contains(vo.Name))
                screenModel.History.Add(vo);

            App.Status.value |= vo.Type;
            //Debug.Log("---------------------" + vo.Type);

            GameObject newPanel = Instantiate(template);
            IPanelView panelView = newPanel.GetComponent<IPanelView>();
            if (panelView != null)
                panelView.vo = vo;
            else
                Debug.LogWarning("No IPanelView on " + newPanel.name);

            newPanel.transform.SetParent(view.Layers[vo.LayerIndex], false);
            newPanel.transform.localScale = Vector3.one;

            _panels.Add(newPanel);
        }

        /// <summary>
        /// Used to prevent having same panels on a layer
        /// </summary>
        private void RemoveSamePanels(IPanelVo vo)
        {
            foreach (Transform child in view.Layers[vo.LayerIndex].transform)
            {
                App.Status.value &= ~child.GetComponent<IPanelView>().vo.Type;
                //if (App.Status.value.HasFlag(child.GetComponent<IPanelView>().vo.Type))
                //    App.Status.value -= child.GetComponent<IPanelView>().vo.Type;

                int index = child.name.IndexOf(vo.Name, StringComparison.Ordinal);
                if (index != -1)
                {
                    Destroy(child.gameObject);
                    RemoveLastByName(vo);
                }
            }
        }

        /// <summary>
        /// Clear all the gameobjecs on the given layer
        /// </summary>
        private void OnClearLayer(IEvent payload)
        {
            int layer = (int)payload.data;
            RemoveLayer(layer);
        }

        /// <summary>
        /// Clear all gameobjects on layer. Called when loading a new screen
        /// </summary>
        private void RemoveLayer(int voLayerIndex)
        {
            foreach (Transform panel in view.Layers[voLayerIndex].transform)
            {
                App.Status.value &= ~panel.GetComponent<IPanelView>().vo.Type;
                //if (App.Status.value.HasFlag(panel.GetComponent<IPanelView>().vo.Type))
                //    App.Status.value -= panel.GetComponent<IPanelView>().vo.Type;

                Destroy(panel.gameObject);
                _panels.Remove(panel.gameObject);
            }
        }

        /// <summary>
        /// Block user input, disable UI
        /// </summary>
        private void OnBlock(IEvent payload)
        {
            App.Status.value |= AppStatus.Blocked;
            view.Block();
        }

        /// <summary>
        /// Unblock user input, disable UI
        /// </summary>
        private void OnUnBlock(IEvent payload)
        {
            view.UnBlock();
            
            App.Status.value &= ~AppStatus.Blocked;
            //if (App.Status.value.HasFlag(Project.Enums.AppStatus.Blocked))
            //    App.Status.value -= Project.Enums.AppStatus.Blocked;
        }

        /// <summary>
        /// Enable loading screen
        /// </summary>
        private void OnShowLoader(IEvent payload)
        {
            if (payload.data != null)
            {
                int time = (int)payload.data;
                if (time > 0)
                {
                    StartCoroutine(WaitForTimeOut(time));
                }
            }

            view.ShowLoader();
        }

        /// <summary>
        /// Automated disabling of loading screen in given time
        /// </summary>
        private IEnumerator WaitForTimeOut(int time)
        {
            yield return new WaitForSeconds(time);
            view.HideLoader();
        }

        /// <summary>
        /// Disable loading screen
        /// </summary>
        private void OnHideLoader(IEvent payload)
        {
            view.HideLoader();
        }

        /// <summary>
        /// Clear all panels on all layers
        /// </summary>
        private void RemoveAllPanels()
        {
            foreach (var panel in _panels)
            {
                App.Status.value &= ~panel.GetComponent<IPanelView>().vo.Type;
                //if (App.Status.value.HasFlag(panel.GetComponent<IPanelView>().vo.Type))
                //    App.Status.value -= panel.GetComponent<IPanelView>().vo.Type;

                Destroy(panel);
            }

            _panels.Clear();
        }

        [UsedImplicitly]
        private void OnApplicationQuit()
        {
            if (dispatcher != null)
                dispatcher.Dispatch(ScreenEvent.AppQuit);
        }

        [UsedImplicitly]
        private void OnApplicationFocus(bool hasFocus)
        {
            if (dispatcher != null)
                dispatcher.Dispatch(ScreenEvent.FocusChanged, hasFocus);
        }

        [UsedImplicitly]
        private void OnApplicationPause(bool pauseStatus)
        {
            if (dispatcher != null)
                dispatcher.Dispatch(ScreenEvent.PauseChanged, pauseStatus);
        }

        /// <summary>
        /// Works when connected gameobject is destroyed. 
        /// Useful to remove listeners
        /// Before OnDestroy method
        /// </summary>
        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(ScreenManagerEvent.OpenLogPanel, OnOpenPanel);
            dispatcher.RemoveListener(ScreenEvent.ShowError, OnShowError);
            dispatcher.RemoveListener(ScreenEvent.Back, OnBack);
            dispatcher.RemoveListener(ScreenEvent.OpenPanel, OnOpenPanel);
            dispatcher.RemoveListener(ScreenEvent.ShowLoader, OnShowLoader);
            dispatcher.RemoveListener(ScreenEvent.HideLoader, OnHideLoader);
            dispatcher.RemoveListener(ScreenEvent.Block, OnBlock);
            dispatcher.RemoveListener(ScreenEvent.UnBlock, OnUnBlock);
            dispatcher.RemoveListener(ScreenEvent.ClearLayer, OnClearLayer);
            dispatcher.RemoveListener(ScreenEvent.Initialize, OnInitialize);
        }
    }
}