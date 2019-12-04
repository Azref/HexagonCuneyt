#if UNITY_EDITOR || DEBUG
using Assets.Scripts.Core.Manager.Screen;
using strange.extensions.context.impl;
using UnityEngine;

namespace Assets.Tests.HomeScreen.Scripts
{
    /// <summary>
    /// Root class which is attached on root object on scene
    /// </summary>
    public class HomeScreenTestRoot : ContextView
    {
        private void Awake()
        {
            Debug.Log("HomeScreenTestRoot");

            var sm = GetComponentInChildren<ScreenManager>();
            if (sm != null)
            {
                // Clear gameobjects in all layers inside ScreenManager 
                for (var i = 0; i < sm.Layers.Length; i++)
                {
                    while (sm.Layers[i].childCount > 0)
                    {
                        DestroyImmediate(sm.Layers[i].GetChild(0).gameObject);
                    }
                }
            }

            //Instantiate the context, passing it this instance.
            context = new HomeScreenTestContext(this);
        }
    }
}
#endif