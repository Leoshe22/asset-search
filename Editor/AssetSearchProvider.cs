using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System;

namespace LS.Attributes 
{
    public class AssetSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        Type assetType;
        public SerializedProperty serializedProperty;

        public AssetSearchProvider Init(
            Type assetType, 
            SerializedProperty serializedProperty
        ) {
            this.assetType = assetType;
            this.serializedProperty = serializedProperty;
            return this;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context) {
            serializedProperty.objectReferenceValue = (UnityEngine.Object)searchTreeEntry.userData;
            serializedProperty.serializedObject.ApplyModifiedProperties();
            return true;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> list = new List<SearchTreeEntry>();

            List<string> paths = GetPathsOfAssetsWithType(assetType);

            PopulateSearchTree(list, paths);

            return list;
        }

        private static void PopulateSearchTree(List<SearchTreeEntry> list, List<string> paths)
        {
            //First element in List is the title of the search window
            list.Add(new SearchTreeGroupEntry(new GUIContent("Select Asset")));

            List<string> groups = new List<string>();
            foreach (string item in paths)
            {
                string[] entryTitle = item.Split('/');
                string groupName = "";
                for (int i = 0; i < entryTitle.Length - 1; i++)
                {
                    groupName += entryTitle[i];
                    if (!groups.Contains(groupName))
                    {
                        list.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]), i + 1));
                        groups.Add(groupName);
                    }
                    groupName += "/";
                }

                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(item);
                SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(entryTitle.Last(), EditorGUIUtility.ObjectContent(obj, obj.GetType()).image));
                entry.level = entryTitle.Length;
                entry.userData = obj;
                list.Add(entry);
            }
        }

        private static string[] GetAllAssets() => AssetDatabase.FindAssets($"t:object", new string[] { "Assets" });

        private static List<string> GetPathsOfAssetsWithType(Type assetType) {
            string[] assetGuids = GetAllAssets();
            
            List<string> paths = new List<string>();
            foreach (string guid in assetGuids)
                paths.Add(AssetDatabase.GUIDToAssetPath(guid));

            paths = paths.Where(x => PathIsCorrectType(x, assetType)).OrderBy(x => x).ToList();
            return paths;
        }
        

        private static bool PathIsCorrectType(string path, Type type) {
            return AssetDatabase.LoadAssetAtPath(path,type) != null;
        }
    }
}