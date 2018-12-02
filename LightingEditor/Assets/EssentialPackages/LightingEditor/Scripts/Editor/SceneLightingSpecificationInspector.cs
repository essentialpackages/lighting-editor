using UnityEditor;

namespace EssentialPackages.LightingEditor.Editor
{
    [CustomEditor(typeof(SceneLightingSpecification))]
    public class SceneLightingSpecificationInspector : UnityEditor.Editor
    {
        private void OnEnable()
        {
            
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}
