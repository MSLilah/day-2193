using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

  private BoxCollider2D boxCollider;
  private Rigidbody2D rb2D;
  private Transform playerTransform;
  private GameObject player;
  private GameObject projectile;
  private Vector3 projPosition;

  private Vector2 playerDirection;

  private float xVel;
  private float yVel;

  void Start() {
    boxCollider = GetComponent<BoxCollider2D>();
    rb2D = GetComponent<Rigidbody2D>();

    player = GameObject.FindWithTag(Tags.PLAYER);
    playerTransform = player.transform;

    projPosition = playerTransform.position;

    playerDirection = player.GetComponent<PlayerController>().getPlayerDirection();

    xVel = playerDirection.x;
    yVel = playerDirection.y;
  }

  void Update() {
    Move();
  }

  void Move() {
    rb2D.velocity = new Vector2(xVel, yVel) * 2f;
  }

  void OnTriggerEnter2D(Collider2D coll) {
    if (coll.gameObject.tag == Tags.BORDER) {
      Destroy(gameObject);
    }
  }
}
