#if UNITY_EDITOR || DEBUG
using Assets.Scripts.Project.Manager.Game;
using Assets.Scripts.Project.Manager.Selection;
using Assets.Scripts.Project.View.Cam;
using Assets.Scripts.Project.View.Hexagon;
using Assets.Tests.Base;
using Assets.Tests.GamePlay.Scripts.Controller;
using strange.extensions.context.api;
using UnityEngine;

namespace Assets.Tests.GamePlay.Scripts
{
    public class GameManagerTestContext : BaseTestContext
    {
        public GameManagerTestContext(MonoBehaviour view) : base(view)
        {
        }

        public GameManagerTestContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }

        protected override void mapBindings()
        {
            base.mapBindings();

            mediationBinder.Bind<GameManager>().To<GameManagerMediator>();
            mediationBinder.Bind<HexView>().To<HexMediator>();
            mediationBinder.Bind<CamManager>().To<CamManagerMediator>();
            mediationBinder.Bind<SelectionManager>().To<SelectionManagerMediator>();

            commandBinder.Bind(ContextEvent.START).To<GameManagerTestCommand>().Once();
        }
    }
}
#endif