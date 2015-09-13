using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class timer : MonoBehaviour
{

    public int turn_time = 60;
    private int turn_time_tmp = 60;
    private Text text;
	public Text whos_turn;
	public GameObject[] player;
	private int turn_index = 0;

    // Use this for initialization
    void Start()
    {
        turn_time_tmp = turn_time;
        text = GetComponent<Text>();
        StartCoroutine("counter");
		
		whos_turn.text = player[turn_index].GetComponent<move_with_keys>().player_name + " turn";
    }

    // Update is called once per frame
    void Update()
    {

    }



    IEnumerator counter()
    {

        while (true)
        {
            text.text = turn_time_tmp.ToString();
            yield return new WaitForSeconds(1);
            turn_time_tmp--;
            if (turn_time_tmp < 0)
            {
                turn_time_tmp = turn_time;
                turn_end();
            }
        }
    }

    public void turn_end()
    {
		turn_index++;
		turn_index = turn_index % player.Length;
		whos_turn.text = player[turn_index].GetComponent<move_with_keys>().player_name + "'s turn";
    }

}
