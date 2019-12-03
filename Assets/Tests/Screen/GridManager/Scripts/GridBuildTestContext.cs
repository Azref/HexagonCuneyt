#if UNITY_EDITOR || DEBUG
using Assets.Scripts.Project.Manager.Game;
using Assets.Scripts.Project.View.Cam;
using Assets.Scripts.Project.View.Hexagon;
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

            mediationBinder.Bind<GameManager>().To<GameManagerMediator>();
            mediationBinder.Bind<HexView>().To<HexMediator>();
            mediationBinder.Bind<CamManager>().To<CamManagerMediator>();

            commandBinder.Bind(ContextEvent.START).To<GridBuildTestCommand>().Once();
        }
    }
}
#endif