using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WSB_Dialogue))]
public class CE_WSB_Dialogue : Editor
{

    bool ceActive = true;

    SerializedProperty dialogues;
    SerializedObject dialogueObject;
    GUIStyle deleteStyle;

    Color defaultColor;
    TextEditor lastTextSelected = null;


    private void OnEnable()
    {
        // Get the inspected list of dialogues
        dialogues = serializedObject.FindProperty("Dialogues");
    }

    public override void OnInspectorGUI()
    {

        // Toggle or not the custom editor
        ceActive = EditorGUILayout.ToggleLeft("Toggle custom editor ", ceActive);
        if (!ceActive)
        {
            base.OnInspectorGUI();
            return;
        }

        serializedObject.Update();
        Undo.RecordObjects(serializedObject.targetObjects, "Dialog Change");

        // Setup the GUIStyle of the delete buttons
        deleteStyle = new GUIStyle(EditorStyles.miniButton);
        deleteStyle.fontStyle = FontStyle.Bold;
        deleteStyle.normal.textColor = Color.white;

        // Stock the default color of the background
        defaultColor = GUI.backgroundColor;

        EditorGUILayout.Space();

        // Button to add new dialogue
        if (GUILayout.Button("Add new dialogue", GUILayout.MaxWidth(200)))
        {
            // Create a new dialogue in the list
            dialogues.InsertArrayElementAtIndex(dialogues.arraySize);

            // Set to null if not null
            if (dialogues.GetArrayElementAtIndex(dialogues.arraySize - 1).objectReferenceValue != null)
                dialogues.DeleteArrayElementAtIndex(dialogues.arraySize - 1);
        }

        EditorGUILayout.Space();

        ShowDialogues();

        EditorGUILayout.Space();

        DrawLine(new Color(.7f, .7f, .7f, 1));

        serializedObject.ApplyModifiedProperties();

    }

