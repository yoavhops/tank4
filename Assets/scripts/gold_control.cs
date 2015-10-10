using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (Text))]
public class gold_control : MonoBehaviour {
	private const int DEFAULT_GOLD = 100;
	private const string GOLD_KEY = "gold";
	private Text current_gold;

	void Start () {
		current_gold = GetComponent<Text>();
		current_gold.text = get_gold().ToString();
	}

	public static int get_gold() {
		if (!PlayerPrefs.HasKey(GOLD_KEY)) {
			set_gold(DEFAULT_GOLD);
		}
		return PlayerPrefs.GetInt(GOLD_KEY);
	}
	
	public static void set_gold(int gold) {
		PlayerPrefs.SetInt(GOLD_KEY,gold);
	}

	public void Fee25()	{
		int gold = get_gold();
		if (gold >= 25) {
			gold -= 25;
			set_gold(gold);
			current_gold.text = gold.ToString();
			Application.LoadLevel("main");
		}
		else {
			//TODO: goto buy screen
		}
	}

	public void Buy100() {
		//TODO: goto buy screen
		set_gold(get_gold()+100);
		current_gold.text = get_gold().ToString();
	}
}
