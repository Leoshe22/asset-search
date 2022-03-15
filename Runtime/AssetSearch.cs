using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LS.Attributes
{
    public class AssetSearch : PropertyAttribute
    {
        public Type optionalType;
        public AssetSearch(Type type = null)
        {
            optionalType = type;
        }
    }
}