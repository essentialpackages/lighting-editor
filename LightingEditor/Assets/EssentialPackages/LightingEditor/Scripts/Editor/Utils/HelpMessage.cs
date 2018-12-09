namespace EssentialPackages.LightingEditor.Editor.Utils
{
    public class HelpMessage
    {
        public const string Summary =
            "This scriptable object enables developers to version specific lighting settings." +
            "It can be used to transfer lighting settings to other scenes as well as to" +
            " create design guidelines.";

        public const string Advice =
            "Unless there are many scenes with the same lighting settings," +
            " developers should always create one instance per scene inside an Editor folder." +
            " Ideally choose a name for the scriptable object which is similar to the name of" +
            " the corresponding scene.";

        public const string Footnote =
            "Active values of dropdown menus (inside the scene lighting window) are stored as" +
            " strings (inside the scriptable object). This will assure that settings are not" +
            " accidentally changed when upgrading Unity to a version, having different" +
            " enumerations. But this does also mean, that developers should check all" +
            " instances previously created, when upgrading Unity/project.";
    }
}