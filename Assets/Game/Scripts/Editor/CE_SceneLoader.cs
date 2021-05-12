using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(WSB_SceneLoader))]
public class CE_SceneLoader : Editor
{
    WSB_SceneLoader tLoader;

    bool editorOpen = true;
    bool showLoad = false;
    bool showUnload = false;

    public override void OnInspectorGUI()
    {
        editorOpen = EditorGUILayout.Foldout(editorOpen, (editorOpen ? "Close" : "Open") + " custom editor", true);

        EditorGUILayout.Space();

        if (!editorOpen)
            base.OnInspectorGUI();

        else
        {
            serializedObject.Update();
            Undo.RecordObjects(serializedObject.targetObjects, "SceneLoader change");
            tLoader = (WSB_SceneLoader)target;
            CustomEditor();
        }
    }

    SceneAsset buffer = null;

    void CustomEditor()
    {
        EditorGUI.BeginChangeCheck();
        buffer = null;
        tLoader.LoadOnPlay = EditorGUILayout.Toggle("Are scenes loading on play : ", tLoader.LoadOnPlay);

        EditorGUILayout.Space();

        if (!tLoader.Trigger && !tLoader.LoadOnPlay)
        {
            EditorGUILayout.HelpBox("You need to specify the Collider2D", MessageType.Error);
            tLoader.Trigger = (Collider2D)EditorGUILayout.ObjectField(tLoader.Trigger, typeof(Collider2D), false);
            EditorGUILayout.Space();
        }

        if (EditorGUI.EndChangeCheck())
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

        string[] _guids = AssetDatabase.FindAssets("t:scene");

        string[] _paths = new string[_guids.Length];

        for (int i = 0; i < _guids.Length; i++)
        {
            _paths[i] = AssetDatabase.GUIDToAssetPath(_guids[i]);
        }

        EditorGUILayout.Space();

        showLoad = EditorGUILayout.Foldout(showLoad, "Load scenes", true);
        if(showLoad)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("List of scenes to load :", EditorStyles.boldLabel);
            tLoader.LoadListSize = EditorGUILayout.IntField(tLoader.LoadListSize);
            EditorGUILayout.EndHorizontal();

            if (tLoader.AllScenesToLoadInOrder.Count > tLoader.LoadListSize)
                tLoader.AllScenesToLoadInOrder.RemoveRange(tLoader.LoadListSize, tLoader.AllScenesToLoadInOrder.Count - tLoader.LoadListSize);

            if (tLoader.AllScenesToLoadInOrder.Count < tLoader.LoadListSize)
            {
                for (int i = 0; i < tLoader.LoadListSize - tLoader.AllScenesToLoadInOrder.Count; i++)
                {
                    tLoader.AllScenesToLoadInOrder.Add(new ScenePicker());
                }
            }


            buffer = null;

            for (int i = 0; i < tLoader.AllScenesToLoadInOrder.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                if (tLoader.AllScenesToLoadInOrder[i].SceneName == string.Empty)
                {
                    buffer = (SceneAsset)EditorGUILayout.ObjectField(buffer, typeof(SceneAsset), false);
                    if (buffer)
                    {
                        string _path = System.Array.Find(_paths, p => p.Contains(buffer.name));
                        tLoader.AllScenesToLoadInOrder[i] = new ScenePicker(_path, buffer.name);
                    }
                }
                else
                {
                    buffer = AssetDatabase.LoadAssetAtPath(tLoader.AllScenesToLoadInOrder[i].ScenePath, typeof(SceneAsset)) as SceneAsset;
                    buffer = (SceneAsset)EditorGUILayout.ObjectField(buffer, typeof(SceneAsset), false);
                    string _path = System.Array.Find(_paths, p => p.Contains(buffer.name));
                    tLoader.AllScenesToLoadInOrder[i] = new ScenePicker(_path, buffer.name);
                }
                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }


        showUnload = EditorGUILayout.Foldout(showUnload, "Unload scenes", true);
        if (showUnload)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("List of scenes to unload :", EditorStyles.boldLabel);
            tLoader.UnloadListSize = EditorGUILayout.IntField(tLoader.UnloadListSize);
            EditorGUILayout.EndHorizontal();

            if (tLoader.AllScenesToUnloadInOrder.Count > tLoader.UnloadListSize)
                tLoader.AllScenesToUnloadInOrder.RemoveRange(tLoader.UnloadListSize, tLoader.AllScenesToUnloadInOrder.Count - tLoader.UnloadListSize);

            if (tLoader.AllScenesToUnloadInOrder.Count < tLoader.UnloadListSize)
            {
                for (int i = 0; i < tLoader.UnloadListSize - tLoader.AllScenesToUnloadInOrder.Count; i++)
                {
                    tLoader.AllScenesToUnloadInOrder.Add(new ScenePicker());
                }
            }


            buffer = null;

            for (int i = 0; i < tLoader.AllScenesToUnloadInOrder.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                if (tLoader.AllScenesToUnloadInOrder[i].SceneName == string.Empty)
                {
                    buffer = (SceneAsset)EditorGUILayout.ObjectField(buffer, typeof(SceneAsset), false);
                    if (buffer)
                    {
                        string _path = System.Array.Find(_paths, p => p.Contains(buffer.name));
                        tLoader.AllScenesToUnloadInOrder[i] = new ScenePicker(_path, buffer.name);
                    }
                }
                else
                {
                    buffer = AssetDatabase.LoadAssetAtPath(tLoader.AllScenesToUnloadInOrder[i].ScenePath, typeof(SceneAsset)) as SceneAsset;
                    buffer = (SceneAsset)EditorGUILayout.ObjectField(buffer, typeof(SceneAsset), false);
                    string _path = System.Array.Find(_paths, p => p.Contains(buffer.name));
                    tLoader.AllScenesToUnloadInOrder[i] = new ScenePicker(_path, buffer.name);
                }
                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        serializedObject.ApplyModifiedProperties();
    }
}
