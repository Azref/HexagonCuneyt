#if UNITY_EDITOR || DEBUG
using strange.extensions.command.impl;
using Assets.Scripts.Project.Event;
using UnityEngine;

namespace Assets.Tests.Screen.GridManager.Scripts.Controller
{
    public class GridBuildTestCommand : EventCommand
    {
        public override void Execute()
        {
            Debug.Log("GridManagerTestCommand");

            dispatcher.Dispatch(GameEvent.BuildGrid);
        }
    }
}
#endif