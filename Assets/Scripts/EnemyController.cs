using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

  public float enemySpeed = 3f;
  public float enemyDamage = 5f;
  public float enemyHealth = 15f;

  private GameObject target;
  private Rigidbody2D rb;
  private bool canAttack;

  private GameManager gm;
  private Animator anim;
  private SpriteRenderer sr;

  private AudioSource audio;
  public AudioClip death1;
  public AudioClip death2;
  public AudioClip death3;

  public AudioClip roar1;
  public AudioClip roar2;
  public AudioClip roar3;
  public AudioClip roar4;

  private float timeSinceAttack;
  private float attackCooldown;

  private bool touchingPlayer;
  private bool touchingStation;

  // Use this for initialization
  void Start () {
    rb = gameObject.GetComponent<Rigidbody2D>();
    gm = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameManager>();
    canAttack = true;
    anim = gameObject.GetComponent<Animator>();

    audio = GetComponent<AudioSource>();

    sr = gameObject.GetComponent<SpriteRenderer>();

    timeSinceAttack = 0.0f;
    attackCooldown = 2.0f;
  }

  // Update is called once per frame
  void Update () {
    if (!HasTarget()) {
      SelectTarget();
    }

    Move();

    if (TouchingTarget() && canAttack) {
      AttackTarget();
    } else if (!canAttack) {
      timeSinceAttack += Time.deltaTime;
      if (timeSinceAttack >= attackCooldown) {
        canAttack = true;
      }
    }

    if (touchingPlayer) {
      GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<PlayerController>().Damage(enemyDamage);
    }
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == Tags.RESTORATION_STATION && other.gameObject == target) {
      touchingStation = true;
    } else if (other.gameObject.tag == Tags.PLAYER) {
      touchingPlayer = true;
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject.tag == Tags.PLAYER) {
      touchingPlayer = false;
    } else if (other.gameObject.tag == Tags.RESTORATION_STATION && other.gameObject == target) {
      touchingStation = false;
    }
  }

  bool HasTarget() {
    return target != null;
  }

  // Select a target for the Enemy to attack
  void SelectTarget() {

    GameObject player = GameObject.FindGameObjectWithTag(Tags.PLAYER);

    // Figure out which room the enemy is currently in
    BoardManager.Locations enemyLocation = BoardManager.DetermineLocation(gameObject.transform.position);

    // Ensure there is a 20% chance that an enemy in the same room as a player will attack them
    if (Random.Range(0, 100) < 20 && enemyLocation == BoardManager.DetermineLocation(player.transform.position)) {
      target = player;
      return;
    }

    // Head for the closest restoration station
    GameObject closestStation = null;
    float distanceToClosest = 5000f;
    GameObject[] stations = GameObject.FindGameObjectsWithTag(Tags.RESTORATION_STATION);

    // Calculate the closest Restoration Station
    foreach (GameObject obj in stations) {
      // Only take into account stations that are not health stations and that are in the same room as the enemy
      if (obj.name != RestorationStations.HEALTH_STATION && BoardManager.DetermineLocation(obj.transform.position) == enemyLocation) {
        Transform station = obj.transform;
        float distance = Vector2.Distance(gameObject.transform.position, station.position);

        if (distance < distanceToClosest) {
          distanceToClosest = distance;
          closestStation = obj;
        }
      }
    }

    if (closestStation != null) {
      target = closestStation;
    } else {
      target = GameObject.FindGameObjectWithTag(Tags.PLAYER);
    }
  }

  float DistanceToTarget() {
    return Vector2.Distance(gameObject.transform.position, target.transform.position);
  }

  void Move() {
    Vector2 direction = (target.transform.position - gameObject.transform.position).normalized;
    if (!TouchingTarget()) {
      rb.velocity = direction * enemySpeed;
      anim.SetBool("Walking", true);
      if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x)) {
        if (direction.y > 0) {
          anim.SetInteger("Direction", 3);
        } else {
          anim.SetInteger("Direction", 2);
        }
      } else {
        if (direction.x > 0) {
          anim.SetInteger("Direction", 0);
          sr.flipX = false;
        } else {
          anim.SetInteger("Direction", 1);
          sr.flipX = true;
        }
      }
    } else {
      rb.velocity = Vector2.zero;
      anim.SetBool("Walking", false);
    }
  }

  void AttackTarget() {
    if (target.tag == Tags.PLAYER) {
      int randomAttackSound = Random.Range(0,100);
      if (randomAttackSound % 2 == 0) {
        int randomSound = Random.Range(0, 4);
        if (randomSound == 0) {
          audio.PlayOneShot(roar1, 1.0F);
        } else if (randomSound == 1) {
          audio.PlayOneShot(roar2, 1.0F);
        } else if (randomSound == 2) {
          audio.PlayOneShot(roar3, 1.0F);
        } else if (randomSound == 3) {
          audio.PlayOneShot(roar4, 1.0F);
        }
      }
    }
    anim.SetTrigger("Attack");
    timeSinceAttack = 0.0f;
    canAttack = false;
  }

  public void DamageTarget() {
    if (target.tag == Tags.RESTORATION_STATION) {
      target.GetComponent<RestorationStationController>().DamageResource();
    } else if (target.tag == Tags.PLAYER && DistanceToTarget() <= 1.5f) {
      target.GetComponent<PlayerController>().Damage(enemyDamage);
    }
  }

  public void Damage(float damage) {
    enemyHealth -= damage;
    if (enemyHealth <= 0) {
      int randomDeathSound = Random.Range(0, 2);

      if (randomDeathSound == 0) {
        audio.PlayOneShot(death1, 1.0F);
      } else if (randomDeathSound == 1) {
        audio.PlayOneShot(death2, 1.0F);
      } else if (randomDeathSound == 2) {
        audio.PlayOneShot(death3, 1.0F);
      }

      Destroy(this.gameObject);
    } else {
      target = GameObject.FindGameObjectWithTag(Tags.PLAYER);
    }
  }

  bool TouchingTarget() {
    return (target.tag == Tags.PLAYER && touchingPlayer) ||
            (target.tag == Tags.RESTORATION_STATION && touchingStation);
  }
}
