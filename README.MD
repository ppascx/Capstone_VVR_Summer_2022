# Voice Recognition Virtual Reality 3.0


# Team Contributions:
**Kevin Louis-Jean** - Senior Project: VM Dev Environment Designer/Developer
**Michael Pena-Knight** - Capstone II: Mobile Developer/Tester, Scrum Master
**Zackary Schreiner** - Capstone II: System Designer/Developer, File Manager System Developer, and Capstone I Mentor
**Laura Figueroa** - Capstone II: System Developer, File Manager System Developer, Desktop Tester
**Bryan Pepe** - Capstone II: System Developer, Graph System Developer, Desktop Tester
**Erick Magyar** - Capstone II: System Developer, Graph System Developer
**Capstone I Students**: UI Elements, Assets, and Research


# Code Directory
```
├── ...
    ├── Windows Code              # Source Code for the Windows Version
    │   ├── AndroidPluginProject  # Android Build LeftOvers from the Original VVR Project
    │   ├── VVR         		  # Windows Version Unity Project Root
    │   |	├── Assets			  # Unity Assets Folder, source code files found here.
    | 	|	├── builds			  # Contains the Windows Executable
    | 	|	├── Packages		  # Unity packages used in this project.
    | 	|	└── ProjectSettings	  # Unity project settings.
    | 	└── 
    ├── Mobile Code
    |	├── AndroidBuildFiles     # Contains the Android build apk file.
    │   ├── IOSBuildFiles         # Contains the iOS Build Files
    |	├── Mobile VVR
    │   |	├── Assets			  # Unity Assets Folder, source code files found here.
    | 	|	├── Library			  # Unity Library folder
    | 	|	├── Packages		  # Unity packages used in this project.
    | 	|	└── ProjectSettings	  # Unity project settings.
    | 	└── 
    └──
```

# Windows Code Directory

## Assets
### Asset Directories
* Fantasy Skybox FREE - Free Unity Asset, includes the sky material used in the Menu Screen.
* Materials  - Contains the Sphere Material referenced by the Player Script.
* Models  - Contains the Inverted Normals Sphere Model.
* Plugins  --This folder has the Speech-and-Text Plugins that allow phrase parsing, unused in the Windows Build.
* PointOfInterestSystem - Assets that allow the Interactable icon UI to appear around the player. Used by UIInteractablesManager.cs. Developed by Capstone 1 Student - Michael Pena-Knight
* SpeechAndText --This folder is SpeechAndText API used to parse speech for mobile, Unused in Windows Version.
* TextMeshPro  - Unity Asset for UI Text.
* Sprites - Contains images used for the demo selection menu.
* UIKits - Unity Asset for the Main Menu, modified to display our demos and fit in with our Event System.
* TextMenus - Assets that allow the interactable objects at each node to display the modal UI. Used by UIInteractablesManager.cs. Developed by Capstone 1 Students - Zackary Schreiner and Laura Figueroa.
* WaypointSystemUI - Assets that allow the Waypoint icon UI to appear around the player. Used by UINodeManager.cs. Developed by Capstone 1 Student - Michael Pena-Knight.
* VideoPlayerControl - UI elements for displaying video progress bar, not implemented.
* Resources  - Contains 360 videos and 360 images used in our demos.
* Scenes
* * SampleScene - The Main Project Unity Scene.
* scripts - contains C# scripts, detailed in the following subsection.

### Scripts Directory

* **Graphs_Nodes** - Classes relating to Graph and Node data.
* * Graph.cs - Graph class and methods. Contains a list of GraphNodes. Used by GraphController.cs.
 * * GraphNode.cs - GraphNode class. Contains a list of its GraphNode neighbors. Each GraphNode has a NodeData variable and a Vector3 to determine its position in 3D space.
