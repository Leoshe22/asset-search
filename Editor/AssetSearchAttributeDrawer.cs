using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Experimental.GraphView;
using System.Reflection;

namespace LS.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(AssetSearch))]
    public class AssetSearchAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
        {
            position.width -= 60;
            var fieldType = this.fieldInfo.FieldType;

            EditorGUI.ObjectField(position, label, property.objectReferenceValue, fieldType, true);

            position.x += position.width;
            position.width = 60;

            if (GUI.Button(position, new GUIContent("Search")))
            {
                Type t = GetType(
                    this.attribute as AssetSearch, 
                    fieldType, 
                    property.serializedObject.targetObject
                );
                SearchWindow.Open(
                    new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                    ScriptableObject.CreateInstance<AssetSearchProvider>().Init(t, property)
                );
            }
        }

        public static Type GetType(AssetSearch attribute, Type fieldType, object targetObject) 
        {
            if (attribute.optionalType != null)
                return attribute.optionalType;
            if (attribute.typePropertyName != null)
            {
                var typeProperty = GetValue(targetObject, attribute.typePropertyName);
                if (typeProperty != null)
                    return (Type)typeProperty;
            }
            return fieldType;
        }

        //Pulled from:
        //https://answers.unity.com/questions/425012/get-the-instance-the-serializedproperty-belongs-to.html
        public static object GetValue(object source, string name)
        {
            if(source == null)
                return null;
            var type = source.GetType();
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if(f == null)
            {
                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if(p == null)
                    return null;
                return p.GetValue(source, null);
            }
            return f.GetValue(source);
        }
    }
}