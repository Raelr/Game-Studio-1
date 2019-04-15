using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalExtensionMethods
{
    public static bool isNull (this GameObject obj)
    {
        return obj == null;
    }
    public static bool isNotNull(this GameObject obj)
    {
        return obj != null;
    }
   
}
