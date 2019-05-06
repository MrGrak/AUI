# AUI  

An Animated User Interface library for Monogame  


![](https://github.com/MrGrak/AUI/blob/master/Imgs/aui_003.gif)  



## Goals

To be an animated user interface library that is user facing.
This library is not meant to replace winforms or other backend
ui libraries. This is a frontend ui library that users interact
with in your game, that is animated and interesting to play with.



## A Brief Introduction/Overview

In order to understand why the UI was designed the way it was,
I will first need to explain how xna/monogame works, a bit.

In Monogame and XNA, the base game class has two methods that
are called each frame, Update() and Draw(). Update handles game
logic and Draw handles putting sprites, fonts, textures, on screen.
Usually Input is handled in the Update method. 
This AUI library has been designed around this Update and Draw 
interface, and every AUI class has both an Update() and Draw()
method. All AUI objects inherit from the abstract base class,
which forces all AUI objects to implement their own Open(),
Close(), Update(), and Draw() methods.



## Making a Button

There are 5 steps to working with an AUI object, but this can
be reduced to 3, depending on how much control you want over
the ui object. We'll start with the verbose 5 step process.

```csharp
//1. create the ui instance in a constructor or open()
AUI_Button button_example = new AUI_Button(
	16 * 3, 16 * 2 + 8,  //x, y pos on screen
	16 * 3, "example");  //width, text displayed

//2. open the ui instance from an open()	
button_example.Open();

//3. close the ui instance from a close()
button_example.Close();

//4. update the ui instance from an update()
button_example.Update();

//5. draw the ui instance from a draw()
button_example.Draw();

```

How you want your ui to appear to the user determines how
and when your ui objects call their Open() and Close() methods.
You can immediately open a ui object upon it's creation,
or you can create it and then open it later, if you want
cascading animations of ui opening, or want to delay the
opening (or closing) of ui objects, for artistic purposes.





## Making Tons of Buttons

Now we're going to explore the power of AUI objects and
their design by making a few hundred buttons with just
a few extra lines of code.

```csharp
//0. define a list to put aui objects on, via base class
List<AUI_Base> aui_instances = new List<AUI_Base>();

//1. add ui objs to list in constructor or open
for (i = 0; i < 500; i++)
{ 
	AUI_Button button_example = new AUI_Button(
	16 * 3, 10 + i * 16,  //x, y pos on screen
	16 * 3, "example");  //width, text displayed
    aui_instances.Add(button_example);
}

//2. open the ui instances from an open()	
for (i = 0; i < aui_instances.Count; i++)
{ aui_instances[i].Open(); }

//3. close the ui instances from a close()
for (i = 0; i < aui_instances.Count; i++)
{ aui_instances[i].Close(); }

//4. update the ui instances from an update()
for (i = 0; i < aui_instances.Count; i++)
{ aui_instances[i].Update(); }

//5. draw the ui instances from a draw()
for (i = 0; i < aui_instances.Count; i++)
{ aui_instances[i].Draw(); }

```

Now if we want to add more AUI objects, we only have
to create them and add them to the AUI instances list.

```csharp

//add some text AUI objects to the screen/game
for (i = 0; i < 100; i++)
{ 
	AUI_Text text_example = new AUI_Text(
		"example",		//string to draw
		16 * 8, 16 * i, //x, y pos	
		Color.Blue);	//color
    aui_instances.Add(text_example);
}

```


## Advanced Examples

All advanced examples require explaining how fonts and 
textures are loaded and drawn in xna/monogame. Explaining 
all that is outside the scope of this AUI lib's readme, 
so it won't be done here. Instead it is suggested that you 
look at the getting started guide, to get an understanding 
of how monogame works and the expected way that content is 
loaded and drawn. If you understand how content is loaded 
and how spriteBatch works, then the next step is grasping
the custom screens and screenmanager that the example 
game uses.

In the example game, screens have the expected update and
draw methods, but they also have open and close methods like
the AUI objects do. This was done on purpose, so calling a
screen's Open() would in turn call all the Open() methods
on all the screen's AUI items, via iterating the AUI list.
This same idea is applied to close, update, and draw.

This limits the amount of what a developer has to create
in order to get AUI objects working in their game. A dev
needs only to construct an AUI object, set optional 
parameters, then add it to a List of AUI instances. In 
the update() method, mouse/keyboard/controller input is 
collected and then evaluated against the AUI objects 
htiboxes to determine overlap (hover and click states, 
for example). 

To see click events in action, check out the Screen_Title.cs
file in the example game project.

To see sliders and animated buttons in action, check out 
the Screen_Example1.cs file in the example game project.



























