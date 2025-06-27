# Task: Unity SpaceX API Client

Your task is to prepare Unity project that will fetch data from API and display SpaceX's Tesla Roadster orbiting around the sun in 3D space and 
an interface that will list all SpaceX launches with additional details about the launch provided when interacting with any launch on the 
list.

SpaceX API: https://github.com/r-spacex/SpaceX-API/tree/master/docs#rspacex-api-docs

**Required Unity version: 6000.1.4f1**

## Objectives

* Project should be targeting either Android or iOS and should be adapted for mobile device usage, with touch input and gestures
* Create main menu that will let the user choose between Tesla Roadster simulation on the orbit of the sun or SpaceX launches browser
* It should be possible to go back to the main menu from either Roadster simulation or launches browser and choose either one again without restarting the game
* Feel free to use any libraries, tools, assets, and AI to implement this task

### Tesla Roadster simulation

* When the user chooses to see Tesla Roadster on the orbit of the sun he should see some 3D model that symbolizes the sun and a 3D model on it's orbit that symbolizes the car. Use orbital data from CSV file `Resources/Roadster.csv` and provided library `Resources/OrbitalElements.dll` to present sped up simulation of Tesla Roadster position on the orbit of the Sun. This simulation should run with speed of at least 24 hours / second and Roadster's position on the orbit can jump from one position to the next without any interpolation.
* It will suffice if the simulation will only use data between 07.02.2018 and 08.10.2019
* When the simulation ends, it should loop from the beginning of provided data set
* After updating Roadster's position show the last 20 positions as a line that connects to current Roadster's position (think tail of a comet).
* While the sped up simulation is playing and Roadster's position is being updated show all the current orbital data as text in the corner of the screen where current simulation date will be presented in local time zone, not UTC.

#### SpaceX launches browser

* When the user chooses to see SpaceX launches browser in the main menu, he should see fullscreen Unity UI that will present a scrollable list containing interactable elements. Each interactable element in the list should represent one SpaceX launch and provide text with name of the mission and number of payloads that were involved in the mission, as well as the name of the rocket used in the launch and it's country of origin.
* Each launch in the list should have an image that symbolizes if the launch has already happened or it is a future launch.
* Tapping on any launch element in the list should open a popup window with information about each ship used in the launch: number of missions it was used in, name, ship type, and home port

## Optional objectives

(listed in random order)

* SpaceX Launches browser should use object pooling to optimize resources consumption
* Use an async/await pattern
* Adhere to the SOLID principles

# Implementation: SpaceX API Client

## Overview
This Unity-based application visualizes SpaceX launches and the trajectory of the Tesla Roadster in space. The project uses real orbital data to simulate space objects and provides an interactive experience for exploring missions and space visualization.
#### Time spent: around 40 hours.
#### Around 50% of the code was generated with the help of AI (Claude, ChatGPT).

## Screens

Here are some screenshots that demonstrate the features and design of the application:

### Main Menu / Missions List / Space View
![Screenshots](/Resources/screens.jpeg)

## Project Structure
The project follows a modular architecture based on the Model-View-Presenter (MVP) pattern and is divided into several modules:

### Core Modules
- **MVP Framework**: Base implementation of the Model-View-Presenter pattern
- **Game**: Main game functionality including controllers, models, presenters, and views
- **Utils**: Utility modules for various functionalities

### Main Features

#### Space Visualization
- Interactive 3D visualization of space objects (Sun, Earth, Tesla Roadster)
- Camera controls for exploring the space environment
- Visual trails showing the trajectory of objects
- Real-time display of orbital data and current simulation date

#### Mission Explorer
- Browse SpaceX missions with their status (Failed, Completed, Upcoming)
- Detailed mission information including rocket name, payloads, and country of origin
- Mission details view for exploring specific mission parameters

#### Main Menu
- Navigation between Space and Missions modules

## Technologies

- **Unity**: Version 6000.1.4f1 with Universal Render Pipeline (URP)
- **TextMesh Pro**: For UI text rendering
- **TinyContainer**: Dependency injection framework
- **OrbitalElements**: Helper library for orbital elements transformation into Cartesian space positional vector
- **Newtonsoft.Json**: A high-performance JSON framework for .NET

