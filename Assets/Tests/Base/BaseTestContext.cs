#if UNITY_EDITOR || DEBUG
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.mediation.api;
using strange.extensions.mediation.impl;
using Assets.Scripts.Core.Manager.Screen;
using Assets.Scripts.Core.Model.Bundle;
using Assets.Scripts.Core.Manager.Pool;
using Assets.Scripts.Core.Model.Game;
using Assets.Scripts.Project.Event;

namespace Assets.Tests.Base
{
    /// <summary>
    /// All test classes derived from this class. Ex. Pool model should be needed in every test scene. So It can be 
    /// automaticly injected for every test scene
    /// </summary>
    public class BaseTestContext : MVCSContext
    {
        public BaseTestContext(MonoBehaviour view) : base(view)
        {
        }

        public BaseTestContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }

        protected override void mapBindings()
        {
            base.mapBindings();

            // crosss context
            CrossContextEvent<ContextEvent>();
            CrossContextEvent<ScreenEvent>();
            CrossContextEvent<GameEvent>();

            // base models
            injectionBinder.Bind<IScreenModel>().To<ScreenModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<IBundleModel>().To<BundleModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<IObjectPoolModel>().To<ObjectPoolModel>().ToSingleton();
            injectionBinder.Bind<IGameModel>().To<GameModel>().ToSingleton().CrossContext();

            // services
            //injectionBinder.Bind<ILocalizationService>().To<I2LocalizationService>().ToSingleton().CrossContext();

            // views
            mediationBinder.Bind<ScreenManager>().To<ScreenManagerMediator>();

        }

        /// <summary>
        /// Unbind signal structure and bind event structure
        /// </summary>
        protected override void addCoreComponents()
        {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>(); //Unbind to avoid a conflict!
            injectionBinder.Bind<ICommandBinder>().To<EventCommandBinder>().ToSingleton();

            injectionBinder.Unbind<IMediationBinder>();
            injectionBinder.Bind<IMediationBinder>().To<MediationBinder>().ToSingleton();
        }
    }
}
#endif