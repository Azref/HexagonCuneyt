# Core
This is a core project which provide MVCS structure for unity projects. Based on StrangeIoc framework.

- Benefits
  - Dependency Injection
  - Model-View-Controller 
  - Totally Scalable
  - More Readable
  - Event Based
  - Testable
  - Rapid Development
  - Modular Code

For more details and documentaion of StrangeIoc:

Website: https://strangeioc.github.io/strangeioc/

## Architecture 

![alt text](https://strangeioc.github.io/strangeioc/class-flow.png)

## Additionally
Some feature added to simplify usage. These are not coming with the basic StrangeIoc framework.

- Main
  - Main scene contains a root prefab with AppRoot file attached. AppRoot file triggers the app context and start the application. If there is only one context, It means there will be only one scene running and only the content of this scene will be changed. It is also possible to add more context. The main context of core project is basically implemented for UI structures. But for example, If you need to play a game in a 3D environment. Adding a different context and a scene for level structure is a better approach.    
  ![alt text](https://i.ibb.co/sRpGgkD/main.png)
  

- Screen Manager
  - Screen manager is implemented to administrate screens. It listens for "openpanel" event and change the screens according to needs. Every screen on the application is a prefab. So you don't need to change the scene completely, you just need to change or add prefabs. There are 3 layers at the bottom of ScreenManager. All layers can be used seperately (Also possible to add more layers)    

```C#
IPanelVo panel = new ConfirmPanelVo("Ok","Cancel",1);
dispacther.Dispatch(ScreenEvent.OpenPanel,panel);
```

- Events
  - StrangeIoc uses the signal structure to communicate between classes. More suitable and useful event structure implemented for simple usage. 

- Test screens
  - These are created for testing every screen individually. So they have seperate contexts, scenes, classes. So easily mock datas can be set without running whole application. And every screen can be testable separately. Everything under "Tests" folder are flagged as debug. So these scripts and files are not be contained on the release build. 

- Runtime variables
  - These variables are also used to communicate inside whole application. And they can be easily monitorize with Odin asset. So, you can control the application with these variables. Check AppStatus.cs for usage. 
  
- AppStatus
  - This is a state machine. Used flag enum structure to implement. Also this is a runtime variable and you can easily see and edit the states with editor.  
  ![alt text](https://i.imgyukle.com/2019/09/25/ot9siM.png)
  
## Tools

- Create Screen (Tools/Core/Create Screen)
  - This is an editor tool to create screens. You just need to write the name of the screen and add some events If needed. Then everything will be setted up. This tool creates;   
    - Tests/Screen/&NAME&/Scripts/Controller/&NAME&ScreenTestCommand.cs 
    - Tests/Screen/&NAME&/Scripts/&NAME&ScreenTestContext.cs
    - Tests/Screen/&NAME&/Scripts/&NAME&ScreenTestRoot.cs
    - Tests/Screen/&NAME&/Scripts/&NAME&ScreenTest.unity
    - Scripts/Project/View/&NAME&/&NAME&ScreenMediator.cs
    - Scripts/Project/View/&NAME&/&NAME&ScreenView.cs
    - Resources/Screens/&NAME&.prefab

And connects them with each other. So you can directly start editing the new screen. 

After the new screen is done. You also need to add the related scripts to app context

```mediationBinder.Bind<HomeScreenView>().To<HomeScreenMediator>(); ```

- Create Reporter (Tools/Core/Create Reporter)
  - Add a reporter prefab to current scene. Reporter is used to view logs on mobile devices with finger movement. This prefab should be removed manually before release build. 

## Assets

- Odin: https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041
- DOTween: http://dotween.demigiant.com/documentation.php
- I2 Localization: https://assetstore.unity.com/packages/tools/localization/i2-localization-14884
- Procedural UI Image: https://assetstore.unity.com/packages/tools/gui/procedural-ui-image-52200
- Strange Ioc: https://assetstore.unity.com/packages/tools/strangeioc-9267
- Log Viewer : https://assetstore.unity.com/packages/tools/integration/log-viewer-12047
