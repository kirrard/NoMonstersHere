using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesCache
{
    private static Dictionary<string, Object> _cache = new Dictionary<string, Object>();

    public static void Load(string folder)
    {
        Object[] temp = Resources.LoadAll("Images/" + folder);
        
        for(int i = 0; i<temp.Length; i++)
        {
            Object tempObj = temp[i];
            _cache[tempObj.name] = tempObj;
        }
    }

    public static Object GetObject(string key)
    {
        return _cache[key];
    }
}
