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
    // Head for the closest restoration station
    GameObject closestStation = null;
    float distanceToClosest = 5000f;
    GameObject[] stations = GameObject.FindGameObjectsWithTag(Tags.RESTORATION_STATION);

    foreach (GameObject obj in stations) {
      Transform station = obj.transform;
      float distance = Vector2.Distance(gameObject.transform.position, station.position);

      if (distance < distanceToClosest) {
        distanceToClosest = distance;
        closestStation = obj;
      }
    }

    target = closestStation;
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
      gm.DecreaseResource(target);
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
