using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

  public float playerSpeed;

  public GameObject projectile;
  private float fireDelay = 0f;

  private Rigidbody2D rb;
  private bool canRestore;
  private GameObject currentStation;
  private GameManager gameManager;

  public Vector2 playerDirection;

  // Use this for initialization
  void Start () {
    playerDirection = new Vector2(0f, 2f);
    rb = GetComponent<Rigidbody2D>();
    canRestore = false;
    gameManager = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameManager>();
  }

  public Vector2 getPlayerDirection() {
    return this.playerDirection;
  }

  void Attack() {
    Vector3 playerPosition = transform.position;

    Debug.Log(playerDirection);
    if (playerDirection.x == 0 && playerDirection.y > 0) {
      playerPosition.y += 1.0f;
    } else if (playerDirection.x == 0 && playerDirection.y < 0) {
      playerPosition.y -= 1.0f;
    } else if (playerDirection.x > 0 && playerDirection.y == 0) {
      playerPosition.x += 1.0f;
    } else if (playerDirection.x < 0 && playerDirection.y == 0) {
      playerPosition.x -= 1.0f;
    }

    Instantiate(projectile, playerPosition, Quaternion.identity);

  }

  // Update is called once per frame
  void Update () {
    if (fireDelay > 0f) {
      fireDelay -= Time.deltaTime;
    }

    if (Input.GetKey(KeyCode.LeftShift) && fireDelay <= 0) {
      Attack();
      fireDelay = 0.3f;
    }

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

    if (!(xVel == 0 && yVel == 0)) {
      playerDirection.x = xVel;
      playerDirection.y = yVel;
    }

    rb.velocity = new Vector2(xVel, yVel);
  }

  void RestoreResource() {
    if (canRestore && Input.GetKey(KeyCode.Space)) {
      print("Restoring Resource");
      gameManager.RestoreResource(currentStation);
    }
  }
}
