using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

  public float enemySpeed = 1f;
  public float enemyDamage = 5f;
  public float enemyHealth = 15f;

  private GameObject target;
  private Rigidbody2D rb;
  private bool attackingTarget;

  private GameManager gm;

	// Use this for initialization
	void Start () {
	  rb = gameObject.GetComponent<Rigidbody2D>();
    gm = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameManager>();
    attackingTarget = false;
	}
	
	// Update is called once per frame
	void Update () {
	  if (!HasTarget()) {
      SelectTarget();
    }

    Move();

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
    if (DistanceToTarget() > 0.8f) {
      rb.velocity = direction * enemySpeed;
    } else {
      rb.velocity = Vector2.zero;
    }
  }

  void AttackTarget() {
    if (target.tag == Tags.RESTORATION_STATION) {
      target.GetComponent<RestorationStationController>().DamageResource();
    } else if (target.tag == Tags.PLAYER) {
      target.GetComponent<PlayerController>().Damage(enemyDamage);
    }
  }

  public void Damage(float damage) {
    enemyHealth -= damage;
    if (enemyHealth <= 0) {
      Destroy(this.gameObject);
    } else {
      target = GameObject.FindGameObjectWithTag(Tags.PLAYER);
    }
  }
}
