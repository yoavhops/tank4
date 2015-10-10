using UnityEngine;
using System.Collections;

[RequireComponent( typeof( PhotonView ) )]
public class manager_script : Photon.MonoBehaviour {

	public smoth_fllow camera; 
	private GameObject tank;/*the tank that it is his turn right now*/
	public GameObject[] player;
	public GameObject game_rect;
	public GameObject camera_follow_obj;
	public timer m_timer;
	public float focos_time_on_explotion = 3.0f;
	private int player_turn = 0;
	public bool master_player_is_first = false;
	public touch_script m_touch_screen;

	public void start_game()
	{
		player_turn = Random.Range (0, player.Length);
		if (master_player_is_first) {
			player_turn = 0;
		}
		photonView.RPC ("start_game_RPC", PhotonTargets.All, player_turn);
	}

	/**
	 * only will be called from the master client.
	 */
	[PunRPC]
	void start_game_RPC(int player_turn){

		Debug.Log ("player turn:");
		Debug.Log (player_turn);
		this.player_turn = player_turn;
		Debug.Log (player_turn);
		tank = player [player_turn];
		m_timer.start_timer (player [player_turn].GetComponent<tank_proproties>().tank_name, PhotonNetwork.isMasterClient);
		/**
		 * each player settes is own tank to be good tank.
		 */
		player [PhotonNetwork.player.ID - 1].GetComponent<tank_proproties> ().is_good_tank = true;
		player [PhotonNetwork.player.ID - 1].GetComponent<PhotonView>().photonView.TransferOwnership(PhotonNetwork.player.ID);
		player [PhotonNetwork.player.ID - 1].GetComponent<Rigidbody2D> ().isKinematic = false;
		player [PhotonNetwork.player.ID - 1].GetComponent<move_with_keys> ().enabled = true;
		gui_debug.debug("I am player : " + ( PhotonNetwork.player.ID - 1) );
		m_touch_screen.start_game (player [PhotonNetwork.player.ID - 1]);
	}
	// Update is called once per frame
	void Update () {
	
	}
	
	public void follow_bullet(GameObject bullet){
		photonView.RPC ("follow_bullet_RPC", PhotonTargets.All, bullet);
	}
	
	[PunRPC]
	public void follow_bullet_RPC(GameObject bullet){
		camera.GetComponent<smoth_fllow>().follow(bullet, camera_follow_type.Bullet);
	}

	public void go_to_tank(){
		Debug.Log("pre");
		StartCoroutine(__go_to_tank());
	}

	IEnumerator __go_to_tank(){
		yield return new WaitForSeconds (focos_time_on_explotion);
		//camera.target = good_tank;
		Vector3 newPos = new Vector3(-tank.transform.position.x, -tank.transform.position.y, game_rect.transform.position.z);
		game_rect.transform.position = newPos;
		camera.GetComponent<smoth_fllow>().follow(camera_follow_obj, camera_follow_type.Rect);
	}

	public void turn_end_by_timer(){
		player_turn++;
		player_turn = player_turn % player.Length;
		photonView.RPC ("turn_end_RPC", PhotonTargets.All, player_turn);

	}
	
	[PunRPC]
	public void turn_end_RPC(int player_turn)
	{
		Debug.Log ("player turn:");
		Debug.Log (player_turn);
		this.player_turn = player_turn;
		tank = player[player_turn];
		m_timer.start_timer (player [player_turn].GetComponent<tank_proproties>().tank_name, PhotonNetwork.isMasterClient);
	}

	public bool is_my_turn(){
		Debug.Log (player_turn);
		Debug.Log (PhotonNetwork.player.ID);

		if (player_turn == PhotonNetwork.player.ID - 1) {
			return true;
		}
		return false;
	}

	public string player_name(){
		return tank.GetComponent<tank_proproties>().tank_name;
	}

}
