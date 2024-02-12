using UnityEditor;

namespace MicroCombo
{
    [CustomEditor(typeof(MicrogameData), true)]
    [CanEditMultipleObjects]
    public class MicrogameDataEditor : Editor
    {    
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var picker = target as MicrogameData;

            // Picker for each scene
            for (int i = 0; i < MicrogameData.SceneCount; i++)
            {
                var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(picker.GetSceneName(i));// picker.scenePath);
                var variableName = picker.GetSceneVariableName(i);

                serializedObject.Update();

                EditorGUI.BeginChangeCheck();
                var newScene = EditorGUILayout.ObjectField(ObjectNames.NicifyVariableName(variableName), oldScene, typeof(SceneAsset), false) as SceneAsset;

                if (EditorGUI.EndChangeCheck())
                {
                    var newPath = AssetDatabase.GetAssetPath(newScene);
                    var scenePathProperty = serializedObject.FindProperty(variableName);//"scenePath");
                    scenePathProperty.stringValue = newPath;
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}