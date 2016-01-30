using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

  public float enemySpeed = 1f;

  private Transform target;
  private Rigidbody2D rb;

  private GameManager gm;
  private GameObject collidingWith;
  private bool collidingWithObject;

	// Use this for initialization
	void Start () {
	  rb = gameObject.GetComponent<Rigidbody2D>();
    gm = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameManager>();
    collidingWithObject = false;
	}
	
	// Update is called once per frame
	void Update () {
	  if (!HasTarget()) {
      SelectTarget();
    }

    Move();
    InteractWithTarget();
	}

  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == Tags.RESTORATION_STATION || other.gameObject.tag == Tags.PLAYER) {
      collidingWithObject = true;
      collidingWith = other.gameObject;
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    collidingWithObject = false;
  }

  bool HasTarget() {
    return target != null;
  }

  // Select a target for the Enemy to attack
  void SelectTarget() {
    // Head for the closest restoration station
    Transform closestStation = null;
    float distanceToClosest = 5000f;
    GameObject[] stations = GameObject.FindGameObjectsWithTag(Tags.RESTORATION_STATION);

    foreach (GameObject obj in stations) {
      Transform station = obj.transform;
      float distance = Vector2.Distance(gameObject.transform.position, station.position);

      if (distance < distanceToClosest) {
        distanceToClosest = distance;
        closestStation = station;
      }
    }

    target = closestStation;
  }

  float DistanceToTarget() {
    return Vector2.Distance(gameObject.transform.position, target.position);
  }

  void Move() {
    Vector2 direction = (target.position - gameObject.transform.position).normalized;
    if (DistanceToTarget() > 0.8f) {
      rb.velocity = direction * enemySpeed;
    } else {
      rb.velocity = Vector2.zero;
    }

  }

  void InteractWithTarget() {
    if (collidingWithObject) {
      if (collidingWith.tag == Tags.RESTORATION_STATION) {
        gm.DecreaseResource(collidingWith);
      }
    }
  }
}
