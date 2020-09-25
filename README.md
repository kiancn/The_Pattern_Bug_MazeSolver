# The_Pattern_Bug_MazeSolver
A program with a home cooked pathfinding algorithm to solve uniformly square mazes. KEA Freelance Assigment. Solo XP

Scripts are in the MazeSolver folder.

This version provides one solution per starting field of maze; future version will grant multiple solutions per starting field.

I have no idea how to accurately characterize the type of pathfinding algorithm I cooked up, but it works much like a Djikstra solution, except there is no scoring, and so it is not a shortest distance pathfinder (which is potentially contradictory in terms)... but a path will be found if it is there.



# Try it:
If you copy the content of Assets into a Unity project Assets folder, you should be able to run the scene SampleScene out of the box (in the Unity Editor).

SampleScene - first solution

Djikstraesque - second solution, a proper Djikstra implementation
