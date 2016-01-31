using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

  public float playerSpeed;
  public float health;
  public float maxHealth;
  public float invincibilityWindow = 1.0f;

  public GameObject projectile;
  private float fireDelay = 0f;

  private Rigidbody2D rb;
  private GameManager gameManager;
  private bool invincible;
  private bool canInteract;
  private float healthRestorationRate;

  private GameObject collidingStation;
  private SpriteRenderer sr;
  private Animator anim;

  public Vector2 playerDirection;


  private AudioSource audio;
  public float restoreCooldown;
  private float initRestoreCooldown;
  public AudioClip restore;

  // Use this for initialization
  void Start () {
    playerDirection = new Vector2(0f, 2f);
    rb = GetComponent<Rigidbody2D>();
    gameManager = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameManager>();
    maxHealth = 100f;
    health = maxHealth;
    invincible = false;
    canInteract = false;
    healthRestorationRate = 5.0f;
    sr = gameObject.GetComponent<SpriteRenderer>();
    anim = gameObject.GetComponent<Animator>();

    audio = GetComponent<AudioSource>();
    initRestoreCooldown = restoreCooldown;
  }

  public Vector2 getPlayerDirection() {
    return this.playerDirection;
  }

  void Attack() {
    Vector3 playerPosition = transform.position;

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

    if (restoreCooldown <= initRestoreCooldown) {
      restoreCooldown += Time.deltaTime;
    }

    Move();
    InteractWithStation();
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == Tags.RESTORATION_STATION) {
      collidingStation = other.gameObject;
      canInteract = true;
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject.tag == Tags.RESTORATION_STATION) {
      canInteract = false;
    } 
  }

  void Move() {
    float xVel = Input.GetAxisRaw("Horizontal") * playerSpeed;
    float yVel = Input.GetAxisRaw("Vertical") * playerSpeed;

    if (!(xVel == 0 && yVel == 0)) {
      playerDirection.x = xVel;
      playerDirection.y = yVel;
    }

    if (playerDirection.x > 0) {
      sr.flipX = true;
    } else if (playerDirection.x < 0) {
      sr.flipX = false;
    }

    rb.velocity = new Vector2(xVel, yVel);

    if (rb.velocity.magnitude != 0) {
      anim.SetBool("Walking", true);
    } else {
      anim.SetBool("Walking", false);
    }
  }

  void InteractWithStation() {
    if (canInteract && Input.GetKey(KeyCode.Space)) {
      RestorationStationController rsc = collidingStation.GetComponent<RestorationStationController>();
      if (rsc.name == RestorationStations.HEALTH_STATION) {
        RestoreHealth(healthRestorationRate * Time.deltaTime);
      } else {
        rsc.RestoreResource();
      }
    }
  }

  public void Damage(float damage) {
    if (!invincible) {
      health -= damage;
      if (health <= 0) {
        gameManager.GameOver();
      } else {
        invincible = true;
        StartCoroutine("InvincibilityTimer");
      }
    }
  }

  IEnumerator InvincibilityTimer() {
    yield return new WaitForSeconds(invincibilityWindow);
    invincible = false;
  }

  public void RestoreHealth(float toRestore) {
    if (restoreCooldown >= initRestoreCooldown) {
      audio.PlayOneShot(restore, 0.7F);
      restoreCooldown = 0f;
    }
    health = Mathf.Min(health + toRestore, maxHealth);
  }
}
