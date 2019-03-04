HFL version 0.7-incomplete readme
---------------------------------
This is incomplete software. If your computer crashes or you lose data because you used this, I cannot and will not be held responsible for any bad things that occur.
Use at your own risk.

Requirements
------------
- A C++ compiler that can compile OP2 missions (duh)
- The most recent Outpost 2 mission SDK
- A working copy of Outpost 2 (preferably the OPU version)
- Some patience as you work with code that is incomplete :-)

Files
-----
This is a description of the files inside this package:

Doc\		The (incomplete) manual for HFL
Example\	A really small example mission that shows how to use the GUI objects like command panes and buttons in HFL
Source\		The source code to HFL (the headers are located here, along with an MSVC++ 6 project that you can use to compile the library)
readme.txt	You're looking at it right now

Installation
------------
1. **delete step** Install the new SDK by copying the contents of the SDKInclude folder into your old Include folder for the SDK.
(You may want to make a backup of the old Include folder first). Overwrite existing files.

2. Unpack HFL into a directory of your choice (** specify location **).

3. Add HFL to your compiler's include and library paths.  (** fix package so no change is required, using known path **)
follow the directions in the manual, located in the Doc folder, to figure out how to set up your source code to work with HFL. (Specifically, the "basics" page is what you want).

4. #include <HFL.h> into your project, write code, set project to link with HFL.lib, compile, run :-)

Bugs / Help
-----------
I'm sure this has lots of bugs. If you find something that might be a bug, post about it on the forum.
Likewise, if you need help using anything in HFL, make a post regarding that.

And yes, I know one of the major 'bugs' is the lack of a complete manual. The manual also probably has the wrong version number listed, has pages missing, has stuff documented that doesn't even exist in the library, etc.
As far as documentation on the GUI objects like command panes and such, the manual isn't too helpful. You'd be best off looking at the example mission in the Example folder, and/or playing around until you get things to work.

What you see is what you get. I don't have the time to work on this project anymore, so you're pretty much on your own.
I can try to give some help regarding problems you may have but I can't guarantee anything. As far as the GUI stuff is concerned, don't expect a whole lot (it's been ages since I looked at any of the GUI code in Outpost 2, and I don't remember how much of it works at all).
I know there are others out there who can help you with HFL, I've given them copies ages ago because they needed some feature for a certain mission.

Good luck, and I hope this helps you in your Outpost 2 mission endeavors :-)

-- BlackBox (formerly op2hacker)
