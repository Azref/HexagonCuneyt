#if UNITY_EDITOR || DEBUG
using Assets.Tests.Base;
using Assets.Scripts.Project.View.Home;
using Assets.Tests.Screen.Home.Scripts.Controller;
using strange.extensions.context.api;
using UnityEngine;

namespace Assets.Tests.Screen.Home.Scripts
{
    /// <summary>
    /// Test context for home screen.
    /// </summary>
    public class HomeScreenTestContext : BaseTestContext
    {
        public HomeScreenTestContext(MonoBehaviour view) : base(view)
        {
        }

        public HomeScreenTestContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }

        protected override void mapBindings()
        {
            // base bindings
            base.mapBindings();

            //views
            mediationBinder.Bind<HomeScreenView>().To<HomeScreenMediator>();

            //main flow start point
            commandBinder.Bind(ContextEvent.START).InSequence().To<HomeScreenTestCommand>();
        }
    }
}
#endif