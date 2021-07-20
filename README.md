#  Conway's Game of Life - in Godot

![image](https://user-images.githubusercontent.com/10135321/126365813-0354b79c-246a-4f86-8ebe-69745590edce.png)

This is a simple project using [Godot C#](https://godotengine.org/). 

It follows the rules detailed in the wiki article.
[Conway's Game of Life - Wikipedia](https://en.wikipedia.org/wiki/Conway's_Game_of_Life)

Right now, it randomly generates cells with a 50% chance of being alive or dead, and then runs. 

When a cell lives long enough it will become lighter, to show the cells that have been there the longest. 
Dead cells slowly fade away in a nice way.

The whole project runs from a single scene, node and script file all called Root.
Script parameters are exported to allow for experimentation in the node editor.
