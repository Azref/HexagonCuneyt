#if UNITY_EDITOR || DEBUG
using strange.extensions.context.impl;

namespace Assets.Tests.GamePlay.Scripts
{
    public class GameManagerTestRoot : ContextView
    {
        private void Awake()
        {
            context = new GameManagerTestContext(this);
        }
    }
}
#endif