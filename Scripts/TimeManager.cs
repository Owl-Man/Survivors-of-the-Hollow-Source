﻿using UnityEngine;

public class TimeManager : MonoBehaviour 
{
	public float slowdownFactor = 0.25f;
	public float slowdownLength = 2f;

	private void Update () 
	{
		// If you hold down 'Space', time will slow down 4x times
		if (Input.GetKey (KeyCode.Space)) 
		{
			Time.timeScale = slowdownFactor;
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
		} 
		else // After you release 'Space', time will back to normal in 2 sec
		{
			Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
			Time.timeScale = Mathf.Clamp (Time.timeScale, 0f, 1f);
		}
	}
}
