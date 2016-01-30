using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

  public float playerSpeed;

  private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
	  rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	  Move();
	}

  void Move() {
    float xVel = Input.GetAxisRaw("Horizontal") * playerSpeed;
    float yVel = Input.GetAxisRaw("Vertical") * playerSpeed;

    rb.velocity = new Vector2(xVel, yVel);
  }
}
