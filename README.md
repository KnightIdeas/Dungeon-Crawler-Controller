# Dungeon-Crawler-Controller
Unity3D controller script for use in dungeon crawler games such as Eye of the Beholder and Legend of Grimrock

To use this script will require some assets from the Unity pack included, the only necessary item is the player prefab but the demo level I created is included for you to try out!

What can the controller do?
This controller allows you to move forward and backwards, sidestep left and right in all the cardinal directions, it can also rotate the player 90o either left or right.
The controller can also detect walls in front of it either through tags which give better flexibility or via distance which will work well for simpler games.
The controller can also climb stairs and ramps of a set height.
It takes in the current grid size that you are working with and tags if you wish to use them otherwise this controller stands alone which was one of my aims when I made it.

Coding breakdown.
Basic movement uses movetowards which has a step variable built in.
Rotation uses Slerp and needs some basic rounding correction.
Movement is triggered on input via bool.
Movement is triggered off when the player object reaches the target, this is measured with simple measurement checks.
Directional raycasts are used during movement input to briefly check the distance in front of the player for solid objects, tags can be added as a check to give greater flexibility here.
There is a small increase to the wall check distance because occasionally the player could ghost through a wall if you moved around it in different directions, if you encountered this problem simply increase this value.
Height correction was a little trickier and I ended up using a raycast distance detection to push up or pull down the player only during movement based on a set distance height.
Also using a similar method I made sure that the target position was shifted up or down with the player's movement to stop the player trying to reposition itself at the end of its movement.

I wanted to give a big thankyou to LutzGrosshennig as his was the code that I started working with at the outset, I rebuilt my own controller from his parts only because I'm too new to coding to know what the script was doing and I needed to do so for comprehension as well as simplification.
https://github.com/LutzGrosshennig

You can find his code here.
https://github.com/LutzGrosshennig/unity3d-AnimatedGridMovement-Camera
He also has a newer version that you can reach via the link also.

If you have questions or suggestions please let me know, I'm going to be working on other aspects of my game now and I'll update the code here when I have time.
