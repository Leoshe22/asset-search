using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Linq;

namespace LS.Attributes 
{
    public class AssetSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        System.Type assetType;
        public SerializedProperty serializedProperty;

        public AssetSearchProvider Init(
            System.Type assetType, 
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

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {
            List<SearchTreeEntry> list = new List<SearchTreeEntry>();

            //string[] assetGuids = AssetDatabase.FindAssets($"t:{assetType.Name}");
            string[] assetGuids = AssetDatabase.FindAssets($"t:object", new string[] {"Assets"});
            List<string> paths = new List<string>();
            foreach(string guid in assetGuids)
                paths.Add(AssetDatabase.GUIDToAssetPath(guid));

            paths = paths.Where(x => PathIsCorrectType(x, assetType)).OrderBy(x => x).ToList();

            
            list.Add(new SearchTreeGroupEntry(new GUIContent("Select Variable")));
            List<string> groups = new List<string>();
            foreach(string item in paths) {
                string[] entryTitle = item.Split('/');
                string groupName = "";
                for (int i = 0; i < entryTitle.Length - 1; i++) {
                    groupName += entryTitle[i];
                    if (!groups.Contains(groupName))
                    {
                        list.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]), i+1));
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

            return list;
        }

        bool PathIsCorrectType(string path, System.Type type) {
            return AssetDatabase.LoadAssetAtPath(path,type) != null;
        }
    }
}