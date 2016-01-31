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

  //TODO: Remove this element as it is here for debugging purposes. We will
  // have a nicer display later
  public Text courseOffsetDisplay;
  public Text oxygenDisplay;
  public Text itemDisplay;

  private BoardManager boardManager;

  private static float courseOffset;
  private float courseRateOfIncrease;
  private float courseRateOfRestoration;
  public GameObject console;

  private float currentOxygen;
  private float oxygenRateOfDecrease;
  private float oxygenRateOfRestoration;
  public GameObject oxygenStation;

  private float currentItems;
  private float itemsRateOfDecrease;
  private float itemsRateOfRestoration;
  public GameObject resourceStation;

  private float healthRateOfRestoration;
  public GameObject healthStation;

  private float timeLeft;

  private const int MAX_COURSE_OFFSET = 100;

  private GameObject player;
  private PlayerController pc;

  void StartGame() {
    boardManager.PlaceObj(robot, new Vector3(2.0f, 4.0f, 0f));

    courseOffset = 15f;
    courseRateOfIncrease = 0.5f;
    courseRateOfRestoration = -1.0f;

    boardManager.PlaceObj(console, new Vector3(8.0f, 3.0f, 0f));

    currentOxygen = 100f;
    oxygenRateOfDecrease = 1.0f;
    oxygenRateOfRestoration = 2.0f;

    boardManager.PlaceObj(oxygenStation, new Vector3(23f, 17f, 0f));

    currentItems = 20f;
    itemsRateOfDecrease = 0.2f;
    itemsRateOfRestoration = 0.3f;

    boardManager.PlaceObj(resourceStation, new Vector3(-12f, 13f, 0f));

    healthRateOfRestoration = 5.0f;

    timeLeft = 60f;

    player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
    pc = player.GetComponent<PlayerController>();


    boardManager.GenerateBoard(wall, floor);

    enemySpawnCooldown = 0f;
  }

  void GameOver() {
    Debug.Log("Game Over");
    SceneManager.LoadScene(Scenes.GAME_OVER);
  }

  bool IsGameOver() {
    // End conditions
    //
    if (!pc.IsAlive()) {
      return true;
    } else if (timeLeft < 0f) {
      return true;
    } else if (currentOxygen <= 0) {
      return true;
    } else if (currentItems <= 0) {
      return true;
    } else if (courseOffset >= 100) {
      return true;
    } else {
      return false;
    }
  }

  void WinGame() {

  }

  void Awake() {
    boardManager = GetComponent<BoardManager>();
    StartGame();
  }

  void Update() {
    if (IsGameOver()) {
      GameOver();
    }

    if (enemySpawnCooldown >= 0) {
      enemySpawnCooldown -= Time.deltaTime;
    }

    if (Mathf.FloorToInt(timeLeft) % 8 == 0 && enemySpawnCooldown < 0) {
      SpawnEnemies(Random.Range(1, 3), (BoardManager.Locations)Random.Range(0,3));
      enemySpawnCooldown = 5f;
    }

    // Decrease resources
    PeriodicDecreaseResources();
  }

  void PeriodicDecreaseResources() {
    courseOffset += courseRateOfIncrease * Time.deltaTime;
    currentOxygen -= Time.deltaTime * oxygenRateOfDecrease;
    currentItems -= Time.deltaTime * itemsRateOfDecrease;
    timeLeft -= Time.deltaTime;

    // TODO: Remove these in favor of a more interesting interface
    courseOffsetDisplay.text = "Course Offset: " + Mathf.FloorToInt(courseOffset);
    oxygenDisplay.text = "Oxygen: " + Mathf.CeilToInt(currentOxygen);
    itemDisplay.text = "Items: " + Mathf.CeilToInt(currentItems);
  }

  public void RestoreResource(GameObject restoringStation) {
    switch (restoringStation.name) {
      case RestorationStations.CONSOLE:
        courseOffset += courseRateOfRestoration * Time.deltaTime;
        break;
      case RestorationStations.OXYGEN_STATION:
        currentOxygen += oxygenRateOfRestoration * Time.deltaTime;
        break;
      case RestorationStations.ITEM_STATION:
        currentItems += itemsRateOfRestoration * Time.deltaTime;
        break;
      case RestorationStations.HEALTH_STATION:
        pc.RestoreHealth(healthRateOfRestoration * Time.deltaTime);
        break;
      default:
        // Do nothing, as this was errantly triggered
        break;
    }
  }

  public void DecreaseResource(GameObject restoringStation) {
    switch (restoringStation.name) {
      case RestorationStations.CONSOLE:
        courseOffset += 4 * courseRateOfIncrease * Time.deltaTime;
        break;
      case RestorationStations.OXYGEN_STATION:
        currentOxygen -= 4 * oxygenRateOfDecrease * Time.deltaTime;
        break;
      case RestorationStations.ITEM_STATION:
        currentItems -= 4 * itemsRateOfDecrease * Time.deltaTime;
        break;
      default:
        // Do nothing, as this was errantly triggered
        break;
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
        minX = 15;
        maxX = 29;
        minY = 7;
        maxY = 22;
        break;
      case BoardManager.Locations.Items:
        Debug.Log("Items");
        minX = -15;
        maxX = -1;
        minY = 7;
        maxY = 22;
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
