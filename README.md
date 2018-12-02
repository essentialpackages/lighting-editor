# lighting-editor
Essential package providing editor scripts to redistribute lighting information

## Scene Lighting Specifications
Have you ever wondered, how other developers define their scene lighting settings?
- Re-typing everything is very time-consuming. But it looks like that it cannot be avoided.
- Making screenshots or opening the project twice to transfer the lighting settings from one scene to another is the naive way. But it is hard to restore earlier states, if developers does not make very small commits.
- Setting up a scene template from which every developer must create their own scenes does not work here. As far as I know, lighting information will not be transfered.

The easiest way, I can imagine, is to export and import scriptable objects. These objects should be placed inside an <i>Editor</i> folder to avoid inflating the project's build size. Now developers can easily transfer lighting settings from one scene to another by simply opening the lighting windows and the inspector window side by side.

The biggest advantage of this approach is that your Rendering Engineers can define a set of reliable lighting settings, which will help to apply graphical styles more easily to upcoming projects. 

### FAQ

#### Why there aren't any dropdown fields but instead string fields
If enumerations must be modified someday, it would affect the settings from all existing instances of the scriptable objects.

#### Is there any other way to restore scene lighting information
If you are using some kind of version control software, if you have set the mode of Asset Serialization to Force Text, and if you are only making small commits, then you should be able to restore scene lighting information from the past. But it can become very expensive. Restoring an old state of a scene will also affect the scene graph. More likely you only want to reset the lighting information. In this case you must open the .unity file inside a text editor and search for specific keywords manually. Unfortunately, the names of entries inside this file differ from the fields, being presented within the scene lighting tab. Either way this workflow is very expensive.
