#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using WaterGame.Showcase;

namespace WaterGame.Showcase.Editor
{
    public static class ShowcaseDemoBuilder
    {
        private const string Root = "Assets/WaterGame/CharacterShowcase";
        private const string ScenePath = Root + "/Demo/CharacterShowcaseDemo.unity";

        [MenuItem("Tools/Water Game/Create Character Showcase Demo")]
        public static void CreateDemo()
        {
            if (!EditorUtility.DisplayDialog("Create Water Game Showcase",
                "Create/open the showcase demo scene? The current scene will be replaced after a save prompt.", "Create", "Cancel"))
                return;
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            EnsureDirectory(Root + "/Demo");
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var root = new GameObject("Water Game Character Showcase");
            root.AddComponent<CharacterShowcase>();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, ScenePath);
            Selection.activeObject = root;
            Debug.Log("Water Game showcase demo created at " + ScenePath + ". Press Play to try it.");
        }

        private static void EnsureDirectory(string path)
        {
            var parts = path.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }
    }
}
#endif
