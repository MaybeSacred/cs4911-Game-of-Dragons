Game of Dragons - Developer Documentation
------------------------------------------


Requirements:
--------------
Game of Dragons is developed using the free version of the Unity game 
engine. Unity can be downloaded at https://unity3d.com/unity/download

The 3D models used in our game were created with Blender and Maya. Each
program can be downloaded at,
Blender - http://www.blender.org/download/
Maya    - http://www.autodesk.com/products/autodesk-maya/free-trial


How to Test/Build/Run
----------------------
In the Unity editor, the project can be opened by selecting 'Open Project'
the the File menu and then choosing to open the downloaded directory
'cs4911-Game-of-Dragons' in the prompt. A scene file must now be opened
before testing or building. Select 'Open Scene' from the File menu and 
then choose 'Scene0-menu' to open the start menu of the game.

With a scene file open, you can test the game in the Unity editor by
clicking the play button at the top. The game can be built by selecting
'Build Settings' from the File menu. Make sure under 'Scenes in Build'
the scenes 'Scene0', 'Scene0-menu', and 'Scene0-end' are checked. Select
the web player platform. Click 'Build' and then choose a folder to put the
built files. After building, the game can be run by opening the build.html
file in a web browser.

We do not use an automated build because doing so is not reasonable within
the Unity framework.


Architecture Design:
---------------------
Our code architecture is detailed in the file CodeArchitecture.pdf. Due to
the flat nature of our class heirarchy, we do not provide a diagram of its
structure. Each class is a self-contained component which can be attached
to a game object in the Unity editor, and the behavior of each class and
function is described in code comments above each item. So, if you want to
understand all about how the Hellmet enemy works, it is only necessary to
look in the HellmetBehavior class.


User Interface:
----------------
The first screen of Game of Dragons is the start menu. This menu only has
two options, 'New Game' and 'Exit'. The exit button is only functional 
when the game is built as a standalone application as opposed to a web
application. The new game button starts a new instance of the game from 
the beginning.

After starting a game, the player will see their dragon avatar from a 
third person perspective. The player can move the dragon by pressing the
W/S/A/D keys, and the dragon will move in the corresponding direction 
based on the direction the player's perspective is pointing. Pressing
the shift key or the left mouse button will make the dragon attack with
fire breath, and pressing the space bar will allow the player to jump
up to three times without touching the ground.

The game interface also includes a HUD, which displays the player's state
and progress in the game. The top-left corner of the HUD has several 
hearts which represent how much health the player has based on how many
of the hearts are still red. Under the hearts is an image of fire, which
becomes more transparent as the player depletes their fire breath. The 
top-right corner of the HUD shows how many gem fragments the player has
collected as well as the number of fragments needed to create a full gem.
Next to the fragments is a display of how many full gems have been 
collected along with how many gems need to be collected to complete the 
game.