## Data Sources

- JPL Horizon orbital elements for Tesla Roadster trajectory (included .scv file)
- SpaceX API for mission data

## Project Components

### Scenes
- **Initial**: Initial application scene, entry point
- **Main**: Main menu UI initialization
- **Space**: Visual simulation of the Roadster space mission

### Game managers
- **GameStarter**: Initialization of the app (installer, initializer)
- **MainMenuStarter**: Main menu with root navigation (Missions browser, Space simulation)
- **SpaceStarter**: Space simulation initialization

### Main views
- **LoadingUIView**: Hides transitions between scenes and loading processes
- **MainMenuUIView**: Central hub for navigation with options to access Space simulation and Missions explorer
- **MissionsUIView**: Displays the SpaceX missions list with status indicators (Failed, Completed, Upcoming)
- **MissionDetailsUIView**: Shows comprehensive information about selected missions including spacecraft details
- **SpaceUIView**: Manages simulation UI elements, displaying orbital data and current simulation date

### Presenters

#### Navigation Presenters
- **MainMenuPresenter**: Controls main navigation flow between Space and Missions modules
- **LoadingPresenter**: Handles loading screen state management and scene transitions

#### Space Visualization Presenters
- **SpaceTrailPresenter**: Manages visual trajectory trails for space objects in the simulation
- **SpaceUIPresenter**: Controls simulation UI elements, handles orbital data displays and date formatting

#### Mission Presenters
- **MissionListPresenter**: Manages the missions list, handling data loading, filtering, and item generation
- **MissionListItemPresenter**: Controls individual mission list items, managing their display state and interaction events
- **MissionDetailsPresenter**: Manages the detailed mission view, showing comprehensive mission information and related actions

### Models
- **SpaceModel**: Contains orbital data and calculations for space objects
- **MissionInfo**: Data structure for mission information

### Utils

The project includes various utility modules organized into dedicated namespaces that provide specialized functionality:

#### Camera & Visualization
- **CameraHelper**: Provides methods for camera positioning, smooth transitions, and targeting space objects
- **CameraDebugHelper**: Visual debugging tools for camera parameters and frustum visualization

#### Time & Simulation
- **DateSimulator**: Controls time flow and simulation speed in the space visualization
- **ICurrentDateStringProvider**: Interface for formatted date strings in the simulation UI

#### Coordinate Systems & Transforms
- **IOrbitalCoordinatesConverter**: Converts astronomical orbital elements into Unity 3D positions
- **IOrbitalDataStringProvider**: Interface for providing formatted orbital parameters to the UI

#### Mission Data
- **MissionInfo**: Core data structure for mission information (name, status, payload details)
- **MissionStatus**: Enumeration defining mission states (Failed, Completed, Upcoming)
- **OddityMissionDataLoader**: Implementation of IMissionDataLoader that fetches and parses SpaceX API data

#### Scene Management
- **SceneLoader**: Manages asynchronous scene loading with loading screen integration
- **ISceneLoader**: Interface for scene loading functionality used by controllers

#### Asset Management
- **AddressablePrefabLoader**: Loads prefabs using Unity's Addressables system
- **AddressableTextLoader**: Loads text assets using Unity's Addressables system
- **IPresenterLoader**: Interface for loading prefabs and presenter components dynamically

#### Object Pooling
- **SimpleGoPool**: Implementation of IObjectPool for GameObject recycling
- **IObjectPool**: Generic interface for object pooling to improve performance

#### UI Management
- **PresenterLoader**: Loads, initializes, and caches UI presenters
- **IUILoader**: Interface for loading UI views when needed

## Development

### Requirements
- Unity Editor 6000.1.4f1 or higher

## Future Enhancements

- Improved camera interpolation
- Configurable simulation parameters
- Additional space objects and missions
- Enhanced visual effects
- Splitting some classes into more entities to comply with SOLID principles
- Better errors handling
- Adding full documentation
- Adding unit tests
