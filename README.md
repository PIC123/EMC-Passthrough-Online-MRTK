<img align="right" width="150" src="docs\images\emc-logo.png"/>

# Earth-Mission-Control

An immersive, multi-user VR/AR data visualization platform, aimed at enabling climate scientists to more effectively communicate their data stories to policy makers to drive more informed policy decisions.

##

## Set-Up/Installation

This project was built with Unity and the final application was compiled into an Android .apk file since that is the format that the Quest headsets use. To test out the application, simple download the apk file from the [releases page](https://github.com/PIC123/Earth-Mission-Control-XRTK/releases/tag/v0.1.0-alpha) and install it on your headset using either Sidequest or the command line. 

To test out the application in the Unity Editor, you will need to set up a few things:
- First, download Unity Hub and install Unity 2021.3.20f, making sure to install the Android Development tools. This process will also install Visual Studio to be able to to view and edit the code. You can follow [these instructions](https://learn.unity.com/tutorial/install-the-unity-hub-and-editor) for more details.
- Ensure your headset is properly set up for development, following [these instructions](https://developer.oculus.com/documentation/unity/unity-env-device-setup/).
- Next clone the repo or download it as a zip file and extract it. In Unity Hub, add the project and open it. 
- Once the project is open, load the 'EMC-VR-layout1' Scene from the Assets/Scenes folder. 
   - If you want to explore the desktop version, open the scene titled 'EMC-desktop-layout1-updated'
- Add the MapSessionConfig and ChatGPTSettings files you have into the Resources folder.
- Ensure that the build settings are set to Android and if the Oculus software is running locally and the headset is connected via Link, you can just press the play button in the Editor and the application will load on the headset. 

## Description of Files

The relevant files for the EMC environment are:
 - Assets/Scripts/EnvironmentManager.cs
   - The manager script for controlling environment parameters, used by the control panel component.
 - Assets/Scripts/GlobeManager.cs
   - The manager script for the globe module, responsible for placing the map pins in the correct locations and controlling the visuals on the globe. 
 - Assets/Scripts/MapManager.cs
   - The manager script for the map table and is responsible for controlling the zoom level, map location, water level visualization, particle simulation, and location info for the dashboard. 
 - Assets/Scripts/MapPinManager.cs
   - The manager script for the map pins that appear on the globe. The data is loaded from a JSON file and sets up the visuals of the pins.

The relevant files for EarthBot are:
 - Assets/Scripts/ChatGPTAssistant.cs
    - The main interface for the ChatGPT game object that connects with the text-to-speech module from the voice SDK and sends text to it in chunks because the Voice SDK has a limit to how much text it can process at once. 
    - Adapted from DilmerVR chatbot implementation.
 - Assets/Scripts/VoiceIntentController.cs
    - Connects the speech understanding to the system and allows actions to be triggered when key words or intents are recognized. 
    - It specifies which actions the system can take and connects them to event listeners
    - Adapted from DilmerVR chatbot implementation.
 - Assets/Scripts/ChatGPT/ChatGPTSettings.cs
    - A helper file used to store ChatGPT settings
    - Adapted from DilmerVR chatbot implementation.
 - Assets/Scripts/ChatGPT/ChatGPTClient.cs
    - The main interface with the ChatGPT REST API. Includes logic for message history storage and sending the ChatGPT response to the ChatGPTAssistant script.
    - Adapted from DilmerVR chatbot implementation.

## Troubleshooting

- If the scene loads without the environment fully loaded, open the package manager and re-import the asset called Nature Starter Kit 2. If it's not listed under the package manager, you can get it from the asset store as a free asset. 
- If the EarthBot doesn't respond to questions or if there is a console error that says there are ChatGPT settings missing even though they are in the Resources folder, select the ChatGPTClient object in the Hierarchy and set the ChatGPTSettings property to the file in the Resources folder. 