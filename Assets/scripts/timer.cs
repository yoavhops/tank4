using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class timer : MonoBehaviour
{

    public int turn_time = 20;
    private int turn_time_tmp;
    private Text text;
	public Text whos_turn;
	private int turn_index = 0;
	public manager_script manager;
	private bool is_master_client = false;
	private IEnumerator timer_coroutine = null;
    // Use this for initialization
    void Start()
    {
		text = GetComponent<Text>();
    }

	public void start_timer(string player_name, bool is_master_client){
		this.is_master_client = is_master_client;
		turn_time_tmp = turn_time;
		whos_turn.text = manager.player_name() + "'s turn";
		if (timer_coroutine != null) {
			StopCoroutine (timer_coroutine);
		}
		timer_coroutine = counter ();
		StartCoroutine(timer_coroutine);
	}


    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator counter()
    {
		Debug.Log ("Start_counter");
        while (true)
        {
            text.text = turn_time_tmp.ToString();
            yield return new WaitForSeconds(1);
            turn_time_tmp--;
            if (turn_time_tmp < 0)
            {
				if (is_master_client){
					turn_end_by_timer();
				}
                timer_coroutine = null;
                break;
            }
        }
    }

    public void turn_end_by_timer()
    {
		manager.turn_end_by_timer();
    }

}
