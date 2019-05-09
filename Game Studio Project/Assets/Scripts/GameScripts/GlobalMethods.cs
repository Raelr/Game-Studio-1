﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalMethods {

	public static void Hide(this GameObject obj) {
		obj.SetActive(false);
	}

	public static void Show(this GameObject obj) {
		obj.SetActive(true);
	}

	/// <summary>
	/// Gets the direction of point A to B.
	/// </summary>
	/// <returns>The direction of two points</returns>
	public static Vector2 GetDirection(Vector2 a, Vector2 b) {
		return (b - a).normalized;
	}

	/// <summary>
	/// Returns true if value is within bounds.
	/// </summary>
	/// <returns><c>true</c>, if is within bounds, <c>false</c> otherwise.</returns>
	public static bool WithinBounds(float value, float min, float max) {
		return value > min && value < max;
	}

	/// <summary>
	/// Returns the vector at a value of 1, retaining sign.
	/// </summary>
	/// <returns>The normalised direction.</returns>
	public static Vector2 Normalise(Vector2 dir) {
		int xValue = dir.x < 0 ? -1 : 1;
		int yValue = dir.y < 0 ? -1 : 1;
		return new Vector2(xValue, yValue);
    }


    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
