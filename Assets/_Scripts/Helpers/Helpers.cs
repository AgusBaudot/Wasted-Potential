using UnityEngine;

public static class Helpers
{
	private static Camera MainCamera;
	
	public static Camera GetCamera()
	{
		if (MainCamera == null) MainCamera = Camera.main;
		return MainCamera;
	}
}