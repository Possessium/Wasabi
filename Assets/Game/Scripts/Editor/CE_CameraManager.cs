using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WSB_CameraManager))]
public class CE_CameraManager : Editor
{

    bool ceActive = true;

    //SerializedProperty isOrtho;
    SerializedProperty camMoveSpeed;
    SerializedProperty camZoomSpeed;
    SerializedProperty minCamZoom;
    SerializedProperty maxCamZoom;

    private void OnEnable()
    {
        // Populate serialized properties
        //isOrtho = serializedObject.FindProperty("isOrtho");
        camMoveSpeed = serializedObject.FindProperty("camMoveSpeed");
        camZoomSpeed = serializedObject.FindProperty("camZoomSpeed");
        minCamZoom = serializedObject.FindProperty("minCamZoom");
        maxCamZoom = serializedObject.FindProperty("maxCamZoom");
    }



    public override void OnInspectorGUI()
    {
        // Fetch all Camera Manager, throw an error if there is more than one in the scene
        WSB_CameraManager[] _foundManagers = FindObjectsOfType<WSB_CameraManager>();
        if(_foundManagers.Length > 1)
        {
            EditorGUILayout.HelpBox("There is more than one WSB_CameraManager in this scene.\nPlease delete unnecessary instances to have only one left.", MessageType.Error);
            EditorGUILayout.Space();
            for (int i = 0; i < _foundManagers.Length; i++)
            {
                EditorGUILayout.LabelField($"There is one instance on {_foundManagers[i].name}");
            }
            return;
        }


        // Toggle or not the custom editor
        ceActive = EditorGUILayout.ToggleLeft("Toggle custom editor", ceActive);

        if (!ceActive)
            base.OnInspectorGUI();

        else
        {
            serializedObject.Update();

            // Create GUIStyle for categories
            GUIStyle _style = new GUIStyle();
            _style.fontStyle = FontStyle.Bold;
            _style.fontSize = _style.fontSize + 15;
            _style.normal.textColor = Color.white;

            EditorGUILayout.LabelField("Parameters :", _style);
            EditorGUILayout.Space();

            // Decrease fontsize a bit
            _style.fontSize = _style.fontSize - 15;

            //EditorGUILayout.PropertyField(isOrtho);
            //EditorGUILayout.Space();
                        
            EditorGUILayout.LabelField("Camera speeds :", _style);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(camMoveSpeed);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(camZoomSpeed);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Camera zoom properties :", _style);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(minCamZoom);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(maxCamZoom);
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }

}
