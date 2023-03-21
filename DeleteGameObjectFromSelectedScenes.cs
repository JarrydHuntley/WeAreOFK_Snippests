using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


public class DeleteGameObjectFromSelectedScenes : EditorWindow
{
    [MenuItem("Delete Game Object From Selected Scenes", false, 100)]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        DeleteGameObjectFromSelectedScenes window = (DeleteGameObjectFromSelectedScenes)EditorWindow.GetWindow(typeof(DeleteGameObjectFromSelectedScenes));
        window.Show();
    }

    private bool deleteRootLevelGameObjectsOnly = true;
    private string gameObjectToDelete= "";

    void OnGUI()
    {
        EditorUtility.ClearProgressBar();
        GUILayout.Label("Game Object to Delete(case sensitive)");
        gameObjectToDelete = GUILayout.TextField(gameObjectToDelete);
        deleteRootLevelGameObjectsOnly = GUILayout.Toggle(deleteRootLevelGameObjectsOnly, "Root Level Game Objects Only");

        Color oldColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.red;

        if (GUILayout.Button("Delete GameObject in Selected Scenes!", GUILayout.MinHeight(34)))
        {
            DeleteThings();
        }

        GUI.backgroundColor = oldColor;
    }

    private void DeleteThings()
    {
        for (int i = 0; i < Selection.objects.Length; ++i)
        {
            Object obj = Selection.objects[i];

            string filePath = AssetDatabase.GetAssetPath(obj);
            if (filePath.EndsWith(".unity"))
            {
                if (EditorSceneManager.GetActiveScene().name != obj.name)
                    EditorSceneManager.OpenScene(filePath);

                GameObject deleteMe = GameObject.Find(gameObjectToDelete.Trim());


                float progress = (float)i / (float)Selection.objects.Length;
                EditorUtility.DisplayProgressBar("Deleting Game Objects files", "Searching and Deleting in " + obj.name, progress);

                //foreach (GameObject item in deleteMe)
                //{
                    if (deleteMe != null && IsRootLevelAndDelete(deleteMe))
                    {
                        DestroyImmediate(deleteMe);
                        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                        Debug.Log("Deleted " + gameObjectToDelete);
                    }
                //}
            }
        }
    }


    bool IsRootLevelAndDelete(GameObject gameObject)
    {
        return deleteRootLevelGameObjectsOnly ? gameObject.transform.parent == null : true;
    }
}

