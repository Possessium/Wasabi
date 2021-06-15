using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WSB_Ban))]
public class CE_Ban : Editor
{
    bool customEditor = true;

    bool showEarth = false;
    bool showShrink = false;
    bool showLight = false;
    bool showWind = false;

    SerializedProperty spriteRend;
    SerializedProperty spells;
    SerializedProperty controllerValues;

    SerializedProperty grabFilter;
    SerializedProperty semiSolidFilter;
    SerializedProperty groundLayer;
    //SerializedProperty moveLayer;

    SerializedProperty maxEarthCharges;
    SerializedProperty maxWindCharges;
    SerializedProperty maxShrinkCharges;
    SerializedProperty maxLightCharges;

    SerializedProperty earthZone;
    SerializedProperty earthSize;
    //SerializedProperty earthDuration;
    SerializedProperty earthRechargeDelay;

    SerializedProperty light;
    //SerializedProperty lightDuration;
    SerializedProperty lightRechargeDelay;

    SerializedProperty shrinkRechargeDelay;

    SerializedProperty windRange;
    SerializedProperty windPower;
    SerializedProperty windRechargeDelay;

    private void OnEnable()
    {
        // Populate the serialized properties
        spriteRend = serializedObject.FindProperty("spriteRend");
        grabFilter = serializedObject.FindProperty("grabContactFilter");
        semiSolidFilter = serializedObject.FindProperty("semiSolidFilter");
        groundLayer = serializedObject.FindProperty("groundLayer");
        //moveLayer = serializedObject.FindProperty("moveLayer");
        controllerValues = serializedObject.FindProperty("controllerValues");
        spells = serializedObject.FindProperty("spells");
        maxEarthCharges = serializedObject.FindProperty("maxEarthCharges");
        maxWindCharges = serializedObject.FindProperty("maxWindCharges");
        maxShrinkCharges = serializedObject.FindProperty("maxShrinkCharges");
        maxLightCharges = serializedObject.FindProperty("maxLightCharges");
        earthZone = serializedObject.FindProperty("earthZone");
        earthSize = serializedObject.FindProperty("earthSize");
        //earthDuration = serializedObject.FindProperty("earthDuration");
        earthRechargeDelay = serializedObject.FindProperty("earthChargeDelay");
        light = serializedObject.FindProperty("lightObject");
        //lightDuration = serializedObject.FindProperty("lightDuration");
        lightRechargeDelay = serializedObject.FindProperty("lightChargeDelay");
        shrinkRechargeDelay = serializedObject.FindProperty("shrinkChargeDelay");
        windRange = serializedObject.FindProperty("windRange");
        windPower = serializedObject.FindProperty("windPower");
        windRechargeDelay = serializedObject.FindProperty("windChargeDelay");
    }

    public override void OnInspectorGUI()
    {
        // Toggle or not the custom editor
        customEditor = GUILayout.Toggle(customEditor, "Toggle Custom Editor");
        GUILayout.Space(25);
        if (!customEditor)
        {
            base.OnInspectorGUI();
            return;
        }

        serializedObject.Update();
        Undo.RecordObjects(serializedObject.targetObjects, "Ban Change");

        Components();

        SpellsValues();

        serializedObject.ApplyModifiedProperties();
    }

    void Components()
    {
        // Check if there is no missing components
        MissingCheck(spriteRend);
        MissingCheck(spells);
        MissingCheck(earthZone);
        MissingCheck(light);
        MissingCheck(controllerValues);
    }

    void ShowEarth()
    {
        Header("Earth parameters");
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(earthSize);
        //EditorGUILayout.PropertyField(earthDuration);
        EditorGUILayout.PropertyField(earthRechargeDelay);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        DrawLine(Color.grey);
        EditorGUILayout.Space();
    }

    void ShowLight()
    {
        Header("Light parameters");
        EditorGUILayout.Space();

        //EditorGUILayout.PropertyField(lightDuration);
        EditorGUILayout.PropertyField(lightRechargeDelay);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        DrawLine(Color.grey);
        EditorGUILayout.Space();
    }

