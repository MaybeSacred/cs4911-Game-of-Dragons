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
function is described in code comments above each item.


User Interface:
----------------



