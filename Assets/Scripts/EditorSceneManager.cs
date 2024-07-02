using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class EditorExtensions
{
    [MenuItem("Scenes/Menu Scene", false, 1)]
    public static void LoadMenu()
    {
        var sceneName = "Main Menu";
        OpenScene(sceneName);
    }

    [MenuItem("Scenes/Game Scene", false, 1)]
    public static void LoadGameScene()
    {
        var sceneName = "Game Scene";
        OpenScene(sceneName);
    }

    static void OpenScene(string sceneName)
    {
        if (Application.isPlaying)
            SceneManager.LoadScene(sceneName);
        else
            EditorSceneManager.OpenScene(
                AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"{sceneName} t:scene")[0]));
    }

}