    void ShowWind()
    {
        Header("Wind parameters");
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(windRange);
        EditorGUILayout.PropertyField(windPower);
        EditorGUILayout.PropertyField(windRechargeDelay);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        DrawLine(Color.grey);
        EditorGUILayout.Space();

    }

    void ShowShrink()
    {
        Header("Shrink parameters");
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(shrinkRechargeDelay);
    }

    void SpellsValues()
    {
        DrawLine(Color.grey);
        EditorGUILayout.Space();

        Header("Filters and Layers");
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(grabFilter, new GUIContent("Filter of objects to grab"));
        EditorGUILayout.PropertyField(semiSolidFilter, new GUIContent("Filter of semi solid plateforms"));
        EditorGUILayout.PropertyField(groundLayer, new GUIContent("Layer of the ground to spawn earth zone"));
        //EditorGUILayout.PropertyField(moveLayer, new GUIContent("Layer of the pushing/pulling objects"));
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        DrawLine(Color.grey);
        EditorGUILayout.Space();

        Header("Spell charges");
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(maxEarthCharges);
        EditorGUILayout.PropertyField(maxShrinkCharges);
        EditorGUILayout.PropertyField(maxLightCharges);
        EditorGUILayout.PropertyField(maxWindCharges);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        DrawLine(Color.grey);
        EditorGUILayout.Space();

        Header("Spell parameters");
        EditorGUILayout.Space();

        bool _showWind = showWind;
        bool _showLight = showLight;
        bool _showEarth = showEarth;
        bool _showShrink = showShrink;

        EditorGUILayout.BeginHorizontal();

        showShrink = EditorGUILayout.ToggleLeft("Shrink", showShrink);
        showEarth = EditorGUILayout.ToggleLeft("Earth", showEarth);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        showLight = EditorGUILayout.ToggleLeft("Light", showLight);
        showWind = EditorGUILayout.ToggleLeft("Wind", showWind);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (showShrink != _showShrink && showShrink)
            showEarth = showLight = showWind = false;
        else if (showWind != _showWind && showWind)
            showEarth = showLight = showShrink = false;
        else if (showLight != _showLight && showLight)
            showEarth = showShrink = showWind = false;
        else if (showEarth != _showEarth && showEarth)
            showShrink = showLight = showWind = false;

        DrawLine(Color.grey);
        EditorGUILayout.Space();

        if (showLight)
            ShowLight();

        if (showWind)
            ShowWind();

        if (showEarth)
            ShowEarth();

        if (showShrink)
            ShowShrink();

    }

    void MissingCheck(SerializedProperty _property)
    {
        // Exit if the component isn't missing
        if (_property.objectReferenceValue != null)
            return;

        // Show an error text and asks user to populate the missing component
        EditorGUILayout.HelpBox($"You need to set the value of {_property.name}", MessageType.Error, true);
        EditorGUILayout.PropertyField(_property);

    }
    void DrawLine(Color _c) => EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), _c);
    
    void Header(string _title)
    {
        GUIStyle bold = new GUIStyle(EditorStyles.label);
        bold.fontStyle = FontStyle.Bold;
        bold.fontSize = 12;
        bold.alignment = TextAnchor.MiddleCenter;

        GUIContent _content = new GUIContent(_title);

        Vector2 _size = bold.CalcSize(_content);

        EditorGUILayout.BeginHorizontal();

        Rect _rect = EditorGUILayout.GetControlRect();

        float _width = _rect.width / 2 - (_size.x / 2 + 10);

        EditorGUI.DrawRect(new Rect(_rect.x, _rect.y, _width, 2), Color.gray);
        EditorGUI.LabelField(new Rect(_rect.x, _rect.y - 10, _rect.width, _rect.height), _title, bold);
        EditorGUI.DrawRect(new Rect(_rect.xMax - _width, _rect.y, _width, 2), Color.gray);


        EditorGUILayout.EndHorizontal();
    }

}
