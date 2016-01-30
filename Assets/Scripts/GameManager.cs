using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

  public GameObject wall;
  public GameObject floor;
  public GameObject robot;
  public GameObject projectile;

  //TODO: Remove this element as it is here for debugging purposes. We will
  // have a nicer display later
  public Text courseOffsetDisplay;
  public Text oxygenDisplay;
  public Text itemDisplay;

  private BoardManager boardManager;

  private static float courseOffset;
  private float courseRateOfIncrease;
  private float courseRateOfRestoration;

  private float currentOxygen;
  private float oxygenRateOfDecrease;
  private float oxygenRateOfRestoration;

  private float currentItems;
  private float itemsRateOfDecrease;
  private float itemsRateOfRestoration;

  private float healthRateOfRestoration;

  private float timeLeft;

  private const int MAX_COURSE_OFFSET = 100;

  private GameObject player;
  private PlayerController pc;

  void StartGame() {
    boardManager.GenerateBoard(wall, floor);

    courseOffset = 15f;
    courseRateOfIncrease = 0.5f;
    courseRateOfRestoration = -1.0f;

    currentOxygen = 100f;
    oxygenRateOfDecrease = 1.0f;
    oxygenRateOfRestoration = 2.0f;

    currentItems = 20f;
    itemsRateOfDecrease = 0.2f;
    itemsRateOfRestoration = 0.3f;

    healthRateOfRestoration = 5.0f;

    timeLeft = 60f;

    player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
    pc = player.GetComponent<PlayerController>();
  }

  void GameOver() {
    Debug.Log("Game Over");
    SceneManager.LoadScene(1);
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
    Instantiate(robot, new Vector3(2.0f, 4.0f, 0f), Quaternion.identity);
    StartGame();
  }

  void Update() {
    if (IsGameOver()) {
      GameOver();
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
}
