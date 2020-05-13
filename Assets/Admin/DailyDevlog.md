Pizza Express Daily Devlog: 
    
**10-05-2020** - 
- Started work on the delivery 'powerup system' successfully got the 'Energised' delivery working as intended
- Started working on the bigger hands to allow the pizza to be effectivley doubled, but ran into issues with it spawing the models incorrectly. Not being able to effectivley check weather the player has the delivery and when it can be activated -- needs more work.
- Tried to fix some bugs that were caused due to updated features and code
- Opened git repo for cloud storage and version control. 

**11-05-2020**
- Implemented delivery of Bigger Hands -- Code is very ugly and convaluted needs re coding asasp. 
- Stuck for 6.5 hours on the movement issue, in between breaks tried to come back to the issue, but eventually gave up.  Done some research but nothing conclusive or tested that could help my issue. Found a way to not use GameObject.Find() instead referencing them to a list. Check  discord resources for the article.. will try that tomorrow..  Have a few feelers and some things to try out tomorrow but felt very deflated and frustrated today. Not much done at all. 
- Made the pizza slices destroy when colliding with the ground, and when they are out of camera render range. 

**12-05-2020**
- Fixed the issue that was causing the HappyHour from not activating properly. I was calling it from a prefab reference of the component and not the component from the instantiated gameObject.  Adding each instantiated customer to a list and getting a reference to each gameobjects component from that list worked.  Then removing each customer on 'fed status'
- Tidied up the code a little getting rid of unused public methods and variables, setting them to private and introduced setter getters for some methods that needed to be accessed from outside its script.
- Removed the spawn pizza coroutine and instead moved it to the update method - I fixed the isssue of letting the player effectivly stop the spawn of the pizzas and breaking the game by executing a check on the customer list.  
- Implemented a money mechanic which im not entirely sure i am happy with.  Seems the make the game unnessecerily hard... but does provide a risk reward element from getting the delivery or 'serving' customers. 
- Implemented Time_Is_Dragging delivery system, reversing the same element as above.. increased the pizza spawn timer and decreased the player and customers speed at random. 
- Implemented Overtime -- made additional customers spawn between a set range -- set the check to the current wave spawner. ***DID NOT TEST LEGITIMATLEY BUT WORKED ON DEBUG TEST***
- Implemented WindFall - Changed the name to represent its 'boost' and added another ability to give a double boost of cash 