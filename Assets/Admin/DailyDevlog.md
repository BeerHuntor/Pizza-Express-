Daily Development Log 2020:
    
**10-05-2020** 
    **Pizza Express**
        - Started work on the delivery 'powerup system' successfully got the 'Energised' delivery working as intended
        - Started working on the bigger hands to allow the pizza to be effectivley doubled, but ran into issues with it spawing the models incorrectly. Not being able to effectivley check weather the player has the delivery and when it can be activated -- needs more work.
        - Tried to fix some bugs that were caused due to updated features and code
        - Opened git repo for cloud storage and version control. 

**11-05-2020**
    **Pizza Express**
        - Implemented delivery of Bigger Hands -- Code is very ugly and convaluted needs re coding asasp. 
        - Stuck for 6.5 hours on the movement issue, in between breaks tried to come back to the issue, but eventually gave up.  Done some research but nothing conclusive or tested that could help my issue. Found a way to not use GameObject.Find() instead referencing them to a list. Check  discord resources for the article.. will try that tomorrow..  Have a few feelers and some things to try out tomorrow but felt very deflated and frustrated today. Not much done at all. 
        - Made the pizza slices destroy when colliding with the ground, and when they are out of camera render range. 

**12-05-2020**
    **Pizza Express**
        - Fixed the issue that was causing the HappyHour from not activating properly. I was calling it from a prefab reference of the component and not the component from the instantiated gameObject.  Adding each instantiated customer to a list and getting a reference to each gameobjects component from that list worked.  Then removing each customer on 'fed status'
        - Tidied up the code a little getting rid of unused public methods and variables, setting them to private and introduced setter getters for some methods that needed to be accessed from outside its script.
        - Removed the spawn pizza coroutine and instead moved it to the update method - I fixed the isssue of letting the player effectivly stop the spawn of the pizzas and breaking the game by executing a check on the customer list.  
        - Implemented a money mechanic which im not entirely sure i am happy with.  Seems the make the game unnessecerily hard... but does provide a risk reward element from getting the delivery or 'serving' customers. 
        - Implemented Time_Is_Dragging delivery system, reversing the same element as above.. increased the pizza spawn timer and decreased the player and customers speed at random. 
        - Implemented Overtime -- made additional customers spawn between a set range -- set the check to the current wave spawner. ***DID NOT TEST LEGITIMATLEY BUT WORKED ON DEBUG TEST***
        - Implemented WindFall - Changed the name to represent its 'boost' and added another ability to give a double boost of cash 

**13-05-2020**
    **Pizza Express**
        - Fixed the issue of the delivery crates not cancelling after a alloted time -- Created a coroutine to handle all the settings of bool to be called on crate destroy -- ran into an issue where the routine would stop running once gameobject was destoryed not allowing the  routine to be completed.  Solved this by calling it by proxy. 
        - Got the players animation to stop playing once the player movement has stopped. Ran into issue where after colliding with a game object in the scene when player stops, it would alternate between the 2 animations -- ***Issue Tracked***
        - Fixed the bug where the player would destroy the pizza when picked up via the bin, and have infinate slices.

**14-05-2020**
    **Pizza Express**
        - Worked on art for the UI -- Customers remaining and counter health icons. 
        - Implemented Customers remaining icon and counter health icon into the game. 
        - Streamed developement of art design for 7 hours - Enjoyed the interactivity -- possibly do more. 
        - Started idea planning for delivery icons in photoshop, energized has been produced, and animated.. Not sure if it is suitable and not entirely happy with it. 

**15-05-2020**
    **Pizza Express**
        - Continued work of the UI sprites -- Designed and completed RushHour, Cash Flow,  Overtime and WindFall sprites. 
        - Streamed the design process on twitch. 

**16-05-2020**
    **General**
        - Spent the early afternoon with the unity premium course, survival with c# learning intemdiate level theory
    **Pizza Express**
        - Deisgned and coloured icons for TimeIsDragging, and 'BiggerHands' delivery.. BiggerHands to be changed to DoubleSlices. 
        - Designed and colored money_note to be used as particle effect. 
        - Designed and colored money representation for the UI. 
        - Started design and rework of the title screen with a new how to play screen. 

**17-05-2020**
    **Pizza Express**
        - Imported finished delivery icons into the game, and implemented them to be functional -- added animations to the icons which pop up when deliveries are collected. 
        - Designed and implemented new main menu title screen complete with hover animation.
        - Deisgned and implemented new game over screen with animation -- added daycount as a stat tracker on the game over screen.
        - Completed design and implementation of how to play screen.  Now has a tutorial that points back to the main menu. 
        - Did some general code cleanup, serialized ui variables -- made it easier to activate / deactivate different ui elements. 

**19-05-2020**
    **Pizza Express**
        - Modeled and textured the delivery crate - used blender to sculpt the detail and used multi resolution glitch to bake texture onto low poly model. 
        - Re textured the pizza's to resemble from the same texture
        - Reset the pizza attributes in unity, had to reset the transforms and references to them.
        - Livestreamed the process for 8 hours. 
        - Implemented a very crude particle system that shows up behind the money UI when a customer is fed.  Not particularly happy with it.
        - Added despawner empty underneath the ground platform in an attempt to despawn the pizza slices, some reason OnBecomeInvisible() was no longer being called. 
        - Fixed happy customers collider box's not turning off when they hit hungry customers. 

