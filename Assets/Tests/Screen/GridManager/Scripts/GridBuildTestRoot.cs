#if UNITY_EDITOR || DEBUG
using strange.extensions.context.impl;

namespace Assets.Tests.Screen.GridManager.Scripts
{
    public class GridBuildTestRoot : ContextView
    {
        private void Awake()
        {
            context = new GridBuildTestContext(this);
        }
    }
}
#endif