using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace MinLibrary
{
    public class MinConvert
    {
        public static List<string> TextAssetToStringList(TextAsset textAsset, params char[] seperator)
        {
            return textAsset.ToString().Split(seperator).ToList();
        }

        public static T FindEnumByInt<T>(int index)
        {
            return (T)Enum.ToObject(typeof(T), index);
        }
    }
}
