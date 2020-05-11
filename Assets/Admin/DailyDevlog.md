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


**BR help with rush hour**
> I really need to get some sleep already, but if you do it like this:
> public GameObject customerPrefab; //<-link you prefab here
> [System.NonSerialized] HungryCustomerMovement  customer;
> 
> SomeSpawnMethod() {
> customer = (HungryCustomerMovement)Instantiate(customerPrefab, customerSpawns[spawnerIndex].transform.position, customerPrefab.transform.rotation);
> }
> 
> SomeOtherMethod() {
> if ( customer != null ) 
> customer.SetDefaultSpeed(); //Calls just the method from the class
> }
