# BeeHexed
The all new roguelike deckbuilder tower defense game, focusing on magical bees!
---
## Administrativa
[Living Document](https://docs.google.com/document/d/1YeMs-TCpdy3aiPiGqQ2wvI9agQTX7e68viUun7czUgE/edit?usp=sharing) (Must sign in with UW-affiliated email)

Uses Unity 2022.3.49f1 (LTS)
---
## User Manual
Beehexed is a game running on the Unity Engine, it is a roguelike deckbuilder hex tower defense game. This game is intended for people that love roguelike deckbuilders as a refreshing hybrid take on deckbuilders and tower defense.
### Installation

To install the game, do the following:

1. Download and Install [Unity Hub](https://unity.com/download)
2. Sign in or Create an Account with Unity
3. Install Unity Editor version 2022.3.49f1 (LTS) through Unity Hub, if version is not available download through [Unity Version Archive](https://unity.com/releases/editor/archive)
4. Download the [Zip file](https://github.com/tmorris37/BeeHexed/archive/refs/heads/main.zip) of this Repo
5. Unzip the file and place into a directory
6. In Unity Hub, click Add -> Add project from disk, then select the unzipped Beehexed folder
7. In Unity Hub, ensure the Editor is the correct version and double-click to open the project (this will take a while)

### Running software

Do the following:
1. Open the Unity Project you created in the Installation step
2. Go to File -> Build Settings
3. Hit Build and Run
4. Select a folder to build the exe file
5. Say yes to any continue prompts
6. Play

If the build fails (potentially because of recent commits), do the following:
1. Open the Unity Project you created in the Installation step
2. In the project window at the bottom of your screen open the 'Scenes' Folder
3. Double click on the Scene called MainMenu to open it
4. Click the play button at the top of the screen to start/stop the game (you may want to change Play Focused to Play Maximized)


### User Guide

#### Game Flow
1. **Main Menu**:  
   - On startup, users are presented with the main menu.  
   - From here, they can choose to start a new game, adjust settings, or exit.  

2. **Overworld Map**:  
   - After selecting "New Game," users are taken to the overworld level selection map.  
   - The map has clickable level icons on the far left and far right.  
   - Clicking an icon starts the corresponding level.

3. **In-Game Level**:  
   - A level consists of multiple rounds where enemies spawn and attack the player's center.  
   - Players gain nectar and cards at the start of each round.  
   - **Objective**: Defend the center using cards to win the game.  

4. **Game Victory or Loss**:  
   - **Win Condition**: Defeat all enemies.  
   - **Loss Condition**:  
      - Health reaches 0.  
      - Enemies are still present 10 seconds after the final wave.  

5. **Post-Level**:  
   - After completing a level, the next level is selected based on the overworld map path.

### Use Cases

#### 1. **Main Menu**
- **Buttons Available**:  
   - **Play**: Begins a new run and takes the player to the overworld level selection map.  
   - **Settings**: Opens the settings page with volume sliders.  
     - Clicking the **Back** button returns the user to the main menu.  
   - **Exit**: Allows the player to exit the game.  
     - In the Unity Editor, the user can stop the game by clicking the play button at the top.

#### 2. **Level Gameplay**
- **Initial Setup**:  
   - Enemy spawn caves are randomly placed around the border of the hex grid.  

- **Gameplay Actions**:  
   - Players can drag cards from their hand onto the hex grid to:  
     - **Spawn Towers**: Towers shoot at enemies periodically.  
     - **Cast Spells**: Unique effects are applied to the player or enemies.  

- **Resources**:  
   - **Nectar**:  
     - Each card has a nectar cost.  
     - Playing a card subtracts its cost from the player’s nectar total (shown in the top-right corner).  
     - If nectar is insufficient, the card is returned to the player's hand.  

- **Game Conditions**:  
   - **Winning**: All enemies are defeated.  
   - **Losing**:  
     - Player health (top-right) reaches 0.  
     - Enemies remain alive 10 seconds after the final wave.  


### Reporting bugs
Report to our [Issues](https://github.com/tmorris37/BeeHexed/issues) page
1. Check the list of bugs to see if your bug is already listed. If your circumstances and/or the cause of the bug aren't already listed, continue with the bug report. 
2. If not already listed, click the bug report button to send us an email following the provided template.
3. Give an explanation of what the bug is, and the steps taken to recreate the bug and/or any current present circumstances when the bug occurred.
4. Include important information such as the game version, and any other relevant information such as level/seed and towers/cards for game simulation bugs, and if necessary, the settings configuration for more overarching bugs like visual/audio bugs or general mechanic bugs. Screenshots/videos of the bug are helpful, though not necessary

### Known bugs
- Executable File can break based on assembly dll
- The game can still be interacted with after winning or losing
- losing then playing again will not reset progress
- There is a delay between casting certain spells and their effect on the bears
- Placing cards off the grid uses them without activating their effects
---
## Development Manual
### Obtaining source code 
Prerequisite, make sure [Git](https://git-scm.com/downloads) is installed, follow the instructions there.

To setup for development, do the following:
1. Download and Install [Unity Hub](https://unity.com/download)
    -  Sign in or Create an Account with Unity
2. Install Unity Editor version 2022.3.49f1 (LTS) through Unity Hub, if version not available download through [Unity Version Archive](https://unity.com/releases/editor/archive)
3. Clone this repository, below are steps for SSH cloning
    - In terminal or git bash `cd` into the directory you want the repo in
    - While in directory, `git clone`
```
git clone git@github.com:tmorris37/BeeHexed.git
```
5. In Unity Hub, click Add -> Add project from disk, then select where you cloned the repository to add the project to Unity Hub
6. In Unity Hub, ensure the Editor is the correct version and double-click to open the project (this will take a while)

### Directory Layout
- **Root/**: Contains bookkeeping files used by Unity and GitHub.
- **.github/**: Contains GitHub Actions workflows and CI/CD configurations.
- **.vscode/**: Stores workspace settings for Visual Studio Code.
- **ProjectSettings/**: Unity project settings and configuration files.
- **Packages/**: Unity package manifest and package-related metadata.
- **Assets/**: Developer-generated files organized into various subfolders.
  - **Animations/**: Contains all animation assets, including clips, controllers, and transitions.  
  - **Audio/**: Includes audio files such as sound effects, background music, and other game sounds.  
  - **Deck/**: Manages overworld map content, levels, and configurations related to deck-building mechanics.  
  - **Demigiant/**: Contains third-party assets or plugins imported from the Unity Asset Store.  
  - **Editor/**: Holds scripts and tools that extend the Unity Editor functionality.  
  - **Fonts/**: Stores font files and related assets used for in-game text.  
  - **HexSpriteTiles_Setup/**: Manages grid-based tiles, colors, and assets specific to the hexagonal grid system.  
  - **Palettes/**: Contains palette data and assets, such as color schemes or UI effects like hover interactions.  
  - **Prefabs/**: Stores pre-configured GameObjects (Prefabs) for reuse across scenes.  
  - **Resources/**: Holds persistent assets like Prefabs and JSON files that need to be accessed at runtime.  
  - **Scenes/**: Contains Unity scenes, including levels, menus, and testing environments.  
  - **Scripts/**: Defines game logic and behavior through C# scripts for GameObjects and systems.  
  - **Settings/**: Stores configuration files, project settings, and package definitions.  
  - **Sprites/**: Includes all 2D sprite images used for visuals and in-game graphics.  
  - **Tests/**: Contains Unity test scripts for verifying game logic and functionality.  
  - **TextMesh Pro/**: Manages text rendering and font assets using Unity's TextMesh Pro system.

### Building software 

Unity automatically builds the software. Go to:

1. File -> Build Settings
2. Select the scenes you want to build
3. Select the correct OS version
4. Build and Run

### Testing software 

There are a couple of options for testing the software. When you push to a branch, tests are automatically run. The other way to run tests is within Unity. Unity has a built in test-runner which can be found by going to Window on the top bar and clicking on the General subfolder. From here, individual tests can be run. Simulations of the game can be individually tested by selecting a scene, and then clicking the play button at the top of the screen. Not all scenes currently work

### Adding tests 

You can add new tests straight in Unity, as we are using the Unity testing suite. In the assets folder, proceed to the tests folder. In there there are multiple scripts which contain tests. If the tests you want to add are relevant to the name of any of the existing scripts, add your tests there. To create a new testing file, and name it describing what tests you will be running. Please name using the camelcase format. Use the previous scripts as reference if need be. 

### Building release 

[CI Tool : Github Actions](https://github.com/tmorris37/BeeHexed/actions)

In order to create a build of the game, in Unity go to file, and then build and run. From there you can choose where you want the build of the game to be stored on your computer.