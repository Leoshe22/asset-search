using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LS.Attributes
{
    public class AssetSearch : PropertyAttribute
    {
        public Type optionalType;
        public string typePropertyName;
        public string label;
        public int buttonWidth;
        public bool shortenPaths;
        public AssetSearch(string label = "Search", int buttonWidth = 60, bool shortenPaths = true)
        {
            this.label = label;
            this.buttonWidth = buttonWidth;
            this.shortenPaths = shortenPaths;
        }

        public AssetSearch(string typePropertyName, string label = "Search", int buttonWidth = 60, bool shortenPaths = true) 
        {
            this.typePropertyName = typePropertyName;
            this.label = label;
            this.buttonWidth = buttonWidth;
            this.shortenPaths = shortenPaths;
        }

        public AssetSearch(Type type, string label = "Search", int buttonWidth = 60, bool shortenPaths = true)
        {
            optionalType = type;
            this.label = label;
            this.buttonWidth = buttonWidth;
            this.shortenPaths = shortenPaths;
        }
    }
}