* * NodeData.cs - Base NodeData class. Contains a string src, meant to hold the path or url to the content we load at each node. Also contains a string that determines its nodetype.
* * NodeFactory.cs - Implementation of Abstract Factory pattern. Lists all subclasses of NodeData and places them in a Dictionary, returns an instance of  GraphNode when GraphController.cs is building a graph.
* * UINodeManager.cs - Handles the Waypoint System, shows waypoints at each neighboring node when the user wants to change their current location. 
* * UIInteractableManager.cs - Handles the Interactable System, shows a node's interactable objects if it has any. Functions similarly to UINodeManager.cs.
* * NodeData_Types - Directory that contains the child classes of NodeData.cs.
* * * Image360Node - For 360 images.
* * * Video360Node - For 360 video.

* **FileManager** - Classes relating to the File Browser.
* * FileBrowser.cs - FileBrowser class and BrowserSelection struct. Manages the browser UI.
* * FileManager.cs - FileManager class. Handles traversing through a file structure to expose directories and files at any given path. Used by FileBrowser.cs
* * SelectionManager.cs - SelectionManager class. Contains the ordered list of selected files. Implements logic to modify the order of the list by moving files up or down the order. Used by FileBrowser.cs
* * 

* **jsonStuff** - Classes Relating to the Reading of JSON files and objects. Using Unity's Json Serializer API.
* * EdgeJSONData.cs - Data Wrapper for reading JSON edges in a Graph. 
* * * NodeJSONData.cs - Data Wrapper for reading JSON nodes in a Graph.
* * GraphData.cs  - Data Wrapper for reading JSON data for Graph info.
* * JsonData.cs - Reads a JSON file given a filename, can read it again if the user decides to select a different file in the main menu. NOTE: Reads from a Unity Project's Application.Persistent data.
* * JSONReader.cs - Unused, first iteration of JsonData.cs.
* * CourseData.cs - Part of a future feature. Not Implemented.
* * CourseSaveData.cs - Part of a player progress saving feature. Not Implemented.

* **Player** - Scripts implementing the player gameObject's position, raycast, and voice control.
* * Gaze.cs - When active, shoots a raycast. Notifies the UIManagers if it selected something and passes an argument into those events.
* * GraphController.cs - Controls the single instance of the graph, will change it if the user changes the content in the main menu. Has a reference to the current GraphNode, which changes if the user decides to move. Interacts with the Content State Machine to set the state based on the type of content the incoming node will load (image or video).
* * Player.cs  --This script creates the sphere canvas where the video will play.
* * VoiceController.cs  - Sets up everything needed for Voice Recognition in Windows.
* * MouseLook.cs  - Allows for Mouse control.

 * **StateMachine** - Classes for the State Machines. Allow for different voice commands and UI elements to be available depending on state.
* * StateMachineMB.cs - Base State Machine Class, calls Exit() when we leave a state and Enter() when we enter a new state.
* * IState.cs - Interface for States.
* * ContentTypeSM.cs - State Machine in charge of switching states depending on the type of content the current node is playing. 
* * ImageContentState.cs - Default state. On Enter, will disable the video player and change the Sphere texture to the 360 image in the incoming node's source value.
* * VideoContentState.cs - On Enter, it will enable the Video Player and load the 360 video in the incoming node's source value.
* * MenuContentState.cs - On Enter, will disable sphere and show menu skybox. Will also tell the MainMenuController to display the menu in front of the user.
* * RaycastSM.cs - State Machine in charge of switching the user's raycast pointer on, also determines what kind of UI will be displayed.
* * EmptyRaycastState.cs - Default State. Disables the Player's Gaze.cs script.
* * MenuRaycastState.cs - On Enter, will allow interaction with the Main Menu and disable whatever UI is on the screen at that time.
* * MoveRaycastState.cs - On Enter, will notify UINodeManager to display its UI elements based on the currentnode.
* * SelectRaycastState.cs - On Enter, will notify UIInteractablesManager to display its Point Of Interest UI elements. 

 * **ScriptableObjects** - Holds the ScriptableObject assets for our Event System. These assets are defined by the classes in the EventChannels Directory.

 * **EventChannels** - Classes Created for our Scriptable Object Event System, all inherit from Scriptable Object. The bulk of our Event System, allowing for cross script communication.
  * * CommandHeardChannelSO.cs - For events that take in no arguments. Used when the user enters a different state through voice commands.
 * * HitNodeChannelSO.cs - Event that takes in a Vector3 argument. Used when the user selects a node neighbor to move to.
 * * HitInteractableChannelSO.cs - Event that takes in an int argument. Used when the user selects an interactable object.
  * * ChangeContentCommand.cs - Event that takes in a string argument. Used when the user decides to load a different demo graph from the main menu.

