using UnityEngine;
using System.Collections;


[RequireComponent(typeof(PhotonView))]
public class bullet_script : Photon.MonoBehaviour
{
	private Vector2 wind_str;
    private pixel_control pixel_cnr;
    public GameObject explosion;
	public manager_script game_manager;

    // Use this for initialization
    void Start ()
    {
        wind_str = GameObject.Find("wind").GetComponent<wind>().wind_str;
		pixel_cnr = GameObject.Find("ground").GetComponent<pixel_control>();
		game_manager = GameObject.Find("the game").GetComponent<manager_script>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}


	void FixedUpdate(){
		gameObject.GetComponent<Rigidbody2D>().AddForce (wind_str);
		
		if (transform.position.y < -5) {
			destory_this ();
		}
	}


    void OnCollisionEnter2D(Collision2D coll)
    {

        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }
        photonView.RPC("bulllet_collided_RPC", PhotonTargets.All, transform.position);
        /*
        Sprite sprite =  GetComponent<SpriteRenderer>().sprite;

        pixel_cnr.kill_by_sub_texture_shape_world_pos(transform.localPosition.x, transform.localPosition.y,
            (int)sprite.textureRect.width, (int)sprite.textureRect.height,
            (int)sprite.textureRect.x, (int)sprite.textureRect.y, sprite.texture);
        */

    }

	void destory_this(){
		game_manager.bullet_exploded();
		Destroy(gameObject);
	}

    [PunRPC]
    public void bulllet_collided_RPC(Vector3 position)
    {
        GameObject explo = Instantiate(explosion) as GameObject;
        explo.transform.position = position;

        destory_this();
    }




}
