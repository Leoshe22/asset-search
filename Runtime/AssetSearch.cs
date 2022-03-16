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
        public AssetSearch()
        {

        }

        public AssetSearch(string typePropertyName) 
        {
            this.typePropertyName = typePropertyName;
        }

        public AssetSearch(Type type)
        {
            optionalType = type;
        }
    }
}