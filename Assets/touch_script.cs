using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class touch_script : MonoBehaviour {

	public manager_script manager;

	private Vector3 mouseposition;
	private Vector3 prev_mouseposition;
	private Vector3 mouse_diff;
	public float speed = 0.1f;
	public float fade_move_speed = 0.6f;
	public GameObject good_tank;
	public GameObject good_tank_canon;
	public GameObject good_tank_canon_shoting_edge;
	public GameObject good_tank_bullet;
	public float shoting_force = 100.0f;
	public float jumping_force = 10000.0f;
	private float canon_angle;
	public GameObject arrow;
	
	enum clicked_on {nothing ,background, good_tank, bad_tank};
	private clicked_on mouse_on;
	private GameObject hit_game_object;

	private GameObject[] arrows; 
	public float size_of_arrow = 0.1f;
	public float distance_between_arrows = 0.05f;
	public int max_amount_of_arrows = 10;



	public float circle_r = 1.0f;
	public float circle_width = 0.02f;
	public float theta_scale = 0.1f;  
	public float min_distance = 0.3f;
	public Color circle_color;
	private LineRenderer lineRenderer;
	
	private bool shot = false;
	private Vector3 diff_norm;
	private Vector3 new_mouse_pos;

	public GameObject the_game;
	public GameObject ui_background;

	public left_panal_manager lpm;
	public float enable_tank_rigid_time = 0.1f;
	// Use this for initialization
	void Start () {
		mouse_on = clicked_on.nothing;
		arrows = new GameObject[max_amount_of_arrows];
		for (int i = 0; i < max_amount_of_arrows; i++) {
			arrows[i]=(GameObject) Instantiate(arrow);
			arrows[i].SetActive(false);
		}

		//circle part
		int size = (int)((2.0f * Mathf.PI) / theta_scale) + 2; //Total number of points in circle.
		
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(circle_color, circle_color);
		lineRenderer.SetWidth(circle_width, circle_width);
		lineRenderer.SetVertexCount(size);
		lineRenderer.enabled = false;

		scale_circle (good_tank.transform.position, 1.0f);



	}


	void scale_circle(Vector3 center, float r){
		
		int size = (int)((2.0f * Mathf.PI) / theta_scale) + 2; //Total number of points in circle.
		
		int i = 0;
		float x;
		float y;
		float theta;
		Vector3 pos;
		
		for(theta = 0; theta < 2 *  Mathf.PI; theta += theta_scale) {
			x = r * Mathf.Cos(theta);
			y = r * Mathf.Sin(theta);
			
			pos = new Vector3(center.x +  x,center.y + y, 0);
			lineRenderer.SetPosition(i, pos);
			i+=1;
		}
		
		theta = 0;
		x = r * Mathf.Cos(theta);
		y = r * Mathf.Sin(theta);			
		pos = new Vector3(center.x +  x,center.y + y, -1.0f);
		lineRenderer.SetPosition(i, pos);
	}



	// Update is called once per frame
	void Update () {
		int i;

		//pressed first time
		if (Input.GetMouseButtonDown (0)) {

			mouse_diff = Vector3.zero;
			prev_mouseposition = Input.mousePosition;
			get_clicked_on(ref mouse_on, ref hit_game_object);

			if (mouse_on == clicked_on.good_tank){

				ui_background.GetComponent<ScrollRect>().enabled = false;

				/*
				Vector3 look_at_tank = new Vector3(hit_game_object.transform.position.x , 
				                                   hit_game_object.transform.position.y, 
				                                   Camera.main.transform.position.z);
				Camera.main.transform.position = look_at_tank;
				*/
			}

		} else {
			//conitune to press.
			if (Input.GetMouseButton(0)){
				mouse_diff += prev_mouseposition - Input.mousePosition;
				prev_mouseposition = Input.mousePosition;

				if ((mouse_on == clicked_on.good_tank) && (lineRenderer.enabled == false)){
					lineRenderer.enabled = true;
				}
			}
			//pick finger up
			if (Input.GetMouseButtonUp (0))  {

				if (mouse_on == clicked_on.good_tank){
					ui_background.GetComponent<ScrollRect>().enabled = true;
				}

				mouse_on = clicked_on.nothing;
				for (i = 0;i < max_amount_of_arrows ; i++ ){
					arrows[i].SetActive(false);
				}

				if (shot){
					shot = false;
					Vector2 canon_angle_vec2 = new Vector2(diff_norm.x, diff_norm.y);
					canon_angle_vec2 *=-1;
					float distnace = Vector3.Distance(new_mouse_pos, good_tank.transform.position);
					float distnace_ratio = distnace /( max_amount_of_arrows * size_of_arrow);

					if (distnace > min_distance){
						if (lpm.btn.name == "weapon"){
							GameObject new_bullet =(GameObject) Instantiate(good_tank_bullet);
							new_bullet.transform.parent = the_game.transform;
							new_bullet.transform.position = good_tank_canon_shoting_edge.transform.position;
							new_bullet.GetComponent<Rigidbody2D>().AddForce( canon_angle_vec2 * shoting_force * distnace_ratio);

							manager.follow_bullet(new_bullet);

						}
						if (lpm.btn.name == "jump"){
							good_tank.GetComponent<Rigidbody2D>().AddForce( canon_angle_vec2 * jumping_force * distnace_ratio);
							disable_tank_rigids(good_tank);
						}
					}
				}
			}
		}
		
		//Debug.Log (mouse_on);

		if (mouse_on == clicked_on.background) {
			mouse_diff = mouse_diff * fade_move_speed;
			if (mouse_diff.sqrMagnitude > 0.01) {
				Camera.main.transform.position += (mouse_diff * (speed * Time.deltaTime));
			}
			return;
		}

		if (mouse_on == clicked_on.good_tank) {

			lineRenderer.enabled = true;
			shot = true;

			new_mouse_pos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, 9.0f);
			new_mouse_pos = Camera.main.ScreenToWorldPoint(new_mouse_pos);

			float distnace = Vector3.Distance(new_mouse_pos, good_tank.transform.position);
			int amount_of_arrows = (int)(distnace / size_of_arrow);
			Vector3 diff = good_tank.transform.position - new_mouse_pos;
			diff_norm = diff.normalized * -1;
			if (amount_of_arrows >= max_amount_of_arrows){
				distnace = max_amount_of_arrows * size_of_arrow;
				amount_of_arrows = max_amount_of_arrows;

				new_mouse_pos = new Vector3(good_tank.transform.position.x + diff_norm.x * distnace,
				                      good_tank.transform.position.y + diff_norm.y * distnace,
				                      good_tank.transform.position.z);
				diff = good_tank.transform.position - new_mouse_pos;
			}

			if (distnace < min_distance){
				
				for ( i = 0; i < amount_of_arrows; i++ ){
					arrows[i].SetActive(false);
				}
				lineRenderer.enabled = false;
				return;
			}

			scale_circle (good_tank.transform.position, distnace);
			canon_angle = Mathf.Atan2(diff.y,diff.x) * Mathf.Rad2Deg;

			good_tank_canon.transform.rotation = Quaternion.AngleAxis(canon_angle, Vector3.forward);
			for (i = 0; i < amount_of_arrows; i++ ){
				arrows[i].SetActive(true);

				arrows[i].transform.rotation = Quaternion.AngleAxis(canon_angle, Vector3.forward);

				Vector3 arrow_pos = new Vector3(new_mouse_pos.x + (diff.x * i)/amount_of_arrows, new_mouse_pos.y + (diff.y * i)/amount_of_arrows, -1f);
				arrows[i].transform.position = arrow_pos;
			}
			for (;i < max_amount_of_arrows ; i++ ){
				arrows[i].SetActive(false);
			}
		}
		else{
			lineRenderer.enabled = false;
		}
	}


	void get_clicked_on(ref clicked_on mouse_on, ref GameObject hit_game_object){
		Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		
        int layerMask = 1 << 9;
		
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity, layerMask);
		if (hit == null || hit.collider == null) {
			mouse_on = clicked_on.nothing;
			hit_game_object = null;
			return;
		}
		hit_game_object = hit.collider.gameObject;
		if (hit.collider.name == "background") {
			mouse_on = clicked_on.background;
			return;
		}
		
		if (hit.collider.name == "good_tank") {
			mouse_on = clicked_on.good_tank;
			return;
		}
		mouse_on = clicked_on.nothing;
	}

	void disable_tank_rigids(GameObject tank){
		Collider2D[] coliders;
		coliders = tank.GetComponentsInChildren<Collider2D> ();
		foreach (Collider2D collide in coliders) {
			if (collide.gameObject == tank){
				continue;
			}
			//disable wheels
			collide.isTrigger = true;
		}
		StartCoroutine(enable_tank_rigids(tank));
	}

	IEnumerator enable_tank_rigids(GameObject tank){
		yield return new WaitForSeconds(enable_tank_rigid_time);
		Collider2D[] coliders;
		coliders = tank.GetComponentsInChildren<Collider2D> ();
		foreach (Collider2D collide in coliders) {
			if (collide.gameObject == tank){
				continue;
			}
			collide.isTrigger = false;
		}
	}

}
