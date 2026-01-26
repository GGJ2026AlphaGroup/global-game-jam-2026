using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

#endif

[AttributeUsage(AttributeTargets.Field)]
public class SceneDropdownAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneDropdownAttribute))]
public class SceneDropdownDrawer : PropertyDrawer
{
    private string[] cachedScenes;

    private void CacheScenes()
    {
        if (cachedScenes != null)
            return;

        List<string> scenes = new List<string>();

        // Get all public const strings from SceneNames
        var fields = typeof(SceneNames).GetFields(
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.FlattenHierarchy);

        foreach (var field in fields)
        {
            if (field.FieldType == typeof(string) && field.IsLiteral)
            {
                string value = field.GetValue(null) as string;
                scenes.Add(value);
            }
        }

        cachedScenes = scenes.ToArray();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CacheScenes();

        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "Use [SceneDropdown] with string fields only");
            return;
        }

        if (cachedScenes.Length == 0)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        int index = Mathf.Max(0, Array.IndexOf(cachedScenes, property.stringValue));

        EditorGUI.BeginProperty(position, label, property);

        int newIndex = EditorGUI.Popup(position, label.text, index, cachedScenes);

        if (newIndex >= 0 && newIndex < cachedScenes.Length)
        {
            property.stringValue = cachedScenes[newIndex];
        }

        EditorGUI.EndProperty();
    }
}
#endif