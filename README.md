# lighting-editor
Essential package providing editor scripts to redistribute lighting information

## Scene Lighting Specifications
Have you ever wondered, how other developers define their scene lighting settings?
- Re-typing everything is very time-consuming. But it looks like that it cannot be avoided.
- Making screenshots or opening the project twice to transfer the lighting settings from one scene to another is the naive way. But it is hard to restore earlier states, if developers does not make very small commits.
- Setting up a scene template from which every developer must create their own scenes does not work here. As far as I know, lighting information will not be transfered.

The easiest way, I can imagine, is to export and import scriptable objects. These objects should be placed inside an <i>Editor</i> folder to avoid inflating the project's build size. Now developers can easily transfer lighting settings from one scene to another by simply opening the lighting windows and the inspector window side by side.

The biggest advantage of this approach is that your Rendering Engineers can define a set of reliable lighting settings, which will help to apply graphical styles more easily to upcoming projects. 