* **Events** - Simple delegate events that don't need to be Scriptable
* * FileBrowserSelectionEvent.cs - Delegate event OnBrowserSelectionComplete that is raised when the user clicks OK in the File Browser after selection.

 * **Interatable.cs** - Holds data, each Interactable object holds one, used to populate the Dialog box UI contents.
 * **InterpretCommand.cs** - Contains a Dictionary of all commands, strings as a key and C# Actions as values. When a voice command is recognized by WindowsVoiceControl.cs it will read the dictionary and Invoke the corresponding Action.
* **MainMenuController.cs** - Activates the Main Menu depending on state.
* **QuitApp.cs** - Quits the Application, linked to the Main Menu's second tab.
* **resolution.cs** - Sets game resolution.
* **VideoManager.cs** - Interacts with the Video Player Component on the player.
* **WaypointData.cs** - Holds Data about the Waypoint UI, each Waypoint prefab holds one.
* **WindowsVoiceControl.cs** - Implements Unity's Window Speech Recognition package, works with InterpretCommand.cs and VoiceController.cs.

 * **Courses** - Classes created for a Courses Feature and Progress Saving feature, not implemented.

# Mobile Code Directory

## .vscode  --VS Code file

## Assets
* Fantasy Skybox FREE - Free Unity Asset that includes the sky material used in the Menu Screen.

* Icons  --This folder has the VVR icon plus other button icons

* Plugins  --This folder has the Speech-and-Text Plugins that allow phrase parsing
* * iOS  --This folder has the Objective-C files that allow on-device voice recogintion
* * Android  --This folder has the Java files that allow on-device voice recognition

* Prefabs  --UI Materials 

* Resources  --Folder that has the 360 videos and 360 images

* Scenes
* * TestScenes --Folder that has some scenes to test features
* * Main.unity  --Main Unity Scene for VVR
* * VR-Scene.unity  --Scene that has an XR camera for VR support
* * Main.unity.meta --Unity Meta File

* Scripts
* **EventChannels** - Classes Created for our Scriptable Object Event System, all inherit from Scriptable Object. The bulk of our Event System, allowing for cross script communication.
* * CommandHeardChannelSO.cs - For events that take in no arguments. Used when the user enters a different state through voice commands.
* * HitNodeChannelSO.cs - Event that takes in a Vector3 argument. Used when the user selects a node neighbor to move to.
* * HitInteractableChannelSO.cs - Event that takes in an int argument. Used when the user selects an interactable object.
* * ChangeContentCommand.cs - Event that takes in a string argument. Used when the user decides to load a different demo graph from the main menu.

* **Graph_Nodes** - Classes relating to Graph and Node data.
* * Graph.cs - Graph class and methods. Contains a list of GraphNodes. Used by GraphController.cs.
* * GraphNode.cs - GraphNode class. Contains a list of its GraphNode neighbors. Each GraphNode has a NodeData variable and a Vector3 to determine its position in 3D space.
* * NodeData.cs - Base NodeData class. Contains a string src, meant to hold the path or url to the content we load at each node. Also contains a string that determines its nodetype.
* * NodeFactory.cs - Implementation of Abstract Factory pattern. Lists all subclasses of NodeData and places them in a Dictionary, returns an instance of  GraphNode when GraphController.cs is building a graph.
* * UINodeManager.cs - Handles the Waypoint System, shows waypoints at each neighboring node when the user wants to change their current location. 
* * UIInteractableManager.cs - Handles the Interactable System, shows a node's interactable objects if it has any. Functions similarly to UINodeManager.cs.
* * NodeData_Types - Directory that contains the child classes of NodeData.cs.
* * * Image360Node - For 360 images.
* * * Video360Node - For 360 video.