**21-05-2020**
    **Other**
        - Spent the day trying to get my head around webscraping for a feature of the discord bot I intend to make for the BU discord. 
        - Also developed an RSS feed watcher with customisable time witch pulls the latest post / or the full page of posts in a given time frame and returns the title and url. 

**22-05-2020**
    **Pizza Express**
        - Cleaned up some code, removed redundant methods and varaibles and trimmed down the fat. 
        - Fixed the how to play screen mistake, and added a picture of the delivery crate to it. 
        - Split the code of the UI from game manager and created a new ui manager class to handle all the ui calls and updates.
        - Changed the game running check to make the code more legible and understandable. 
        - Uploaded and created a page on itch.io
        - Started work on sound manager to call the sounds from. 

**25-05-2020**
    **Pizza Express**
        - Cleaned up the code, used singletons to reference classes instead of GO.Find() Thanks to dev, a viewer on my stream. 
        - Added menu and game music when in the menus and when in the game - The game menu is TERRIBLE... needs re doing, try chiptune. 
        - Fixed the issue with windfall icon not removeing,  wasnt calling the coroutine. 
        - All sound effects are working as intended aswell as voice overs. 
        - Added game options menu in the main menu and a pause menu that appears when paused with adjustable sound sliders - used mixer to have different audio channels for music and sound effects allowing for each to be independantly adjusted.
        - Streamed the process on twitch for 7.5 hours. 
        - Play tested the game myself,  and left thoughts of the experience in the feedback document. 
        - Removed the animation on the interactable buttons and replaced with a straight sprite swap. 
**26-05-2020**
    **Pizza Express**
        - Worked on marketing updating the itch.io page filling in the description and adding screenshots. Changed the page to be public but dissallowing new downloads. 
        - Re built Standalone and WebGL of v0.4 
        - Unity project page updated with latest WebGL build - Future updates on itch only. 
        - Posted first devlog onto itch, for build version 0.4

**28-05-2020**
    **Pizza Express**
        - Fixed a bug from feedback where the double slices delivery was not activating properlly allowing the player with an empty hand to collect another delivery - The fix was in where the boolean that set the double slices active was only activating when the player picked up a pizza, thus without pizza they could circumvent the check allowing for additional deliverys when not suppose to. 
        - Used scenemanger to load the scene on restart, now will allow the player to restart the game sucsessfully. 
        - Went through issue tracker list and fixed some bugs. 

### TAKING A BREAK FROM DEVELOPMENT TO PLAY SOME GAMES... 

**08-06-2020**
    **General**
        - Looked at some exercises on exercism and refactored leap.cs

**10-06-2020**
    **Pizza Express**
        - Made a new branch of the repo to implement the new improvements based on gameplay feedback. 
        - Used inheritence to enable me to create variable customers without having to add new models and prefabs. Base customer class now is inherited by both hungry and happy customers
        - Using the above I was able to implement variable hungry values for each spawned customer and integrate that into the 'feeding' system, so feeding a customer will minus from their hunger value until 0 at which point they will be fed. 
        - Using inheritence I also added a variable speed output.. There is currently no relation to speed and hunger value.  All completeley done at object creation at random. 
        - Changed color of the spotlight in relation to hunger values a different color for each value (Currently 1-3 slices) **This will need keeping an eye on for difficulty** 
        - Started working on getting the sprites to render when instantiated and when fed slices -- Not tested it!.

**11-06-2020**
    **Pizza Express**
        - Went against the initial plan of having a sprite render the happy smiley when customer has been fed, instead made an empty gameobject spawn in the same place as where the happy customer is spawned and made it delete itself after 3 units. This was to solve the issue of not being able to get the sprite rotate towards the player in a 3d environment.
        - Created sprites for each customer hunger value. 
        - Made sprites appear above the hungry customers head when they get instantiated. I had to do this in the movement script and not the base class as I could not find a way to get it to work by having it be set in the base class.  Not what I wanted to do or intended to do.  But its done. 

**12-06-2020**
    **Pizza Express**
        - Refactored the pizza slice spawning to make it a little easier on the eyes, still not completley happy with it, but removed some repeating code. 
        - Fixed an issue with the pizza slices not activating correctly.  They were activating on the current pizza not the next pizza. 
        - Fixed the pause menu where when pausing inbetween waves with the countdown text showing,  it wouldn't dissapear making the slider selection difficult.
        - Stopped the pizzas from spawning when game over is called
        - Fixed a few bugs that I found that haven't been reported or yet seen. Game over was flashing before actually restarting the game,  removing the hide UI call from the button fixed this.  Also made the pizza slices that are fired despawn on contact with the trigger AND the ground. 
        - Added new menu music and game music, aswell as new menu sfx when hovering and clicking.  Changed the default volume down from whatever it was at default, to now be 0.3f for music, and 0.6f for sfx. 
        - Created script for buttons to call the sound clips. 
        - Refactored the switch statement in auidomanager to select the clips, alot of same code in multiple places was replaced by a method call. 