﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalExtensionMethods
{
    public static bool isNull (this GameObject obj)
    {
        return obj == null;
    }

    public static void Hide(this GameObject obj)
    {
        obj.SetActive(false);
    }

    public static void Show(this GameObject obj)
    {
        obj.SetActive(true);
    }
}
