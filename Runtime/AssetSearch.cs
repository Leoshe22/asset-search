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
        public AssetSearch(string label = "Search", int buttonWidth = 60)
        {
            this.label = label;
            this.buttonWidth = buttonWidth;
        }

        public AssetSearch(string typePropertyName, string label = "Search", int buttonWidth = 60) 
        {
            this.typePropertyName = typePropertyName;
            this.label = label;
            this.buttonWidth = buttonWidth;
        }

        public AssetSearch(Type type, string label = "Search", int buttonWidth = 60)
        {
            optionalType = type;
            this.label = label;
            this.buttonWidth = buttonWidth;
        }
    }
}