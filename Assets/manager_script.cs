﻿using UnityEngine;
using System.Collections;

public class manager_script : MonoBehaviour {

	public smoth_fllow camera; 

	public GameObject good_tank;
	
	public float focos_time_on_explotion = 3.0f;
	public float focos_time_on_tank_after_bullet_expoled = 1.0f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void follow_bullet(GameObject bullet){
		camera.enabled = true;
		camera.target = bullet;
	}

	public void go_to_tank(){
		Debug.Log("pre");
		StartCoroutine(__go_to_tank());
	}

	IEnumerator __go_to_tank(){
		yield return new WaitForSeconds (focos_time_on_explotion);
		camera.target = good_tank;
		yield return new WaitForSeconds (focos_time_on_tank_after_bullet_expoled);
		camera.enabled = false;
	}



}
