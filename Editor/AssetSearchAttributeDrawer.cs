using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Experimental.GraphView;

namespace LS.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(AssetSearch))]
    public class AssetSearchAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            position.width -= 60;
            var fieldType = this.fieldInfo.FieldType;

            EditorGUI.ObjectField(position, label, property.objectReferenceValue, fieldType, true);

            position.x += position.width;
            position.width = 60;
            if (GUI.Button(position, new GUIContent("Find")))
            {
                var attrib = this.attribute as AssetSearch;
                Type t = attrib.optionalType ?? fieldType;
                SearchWindow.Open(
                    new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                    ScriptableObject.CreateInstance<AssetSearchProvider>().Init(t, property)
                );
            }
        }
    }
}