    void ShowDialogues()
    {
        // Loop through the entire dialogues list
        for (int i = 0; i < dialogues.arraySize; i++)
        {
            DrawLine(new Color(.7f, .7f, .7f, 1));

            EditorGUILayout.Space();

            // Get the current dialogue
            SerializedProperty _dialogue = dialogues.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginHorizontal();

            GUI.backgroundColor = new Color(2, 0, 0, 1);

            // Button to delete the current dialogue
            if (GUILayout.Button("Delete this dialogue", deleteStyle, GUILayout.Width(150)))
            {
                // Set to null if not null
                if (_dialogue.objectReferenceValue != null)
                    dialogues.DeleteArrayElementAtIndex(i);

                // Remove the current dialogue
                dialogues.DeleteArrayElementAtIndex(i);
                // break to avoid nullRef
                break;
            }

            GUI.backgroundColor = defaultColor;

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_dialogue, GUIContent.none, GUILayout.MaxWidth(200));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            // Stop if the current dialogue is null
            if (_dialogue.objectReferenceValue == null)
                continue;

            // Get the property of the current dialogue object
            dialogueObject = new SerializedObject(_dialogue.objectReferenceValue);

            dialogueObject.Update();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(dialogueObject.FindProperty("IsImageRight"));
            EditorGUILayout.PropertyField(dialogueObject.FindProperty("Image"), GUIContent.none, GUILayout.MaxWidth(200));

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(dialogueObject.FindProperty("Character"), GUIContent.none, GUILayout.Width(100));
            EditorGUILayout.Space();

            SerializedProperty _showText = dialogueObject.FindProperty("ShowTexts");

            _showText.boolValue = EditorGUILayout.Foldout(_showText.boolValue, "Show texts", true);

            if (_showText.boolValue)
            {
                EditorGUILayout.Space();

                SerializedProperty _text = dialogueObject.FindProperty("Texts");
                SerializedProperty _showTextPreview = dialogueObject.FindProperty("ShowPreview");

                if(_showTextPreview.arraySize != _text.arraySize)
                {
                    for (int j = 0; j < _text.arraySize; j++)
                    {
                        if (_showTextPreview.arraySize > j) 
                            continue;
                        else
                        {
                            _showTextPreview.InsertArrayElementAtIndex(j);
                            _showTextPreview.GetArrayElementAtIndex(j).boolValue = false;
                        }
                    }
                }

                // Button to create a new text
                if (GUILayout.Button("Add new text"))
                {
                    // Create a new entry in the text & showTextPreview lists on the dialogueObject and set default values
                    _text.InsertArrayElementAtIndex(_text.arraySize);
                    _showTextPreview.GetArrayElementAtIndex(_text.arraySize - 1).boolValue = false;

                    _showTextPreview.InsertArrayElementAtIndex(_text.arraySize-1);
                    _text.GetArrayElementAtIndex(_text.arraySize - 1).stringValue = "";
                }

                EditorGUILayout.Space();

                ShowText(_text);

            }

            EditorGUILayout.Space();

            dialogueObject.ApplyModifiedProperties();

        }

    }

    void ShowText(SerializedProperty _text)
    {
        // Loop through the text list
        for (int j = 0; j < _text.arraySize; j++)
        {
            Rect _rect = EditorGUILayout.GetControlRect();

            EditorGUI.DrawRect(new Rect(_rect.x + _rect.width / 4, _rect.y, _rect.width / 2, 2), Color.gray);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Default font size : ");

            EditorGUILayout.PropertyField(dialogueObject.FindProperty("DefaultSize"), GUIContent.none, GUILayout.Width(100));
            EditorGUILayout.Space();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // Use GUILayout.TextArea to have it stretch and squish based on the text size
            _text.GetArrayElementAtIndex(j).stringValue = GUILayout.TextArea(_text.GetArrayElementAtIndex(j).stringValue);

            EditorGUILayout.Space();

            RichTextButtons(_text, j);

            dialogueObject.FindProperty("ShowPreview").GetArrayElementAtIndex(j).boolValue = EditorGUILayout.Foldout(dialogueObject.FindProperty("ShowPreview").GetArrayElementAtIndex(j).boolValue, "Toggle preview", true);

            EditorGUILayout.Space();

            if (dialogueObject.FindProperty("ShowPreview").GetArrayElementAtIndex(j).boolValue)
            {

                GUIStyle _richTextStyle = new GUIStyle(GUI.skin.label);
                _richTextStyle.richText = true;

                EditorGUILayout.TextArea(_text.GetArrayElementAtIndex(j).stringValue, _richTextStyle);

            }


            EditorGUILayout.Space();

            GUI.backgroundColor = new Color(2, 0, 0, 1);

            if (GUILayout.Button("Delete this text", deleteStyle, GUILayout.Width(150)))
            {
                _text.DeleteArrayElementAtIndex(j);
                break;
            }

            GUI.backgroundColor = defaultColor;

            EditorGUILayout.Space();
        }
    }

    void RichTextButtons(SerializedProperty _text, int _index)
    {
        EditorGUILayout.BeginHorizontal();

        lastTextSelected = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);

        // Adds <b> and </b> with the selected text between
        if (GUILayout.Button("Bold"))
        {
            if (!string.IsNullOrEmpty(lastTextSelected.text) && lastTextSelected.hasSelection)
            {
                lastTextSelected.ReplaceSelection($"<b>{lastTextSelected.SelectedText}</b>");
                _text.GetArrayElementAtIndex(_index).stringValue = lastTextSelected.text;
            }
        }

        // Adds <u> and </u> with the selected text between
        if (GUILayout.Button("Underline"))
        {
            if (!string.IsNullOrEmpty(lastTextSelected.text) && lastTextSelected.hasSelection)
            {
                lastTextSelected.ReplaceSelection($"<u>{lastTextSelected.SelectedText}</u>");
                _text.GetArrayElementAtIndex(_index).stringValue = lastTextSelected.text;
            }
        }

        // Adds <i> and </i> with the selected text between
        if (GUILayout.Button("Italic"))
        {
            if (!string.IsNullOrEmpty(lastTextSelected.text) && lastTextSelected.hasSelection)
            {
                lastTextSelected.ReplaceSelection($"<i>{lastTextSelected.SelectedText}</i>");
                _text.GetArrayElementAtIndex(_index).stringValue = lastTextSelected.text;
            }
        }

        if (GUILayout.Button("Color"))
        {
            ColorPopup.Init(this, _text, _index);
        }

        if(GUILayout.Button("Size"))
        {
            SizePopup.Init(this, _text, _index);
        }

        EditorGUILayout.EndHorizontal();
    }
    public void ApplyFontSizeRichText(int _s, SerializedProperty _text, int _index)
    {
        if (!string.IsNullOrEmpty(lastTextSelected.text) && lastTextSelected.hasSelection)
        {
            serializedObject.Update();
            dialogueObject.Update();
            Undo.RecordObjects(serializedObject.targetObjects, "Main object");
            Undo.RecordObjects(dialogueObject.targetObjects, "Dialog object");
            Undo.RecordObjects(_text.serializedObject.targetObjects, "Text object");

            lastTextSelected.ReplaceSelection($"<size={_s}>{lastTextSelected.SelectedText}</size>");

            _text.GetArrayElementAtIndex(_index).stringValue = lastTextSelected.text;

            _text.serializedObject.ApplyModifiedProperties();
            dialogueObject.ApplyModifiedProperties();
            serializedObject.ApplyModifiedProperties();
        }
    }

    public void ApplyColorRichText(Color _c, SerializedProperty _text, int _index)
    {
        if (!string.IsNullOrEmpty(lastTextSelected.text) && lastTextSelected.hasSelection)
        {
            serializedObject.Update();
            dialogueObject.Update();
            Undo.RecordObjects(serializedObject.targetObjects, "Main object");
            Undo.RecordObjects(dialogueObject.targetObjects, "Dialog object");
            Undo.RecordObjects(_text.serializedObject.targetObjects, "Text object");

            lastTextSelected.ReplaceSelection($"<color=#{ColorUtility.ToHtmlStringRGBA(_c)}>{lastTextSelected.SelectedText}</color>");

            _text.GetArrayElementAtIndex(_index).stringValue = lastTextSelected.text;

            Debug.Log(_text.GetArrayElementAtIndex(_index).stringValue);

            _text.serializedObject.ApplyModifiedProperties();
            dialogueObject.ApplyModifiedProperties();
            serializedObject.ApplyModifiedProperties();
        }
    }

    void DrawLine(Color _c) => EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), _c);
}
public class SizePopup : EditorWindow
{
    int textSize = 36;
    static SerializedProperty property = null;
    static int index = 0;
    static CE_WSB_Dialogue ce = null;

