using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

  public float enemySpeed = 3f;
  public float enemyDamage = 5f;
  public float enemyHealth = 15f;

  private GameObject target;
  private Rigidbody2D rb;
  private bool attackingTarget;

  private GameManager gm;
  private Animator anim;

  private AudioSource audio;
  public AudioClip death1;
  public AudioClip death2;
  public AudioClip death3;

  private float roarCooldown;
  public AudioClip roar1;
  public AudioClip roar2;
  public AudioClip roar3;
  public AudioClip roar4;

  // Use this for initialization
  void Start () {
    rb = gameObject.GetComponent<Rigidbody2D>();
    gm = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameManager>();
    attackingTarget = false;
    anim = gameObject.GetComponent<Animator>();

    audio = GetComponent<AudioSource>();
    roarCooldown = 3f;
  }

  // Update is called once per frame
  void Update () {
    if (!HasTarget()) {
      SelectTarget();
    }

    Move();

    if (roarCooldown <= 3f) {
      roarCooldown += Time.deltaTime;
    }

    if (attackingTarget) {
      AttackTarget();
    }
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject == target) {
      attackingTarget = true;
    }
  }

  void OnTriggerStay2D(Collider2D other) {
    if (other.gameObject.tag == Tags.PLAYER && other.gameObject != target) {
      other.gameObject.GetComponent<PlayerController>().Damage(enemyDamage);
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject == target) {
      attackingTarget = false;
    }
  }

  bool HasTarget() {
    return target != null;
  }

  // Select a target for the Enemy to attack
  void SelectTarget() {

    // Figure out which room the enemy is currently in
    BoardManager.Locations enemyLocation = BoardManager.DetermineLocation(gameObject.transform.position);
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
    if (!attackingTarget) {
      rb.velocity = direction * enemySpeed;
      anim.SetBool("Walking", true);
    } else {
      rb.velocity = Vector2.zero;
      anim.SetBool("Walking", false);
    }
  }

  void AttackTarget() {
    if (target.tag == Tags.RESTORATION_STATION) {
      target.GetComponent<RestorationStationController>().DamageResource();
    } else if (target.tag == Tags.PLAYER) {
      target.GetComponent<PlayerController>().Damage(enemyDamage);

      if (roarCooldown >= 3f) {
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

        roarCooldown = 0f;
      }
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
}
