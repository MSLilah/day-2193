using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

  private BoxCollider2D boxCollider;
  private Rigidbody2D rb2D;
  private Transform player;
  private GameObject projectile;
  private Vector3 projPosition;

  void Start() {
    boxCollider = GetComponent<BoxCollider2D>();
    rb2D = GetComponent<Rigidbody2D>();

    player = GameObject.FindWithTag("Player").transform;
    projPosition = player.position;
  }

  void Update() {
    Move();
  }

  void Move() {
    if (rb2D.position.y <= 5f) {
      rb2D.velocity = new Vector2(0f, 8f);
    } else {
      Destroy(gameObject);
    }
  }
}
