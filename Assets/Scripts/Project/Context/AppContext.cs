using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.mediation.api;
using strange.extensions.mediation.impl;
using Assets.Scripts.Core.Model.Bundle;
using Assets.Scripts.Core.Manager.Screen;
using Assets.Scripts.Core.Manager.Pool;
using Assets.Scripts.Project.View.Home;
using Assets.Scripts.Core.Model.App;
using Assets.Tests.Screen.Home.Scripts.Controller;

namespace Assets.Scripts.Project.Context
{
    /// <summary>
    /// Main application flow should be controlled over this class. 
    /// </summary>
    public class AppContext : MVCSContext
    {

        public AppContext(MonoBehaviour view) : base(view)
        {
        }

        public AppContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }

        protected override void mapBindings()
        {
            base.mapBindings();

            //context
            CrossContextEvent<ContextEvent>();
            CrossContextEvent<ScreenEvent>();

            //base models
            injectionBinder.Bind<IScreenModel>().To<ScreenModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<IBundleModel>().To<BundleModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAppModel>().To<AppModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<IObjectPoolModel>().To<ObjectPoolModel>().ToSingleton();

            //services
            //injectionBinder.Bind<ILocalizationService>().To<I2LocalizationService>().ToSingleton().CrossContext();

            //views
            mediationBinder.Bind<ScreenManager>().To<ScreenManagerMediator>();
            mediationBinder.Bind<HomeScreenView>().To<HomeScreenMediator>();

            // It is the start point of application. It works after all bindings are done
            commandBinder.Bind(ContextEvent.START).InSequence().To<HomeScreenCommand>();

            //Any event that fire across the Context boundary get mapped here.
            //crossContextBridge.Bind(MainEvent.GAME_COMPLETE);
            //crossContextBridge.Bind(MainEvent.REMOVE_SOCIAL_CONTEXT);
            //crossContextBridge.Bind(GameEvent.RESTART_GAME);

            //commandBinder.Bind(ContextEvent.START).To<StartCommand>().Once();
            //commandBinder.Bind(MainEvent.LOAD_SCENE).To<LoadSceneCommand>();
            //commandBinder.Bind(MainEvent.GAME_COMPLETE).To<GameCompleteCommand>();

            //commandBinder.Bind(ContextEvent.START).InSequence()
            ////.To<AddServiceProcessorsCommand>()
            ////.To<LoadBundleInfoCommand>()
            ////.To<LoadBundlesCommand>()
            ////.To<LoadDefaultsCommand>()
            //.To<StartCommand>();

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

