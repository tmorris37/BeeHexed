# BeeHexed
The all new roguelike tower defense game, focusing on magical bees!
## Administrativa
[Living Document](https://docs.google.com/document/d/1YeMs-TCpdy3aiPiGqQ2wvI9agQTX7e68viUun7czUgE/edit?usp=sharing “Beehexed Team Living Document”) (Must sign in with UW-affiliated email)

Uses Unity 2022.3.49f1 (LTS)
## User Manual
Beehexed is a game running on the Unity Engine, it is a roguelike deckbuilder hex tower defense game. This game is intended for people that love roguelike deckbuilders as a refreshing hybrid take on deckbuilders and tower defense.
### Installation
Functionality missing: How to install the software
Our current intention is to publish this game on itch.io and all the required files to play the game. Users would install the game from the distribution site. 
In the meantime, do the following:
Prerequisite, make sure [Git](https://git-scm.com/downloads “download git”) is installed, follow the instructions there
1. Download and Install [Unity Hub](https://unity.com/download “Unity Download”)
    a. Sign in or Create an Account with Unity
2. Install Unity Editor version 2022.3.49f1 (LTS) through Unity Hub, if version not available download through [Unity Version Archive](https://unity.com/releases/editor/archive “Unity Version Archive”)
3. Download the [Zip file](https://github.com/tmorris37/BeeHexed/archive/refs/heads/main.zip “Beehexed Zip Download”) of this Repo
4. Unzip the file and place into a directory
4. In Unity Hub, click Add -> Add project from disk, then select where the unzipped Beehexed folder
5. In Unity Hub, ensure the Editor is the correct version and double-click to open the project (this will take a while)
### Running software
Missing Functionality: An executable file that starts a game. 
The objective is that all it takes to run the game is to open the executable file.
In the meantime, do the following:
1. Open the Unity Project you created above
2. In the project window at the bottom of your screen open the 'Scenes' Folder
2. Double click on the Scene called MainMenu to open it
4. Click the play button at the top of the screen to start/stop the game (you may want to change Play Focused to Play Maximized)
### User guide
On startup the user will be presented with a main menu. They will be able to click New Game and be brought into the game.
The user is brought to the overworld level selection map. From here they can click on one of the flashing icons to start that level. 
This will bring the player into the game. A level takes place over the course of several rounds. Each round the player gains nectar and cards, but enemies will spawn from the caves.
The bears will move to attack the center, which the player must defend using their cards. Tower cards place a tower that attacks the bears. Spell cards place unique effects on the player or enemies.
Cards are played by dragging them from the hand to a tile on the hex grid. 
Each card has a nectar cost that is subtracted from the player's total nectar in the top right. If the player lacks sufficient nectar, the card is returned to the hand.
The player loses the game if their health in the top right is reduced to 0 OR if there are still enemies present 10 seconds after the final wave.
The player wins the game if they have defeated all enemies 10 seconds after the final wave.
### Use Cases
#### Main Menu
Users will see a main menu with 3 buttons: New Game, Settings, and Exit.
Clicking New Game will begin a run, bringing the player to the level select map
Clicking settings takes the user to the settings page with a volume slider and a back button. Clicking the back button takes the user back to the main menu.
Clicking exit will end the game
#### Level 
Upon starting the level, caves that spawn enemies will be randomly placed on the border of the hex grid.
Players can drag cards to tiles to spawn towers that will occasionally shoot at enemies and cast spells.
### Reporting bugs
1. Check the list of bugs to see if your bug is already listed. If your circumstances and/or the cause of the bug aren't already listed, continue with the bug report. 
2. If not already listed, click the bug report button to send us an email following the provided template.
3. Give an explanation of what the bug is, and the steps taken to recreate the bug and/or any current present circumstances when the bug occurred.
4. Include important information such as the game version, and any other relevant information such as level/seed and towers/cards for game simulation bugs, and if necessary, the settings configuration for more overarching bugs like visual/audio bugs or general mechanic bugs. Screenshots/videos of the bug are helpful, though not necessary

We may include a popup when unusual things occur (e.g. unexpected backend errors or if games run too long without ending) to push users to report bugs. We could also have statistics automatically exported to allow users to easily report game information.
### Known bugs
The game can still be interacted with after winning or losing
Winning/Losing does not bring the player back to the level select map
There is a delay between casting certain spells and their effect on the bears
The bears do not do any damage to towers in their path
Placing cards off the grid uses them without activating their effects


## Development Manual
### Obtaining source code 
The source code is stored at this GitHub repository. This is where all of the files generated for this project are located. In order to obtain the Unity-generated files, a developer should download Unity 2022.3.49f1 (LTS) and allow it to build in the same folder as the source code on their local machine. This allows for complete access to this project for further development.

Prerequisite, make sure [Git](https://git-scm.com/downloads “download git”) is installed, follow the instructions there
In the meantime, do the following:
1. Download and Install [Unity Hub](https://unity.com/download “Unity Download”)
    a. Sign in or Create an Account with Unity
2. Install Unity Editor version 2022.3.49f1 (LTS) through Unity Hub, if version not available download through [Unity Version Archive](https://unity.com/releases/editor/archive “Unity Version Archive”)
3. Clone this repository, below are steps for SSH cloning
In terminal or git bash `cd` into the directory you want the repo in
Git Clone in the directory
```
git clone git@github.com:tmorris37/BeeHexed.git
```
4. In Unity Hub, click Add -> Add project from disk, then select where you cloned the repository to add the project to Unity Hub
5. In Unity Hub, ensure the Editor is the correct version and double-click to open the project (this will take a while)

### Directory layout
In the GitHub repository, there are several files immediately available at root. These files are used for bookkeeping purposes, either by Unity or by GitHub. Similarly, the folders .github, .vscode, ProjectSettings, and Packages are also used by Unity and GitHub for bookkeeping. In the Assets folder, the developer-generated files are broken up into Resources, Scripts, Scenes, Sprites, and Tests. The Resources folder contains all of the Prefabs and JSON files. This is the preferred location for persistent files that are needed after compilation. In Scripts, developer-defined behaviors of gameobjects are dictated. In Scenes, the different scenes that can be used in Unity are located. It includes individual levels, testing scenes, and menus. In Sprites, images of in-game sprites are located. In game, these images are spawned in and shown to the player. Finally, Tests includes all of the Unity tests the developers have created.
### Building software 
Unity automatically builds the software.
### Testing software 
There are a couple of options for testing the software. When you push to a branch, tests are automatically run. The other way to run tests is within Unity. Unity has a built in test-runner which can be found by going to Window on the top bar and clicking on the General subfolder. From here, individual tests can be run. Simulations of the game can be individually tested by selecting a scene, and then clicking the play button at the top of the screen. Not all scenes currently work
### Adding tests 
You can add new tests straight in Unity, as we are using the Unity testing suite. In the assets folder, proceed to the tests folder. In there there are multiple scripts which contain tests. If the tests you want to add are relevant to the name of any of the existing scripts, add your tests there. To create a new testing file, and name it describing what tests you will be running. Please name using the camelcase format. Use the previous scripts as reference if need be. 
### Building release 
[CI Tool : Github Actions](https://github.com/tmorris37/BeeHexed/actions “BeeHexed Github Actions”)
In order to create a build of the game, in Unity go to file, and then build and run. From there you can choose where you want the build of the game to be stored on your computer. GAME IS CURRENTLY UNPLAYABLE LIKE THIS 

