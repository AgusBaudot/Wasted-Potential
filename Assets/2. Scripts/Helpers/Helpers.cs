using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class Helpers
{
	private static Camera MainCamera;
	
	public static Camera GetCamera()
	{
		if (MainCamera == null) MainCamera = Camera.main;
		return MainCamera;
	}

	private static Dictionary<float, WaitForSeconds> lookUp = new();
	public static WaitForSeconds GetWait(float wait)
	{
		if (!lookUp.ContainsKey(wait))
			lookUp[wait] = new WaitForSeconds(wait);

		return lookUp[wait];
	}

	public static void FadeIn(CanvasGroup canvasGroup, float fadeTime)
	{
		canvasGroup.DOFade(1, fadeTime);
	}

	public static void FadeOut(CanvasGroup canvasGroup, float fadeTime)
	{
		canvasGroup.DOFade(0, fadeTime);
	}
}