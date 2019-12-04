#if UNITY_EDITOR || DEBUG
using strange.extensions.command.impl;
using Assets.Scripts.Project.Event;
using UnityEngine;

namespace Assets.Tests.GamePlay.Scripts.Controller
{
    public class GameManagerTestCommand : EventCommand
    {
        public override void Execute()
        {
            Debug.Log("GameManagerTestCommand");

            dispatcher.Dispatch(GameEvent.BuildGrid);
        }
    }
}
#endif