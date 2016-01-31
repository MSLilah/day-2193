using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

  public GameObject wall;
  public GameObject floor;
  public GameObject robot;
  public GameObject projectile;

  public GameObject enemy;
  private List<EnemyController> enemies;
  private float enemySpawnCooldown;

  private BoardManager boardManager;

  public GameObject console;
  public GameObject oxygenStation;
  public GameObject resourceStation;
  public GameObject healthStation;

  public float timeLeft;

  private const int MAX_COURSE_OFFSET = 100;

  private GameObject player;
  private PlayerController pc;

  void StartGame() {
    boardManager.PlaceObj(robot, new Vector3(2.0f, 4.0f, 0f));

    boardManager.PlaceObj(console, new Vector3(8.0f, 3.0f, 0f));

    boardManager.PlaceObj(oxygenStation, new Vector3(23f, 17f, 0f));

    boardManager.PlaceObj(resourceStation, new Vector3(-12f, 13f, 0f));

    timeLeft = 60f;

    player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
    pc = player.GetComponent<PlayerController>();

    boardManager.GenerateBoard(wall, floor);

    enemySpawnCooldown = 0f;
  }

  public void GameOver() {
    Debug.Log("Game Over");
    SceneManager.LoadScene(Scenes.GAME_OVER);
  }

  void WinGame() {

  }

  void Awake() {
    boardManager = GetComponent<BoardManager>();
    StartGame();
  }

  void Update() {
    timeLeft -= Time.deltaTime;
    if (timeLeft <= 0) {
      WinGame();
    }

    if (enemySpawnCooldown >= 0) {
      enemySpawnCooldown -= Time.deltaTime;
    }

    if (Mathf.FloorToInt(timeLeft) % 8 == 0 && enemySpawnCooldown < 0) {
      SpawnEnemies(Random.Range(1, 3), (BoardManager.Locations)Random.Range(0,3));
      enemySpawnCooldown = 5f;
    }
  }

  void SpawnEnemies(int total, BoardManager.Locations location) {
    //enemies.Clear();

    Debug.Log("Total: " + total + " Location: " + location);
    for (int i = 0; i < total; i++) {
      SpawnEnemy(location);
    }
  }

  void SpawnEnemy(BoardManager.Locations location) {
    Vector3 position;

    float minX;
    float maxX;
    float minY;
    float maxY;

    switch (location) {
      case BoardManager.Locations.Cockpit:
        Debug.Log("Cockpit");
        minX = 0;
        maxX = 13;
        minY = 0;
        maxY = 13;
        break;
      case BoardManager.Locations.Oxygen:
        Debug.Log("Oxygen");
        minX = 16;
        maxX = 28;
        minY = 9;
        maxY = 20;
        break;
      case BoardManager.Locations.Items:
        Debug.Log("Items");
        minX = -14;
        maxX = -2;
        minY = 9;
        maxY = 20;
        break;
      case BoardManager.Locations.Health:
        Debug.Log("Health");
        minX = 0;
        maxX = 14;
        minY = 15;
        maxY = 29;
        break;
      default:
        // in case of a default, just go to the cockpit
        minX = 0;
        maxX = 15;
        minY = 0;
        maxY = 15;
        break;
    }

    position = new Vector3(Random.Range(minX, maxY), Random.Range(minY, maxY), 0f);
    GameObject en = Instantiate(enemy, position, Quaternion.identity) as GameObject;
  }
}