* **jsonStuff** - Classes Relating to the Reading of JSON files and objects. Using Unity's Json Serializer API.
* * EdgeJSONData.cs - Data Wrapper for reading JSON edges in a Graph. 
* * * NodeJSONData.cs - Data Wrapper for reading JSON nodes in a Graph.
* * GraphData.cs  - Data Wrapper for reading JSON data for Graph info.
* * JsonData.cs - Reads a JSON file given a filename, can read it again if the user decides to select a different file in the main menu. NOTE: Reads from a Unity Project's Application.Persistent data.
* * JSONReader.cs - Unused, first iteration of JsonData.cs.
* * CourseData.cs - Part of a future feature. Not Implemented.
* * CourseSaveData.cs - Part of a player progress saving feature. Not Implemented.

* * Player
* * * Gaze.cs - When active, shoots a raycast. Notifies the UIManagers if it selected something and passes an argument into those events.
* * * GraphController.cs - Controls the single instance of the graph, will change it if the user changes the content in the main menu. Has a reference to the current GraphNode,  which changes if the user decides to move. Interacts with the Content State Machine to set the state based on the type of content the incoming node will load (image or video).
* * * Player.cs  --This script creates the sphere canvas where the video will play
* * * PlayerMovement.cs  --This script defines the movement of the player in the game Note: Mobile Movement uses gyroscopic input to move in game
* * * SphereInverted.cs  --This script tells Unity to use the inside of the sphere to play the 360 videos instead of the outside

* **StateMachine** - Classes for the State Machines. Allow for different voice commands and UI elements to be available depending on state.
* * StateMachineMB.cs - Base State Machine Class, calls Exit() when we leave a state and Enter() when we enter a new state.
* * IState.cs - Interface for States.
* * ContentTypeSM.cs - State Machine in charge of switching states depending on the type of content the current node is playing. 
* * ImageContentState.cs - Default state. On Enter, will disable the video player and change the Sphere texture to the 360 image in the incoming node's source value.
* * VideoContentState.cs - On Enter, it will enable the Video Player and load the 360 video in the incoming node's source value.
* * MenuContentState.cs - On Enter, will disable sphere and show menu skybox. Will also tell the MainMenuController to display the menu in front of the user.
* * RaycastSM.cs - State Machine in charge of switching the user's raycast pointer on, also determines what kind of UI will be displayed.
* * EmptyRaycastState.cs - Default State. Disables the Player's Gaze.cs script.
* * MenuRaycastState.cs - On Enter, will allow interaction with the Main Menu and disable whatever UI is on the screen at that time.
* * MoveRaycastState.cs - On Enter, will notify UINodeManager to display its UI elements based on the currentnode.
* * SelectRaycastState.cs - On Enter, will notify UIInteractablesManager to display its Point Of Interest UI elements. 
* * Video Manager
* * * VideoManager.cs  --This script gives the video player functionality

* * Voice Control Scripts
* * * InterpretCommand.cs  --This script matches the voice command and calls the associated function
* * * MobileVoiceController.cs  --This script calls the SpeechAndText API functions and feeds InterpretCommand the text string

* * VR Control
* * * RuntimeXRLoaderManager.cs  --Loads the XR Coroutine to enable VR support for devices like Oculus that support VR

* * Windows Voice Control
* * * WindowsVoiceControl.cs  --This script enables desktop voice (desktop version does not use the SpeechAndText API)

* shaders  --This folder has the shaders to apply to video projection canvas

* SpeechAndText --This folder is SpeechAndText API used to parse speech for mobile

* TextMeshPro  --Unity UI Text renderer for UI elements

* WaypointSystemUI

* XR  --Unity XR Support for VR

# Research

### RestAPI Demo: a demo that was created to illustrate the potential use of a Rest API
### Finding an online speech recognition API that returns intent
### Playable online 360 video assets using AWS S3 bucket in Unity
### CubeTalk Demo: a demo that was created to test the functionality of the SpeechAndText API to return a string and not use elements that required colliders


