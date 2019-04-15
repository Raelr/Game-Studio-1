using System.Collections;
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
}