    static SizePopup popup = null;

    public static void Init(CE_WSB_Dialogue _ce, SerializedProperty _text, int _index)
    {
        if (popup) popup.Close();
        property = _text;
        index = _index;
        ce = _ce;
        popup = ScriptableObject.CreateInstance<SizePopup>();
        Vector2 _pos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        popup.position = new Rect(_pos.x, _pos.y, 100, 75);
        popup.ShowPopup();
    }

    private void OnGUI()
    {
        textSize = EditorGUILayout.IntField(textSize);
        GUILayout.Space(5);
        if (GUILayout.Button("Apply"))
        {
            ce.ApplyFontSizeRichText(textSize, property, index);
            popup.Close();
        }
        if (GUILayout.Button("Close"))
            popup.Close();
    }
}

public class ColorPopup : EditorWindow
{
    Color textColor = Color.white;
    static SerializedProperty property = null;
    static int index = 0;
    static CE_WSB_Dialogue ce = null;

    static ColorPopup popup = null;

    public static void Init(CE_WSB_Dialogue _ce, SerializedProperty _text, int _index)
    {
        if (popup) popup.Close();
        property = _text;
        index = _index;
        ce = _ce;
        popup = ScriptableObject.CreateInstance<ColorPopup>();
        Vector2 _pos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        popup.position = new Rect(_pos.x, _pos.y, 100, 75);
        popup.ShowPopup();
    }

    private void OnGUI()
    {
        textColor = EditorGUILayout.ColorField(textColor);
        GUILayout.Space(5);
        if (GUILayout.Button("Apply"))
        {
            ce.ApplyColorRichText(textColor, property, index);
            Close();
        }
        if (GUILayout.Button("Close"))
            Close();
    }
}
