# The_Pattern_Bug_MazeSolver
A program with a home cooked pathfinding algorithm to solve uniformly square mazes. KEA Freelance Assigment. Solo XP

Scripts are in the MazeSolver folder.

This version provides one solution per starting field of maze; future version will grant multiple solutions per starting field.

I have no idea how to accurately characterize the type of pathfinding algorithm I cooked up, but it works much like a Djikstra solution, except there is no scoring, and so it is not a shortest distance pathfinder (which is potentially contradictory in terms)... but a path will be found if it is there.



# Try it:
If you copy the content of Assets into a Unity project Assets folder, you should be able to run the scene SampleScene out of the box (in the Unity Editor).

SampleScene - first solution

Djikstraesque - second solution, not a proper Djikstra implementation, but goodish

# 2nd version finished; it finds a path of least resistence (cheapest w…

…eights, one path taken), not the shortest path; name refactoring to follow.

The Djikstraesqeue.unity scene now works (though not a Djikstra algorithm).

<b>Double clicking on a node changes it's value:</b>
<ul>
  <li><p>Node of yellow color is walkable node</p></li>
<li><p>Node with cross allows no passage </p></li>
<li><p>Node of blue color is starting node</p></li>
<li><p>Node with green 'correct' symbol is goal/winning node</p></li>
</ul>

<img src="https://raw.githubusercontent.com/kiancn/The_Pattern_Bug_MazeSolver/master/Assets/_Extra/DjikstraesquePathfindingSceneInAction.png" alt="Screenshot of DjikstraesquePathfindingSceneInAction"></img>

<h3>Click the larger green area beneath the "Much node:" sign to create more nodes.</h3>
<p><b>Click and hold mouse on node to move it.</b></p>
<p></p>
<p><b>NB. Nodes can be too far apart to allow passage!</b></p>
