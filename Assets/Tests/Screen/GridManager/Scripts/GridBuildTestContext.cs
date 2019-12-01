#if UNITY_EDITOR || DEBUG
using Assets.Scripts.Project.Manager.Cam;
using Assets.Scripts.Project.Manager.Hexagon;
using Assets.Tests.Base;
using Assets.Tests.Screen.GridManager.Scripts.Controller;
using strange.extensions.context.api;
using UnityEngine;

namespace Assets.Tests.Screen.GridManager.Scripts
{
    public class GridBuildTestContext : BaseTestContext
    {
        public GridBuildTestContext(MonoBehaviour view) : base(view)
        {
        }

        public GridBuildTestContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }

        protected override void mapBindings()
        {
            base.mapBindings();

            mediationBinder.Bind<HexManager>().To<HexManagerMediator>();
            mediationBinder.Bind<HexView>().To<HexMediator>();
            mediationBinder.Bind<CamManager>().To<CamManagerMediator>();

            commandBinder.Bind(ContextEvent.START).To<GridBuildTestCommand>().Once();
        }
    }
}
#endif