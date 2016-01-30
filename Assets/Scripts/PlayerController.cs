using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

  public float playerSpeed;

  private Rigidbody2D rb;
  private bool canRestore;
  private GameObject currentStation;
  private GameManager gameManager;

  // Use this for initialization
  void Start () {
    rb = GetComponent<Rigidbody2D>();
    canRestore = false;
    gameManager = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameManager>();
  }

  // Update is called once per frame
  void Update () {
    Move();
    RestoreResource();
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == Tags.RESTORATION_STATION) {
      canRestore = true;
      currentStation = other.gameObject;
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    canRestore = other.gameObject.tag == Tags.RESTORATION_STATION;
  }

  void Move() {
    float xVel = Input.GetAxisRaw("Horizontal") * playerSpeed;
    float yVel = Input.GetAxisRaw("Vertical") * playerSpeed;

    rb.velocity = new Vector2(xVel, yVel);
  }

  void RestoreResource() {
    if (canRestore && Input.GetKey(KeyCode.Space)) {
      print("Restoring Resource");
      gameManager.RestoreResource(currentStation);
    }
  }
}
