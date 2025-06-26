# Task: Unity SpaceX API Client

Your task is to prepare Unity project that will fetch data from API and display SpaceX's Tesla Roadster orbiting around the sun in 3D space and 
an interface that will list all SpaceX launches with additional details about the launch provided when interacting with any launch on the 
list.

SpaceX API: https://github.com/r-spacex/SpaceX-API/tree/master/docs#rspacex-api-docs

**Required Unity version: 6000.1.4f1**

## Implementation is here >>> [README.MD](SpaceXClient/README.md)

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

## Implementation is here >>> [README.MD](SpaceXClient/README.md)
