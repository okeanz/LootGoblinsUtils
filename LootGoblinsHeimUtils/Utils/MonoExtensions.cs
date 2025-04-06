using System;
using System.Reflection;
using UnityEngine;

namespace LootGoblinsUtils.Utils;

public static class MonoExtensions
{
    public static bool IsPiecePlaced(this MonoBehaviour script)
    {
        return false;
    }
    
    public static void CopyFields(object originalObject, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
    {
        foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
        {
            var originalFieldValue = fieldInfo.GetValue(originalObject);
            fieldInfo.SetValue(cloneObject, originalFieldValue);
        }
    }
}