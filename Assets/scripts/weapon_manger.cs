using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class weapon_manger : MonoBehaviour {

	public GameObject weapon;
	public canvas_util util;
	public GameObject touch_script;

	// Use this for initialization
	void Start () {
		weapon.GetComponent<Button>().colors = util.cb_pressed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	public void weapon_pressed(GameObject obj)
	{
		weapon.GetComponent<Button>().colors = util.cb_not_pressed;

		weapon = obj;

		weapon.GetComponent<Button>().colors = util.cb_pressed;
	}
	
	
	void OnDisable() {
		print("OnDisable");
		touch_script.SetActive (true);
	}
	
	void OnEnable() {
		print("OnEnable");
		touch_script.SetActive (false);
	}

}
