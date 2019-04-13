using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalMethods {

	/// <summary>
	/// Gets the direction of point A to B.
	/// </summary>
	/// <returns>The direction of two points</returns>
	public static Vector2 GetDirection(Vector2 a, Vector2 b) {
		return (b - a).normalized;
	}
}
