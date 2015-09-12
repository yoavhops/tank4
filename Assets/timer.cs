using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class timer : MonoBehaviour
{

    public int turn_time = 60;
    private int turn_time_tmp = 60;
    private Text text;

    // Use this for initialization
    void Start()
    {
        turn_time_tmp = turn_time;
        text = GetComponent<Text>();
        StartCoroutine("counter");
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

    }

}
