using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WSB_Lux))]
public class CE_Lux : Editor
{
    //bool customEditor = true;

    //SerializedProperty spriteRend;
    //SerializedProperty controllerValues;
    //SerializedProperty spells;
    //SerializedProperty render;

    //SerializedProperty grabFilter;
    //SerializedProperty semiSolidFilter;
    //SerializedProperty potLayer;
    //SerializedProperty shrinkLayer;

    //SerializedProperty maxTrampolineCharges;
    //SerializedProperty maxLadderCharges;
    //SerializedProperty maxBridgeCharges;
    //SerializedProperty maxCarnivoreCharges;

    //SerializedProperty shrinkSpeed;
    ////SerializedProperty shrinkDuration;

    //private void OnEnable()
    //{
    //    // Populate the serialized properties
    //    spriteRend = serializedObject.FindProperty("spriteRend");
    //    grabFilter = serializedObject.FindProperty("grabContactFilter");
    //    semiSolidFilter = serializedObject.FindProperty("semiSolidFilter");
    //    potLayer = serializedObject.FindProperty("potLayer");
    //    shrinkLayer = serializedObject.FindProperty("shrinkLayer");
    //    controllerValues = serializedObject.FindProperty("controllerValues");
    //    spells = serializedObject.FindProperty("spells");
    //    maxTrampolineCharges = serializedObject.FindProperty("maxTrampolineCharges");
    //    maxLadderCharges = serializedObject.FindProperty("maxLadderCharges");
    //    maxBridgeCharges = serializedObject.FindProperty("maxBridgeCharges");
    //    maxCarnivoreCharges = serializedObject.FindProperty("maxCarnivoreCharges");
    //    shrinkSpeed = serializedObject.FindProperty("shrinkSpeed");
    //    //shrinkDuration = serializedObject.FindProperty("shrinkDuration");
    //    render = serializedObject.FindProperty("render");
    //}

    //public override void OnInspectorGUI()
    //{
    //    // Toggle or not the custom editor
    //    customEditor = GUILayout.Toggle(customEditor, "Toggle Custom Editor");
    //    GUILayout.Space(25);
    //    if (!customEditor)
    //    {
    //        base.OnInspectorGUI();
    //        return;
    //    }

    //    serializedObject.Update();
    //    Undo.RecordObjects(serializedObject.targetObjects, "Ban Change");

    //    Components();

    //    SpellsValues();

    //    serializedObject.ApplyModifiedProperties();
    //}

    //void Components()
    //{
    //    // Check if there is no missing components
    //    MissingCheck(spriteRend);
    //    MissingCheck(spells);
    //    MissingCheck(render);
    //    MissingCheck(controllerValues);
    //}


    //void SpellsValues()
    //{
    //    DrawLine(Color.grey);
    //    EditorGUILayout.Space();

    //    Header("Filters and Layers");
    //    EditorGUILayout.Space();

    //    EditorGUILayout.PropertyField(grabFilter, new GUIContent("Filter of objects to grab"));
    //    EditorGUILayout.PropertyField(semiSolidFilter, new GUIContent("Filter of semi solid plateforms"));
    //    EditorGUILayout.PropertyField(potLayer, new GUIContent("Layer of the pots"));
    //    EditorGUILayout.PropertyField(shrinkLayer, new GUIContent("Layer of the objects the blocks unshrink"));
    //    EditorGUILayout.Space();
    //    EditorGUILayout.Space();

    //    DrawLine(Color.grey);
    //    EditorGUILayout.Space();

    //    Header("Spell charges");
    //    EditorGUILayout.Space();

    //    EditorGUILayout.PropertyField(maxBridgeCharges);
    //    EditorGUILayout.PropertyField(maxCarnivoreCharges);
    //    EditorGUILayout.PropertyField(maxLadderCharges);
    //    EditorGUILayout.PropertyField(maxTrampolineCharges);
    //    EditorGUILayout.Space();
    //    EditorGUILayout.Space();

    //    DrawLine(Color.grey);
    //    EditorGUILayout.Space();

    //    Header("Shrink parameters");
    //    EditorGUILayout.Space();

    //    //EditorGUILayout.PropertyField(shrinkDuration);
    //    EditorGUILayout.PropertyField(shrinkSpeed);
    //}

    //void MissingCheck(SerializedProperty _property)
    //{
    //    // Exit if the component isn't missing
    //    if (_property.objectReferenceValue != null)
    //        return;

    //    // Show an error text and asks user to populate the missing component
    //    EditorGUILayout.HelpBox($"You need to set the value of {_property.name}", MessageType.Error, true);
    //    EditorGUILayout.PropertyField(_property);

    //}
    //void DrawLine(Color _c) => EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), _c);

    //void Header(string _title)
    //{
    //    GUIStyle bold = new GUIStyle(EditorStyles.label);
    //    bold.fontStyle = FontStyle.Bold;
    //    bold.fontSize = 12;
    //    bold.alignment = TextAnchor.MiddleCenter;

    //    GUIContent _content = new GUIContent(_title);

    //    Vector2 _size = bold.CalcSize(_content);

    //    EditorGUILayout.BeginHorizontal();

    //    Rect _rect = EditorGUILayout.GetControlRect();

    //    float _width = _rect.width / 2 - (_size.x / 2 + 10);

    //    EditorGUI.DrawRect(new Rect(_rect.x, _rect.y, _width, 2), Color.gray);
    //    EditorGUI.LabelField(new Rect(_rect.x, _rect.y - 10, _rect.width, _rect.height), _title, bold);
    //    EditorGUI.DrawRect(new Rect(_rect.xMax - _width, _rect.y, _width, 2), Color.gray);


    //    EditorGUILayout.EndHorizontal();
    //}
}
