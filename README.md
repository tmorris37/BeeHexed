# BeeHexed
The all new roguelike tower defense game, focusing on magical bees!
## Administrativa
[Living Document](https://docs.google.com/document/d/1YeMs-TCpdy3aiPiGqQ2wvI9agQTX7e68viUun7czUgE/edit?usp=sharing) (Must sign in with UW-affiliated email)

Uses Unity 2022.3.49f1 (LTS)
## User Manual
Beehexed is a game running on the Unity Engine, it is a roguelike deckbuilder hex tower defense game. This game is intended for gamers that love roguelike deckbuilders as a refreshing hybrid take on deckbuilders and tower defense.
### Installation
Functionality missing: How to install the software
Our current intention is to publish this game on itch.io and all the required files to play the game. Users would install the game from the distribution site. 
### Running software
Missing Functionality: An executable file that starts a game. 
The objective is that all it takes to run the game is to open the executable file.
### Use guide
Missing Functionality: A working software product.
This is the section where our user guide will be, but given it is not complete, there is not much to write. 
To give a general idea, on startup the user will be presented with a main menu. They will be able to click start and be brought into the game. The user will then be able to play through the game. The game will exit on completion, or the user can exit from the in-game menu. After exiting the game, they will be brought back to the main menu, from which they can play again or quit to the desktop, closing the program.
### Reporting bugs
1. Check the list of bugs to see if your bug is already listed. If your circumstances and/or the cause of the bug aren't already listed, continue with the bug report. 
2. If not already listed, click the bug report button to send us an email following the provided template.
3. Give an explanation of what the bug is, and the steps taken to recreate the bug and/or any current present circumstances when the bug occurred.
4. Include important information such as the game version, and any other relevant information such as level/seed and towers/cards for game simulation bugs, and if necessary, the settings configuration for more overarching bugs like visual/audio bugs or general mechanic bugs. Screenshots/videos of the bug are helpful, though not necessary
We may include a popup when unusual things occur (e.g. unexpected backend errors or if games run too long without ending) to push users to report bugs. We could also have statistics automatically exported to allow users to easily report game information.
### Known bugs
No known major game bugs since the game in the public main branch since the game is unfinished at the moment. 

## Development Manual
### Obtaining source code 
The source code is stored at this GitHub repository. This is where all of the files generated for this project are located. In order to obtain the Unity-generated files, a developer should download Unity 2022.3.49f1 (LTS) and allow it to build in the same folder as the source code on their local machine. This allows for complete access to this project for further development.
### Directory layout
In the GitHub repository, there are several files immediately available at root. These files are used for bookkeeping purposes, either by Unity or by GitHub. Similarly, the folders .github, .vscode, ProjectSettings, and Packages are also used by Unity and GitHub for bookkeeping. In the Assets folder, the developer-generated files are broken up into Resources, Scripts, Scenes, Sprites, and Tests. The Resources folder contains all of the Prefabs and JSON files. This is the preferred location for persistent files that are needed after compilation. In Scripts, developer-defined behaviors of gameobjects are dictated. In Scenes, the different scenes that can be used in Unity are located. It includes individual levels, testing scenes, and menus. In Sprites, images of in-game sprites are located. In game, these images are spawned in and shown to the player. Finally, Tests includes all of the Unity tests the developers have created.
### Building software 
Unity automatically builds the software.
### Testing software 
There are a couple of options for testing the software. When you push to a branch, tests are automatically run. The other way to run tests is within Unity. Unity has a built in test-runner which can be found by going to Window on the top bar and clicking on the General subfolder. From here, individual tests can be run.
### Adding tests 
You can add new tests straight in Unity, as we are using the Unity testing suite. In the assets folder, proceed to the tests folder. In there there are multiple scripts which contain tests. If the tests you want to add are relevant to the name of any of the existing scripts, add your tests there. To create a new testing file, and name it describing what tests you will be running. Please name using the camelcase format. Use the previous scripts as reference if need be. 
### Building release 
In order to create a build of the game, in Unity go to file, and then build and run. From there you can choose where you want the build of the game to be stored on your computer. 