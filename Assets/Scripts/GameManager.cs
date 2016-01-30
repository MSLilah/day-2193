using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

  private BoardManager boardManager;
  private int itemsTotal;
  private int oxygenTotal;
  private int steering;

  public GameObject wall;
  public GameObject floor;
  public GameObject robot;

  //TODO: Remove this element as it is here for debugging purposes. We will
  // have a nicer display later
  public Text courseOffsetDisplay;

  private float courseOffset;
  private float courseRateOfIncrease;

  private const int MAX_COURSE_OFFSET = 100;

  void StartGame() {
    Debug.Log("Hello there");
    boardManager.GenerateBoard(wall, floor);

    courseOffset = 15f;
    courseRateOfIncrease = 0.5f;
  }

  void GameOver() {
  }

  void WinGame() {

  }

  void Awake() {
    boardManager = GetComponent<BoardManager>();
    StartGame();
  }

  void Update() {
    courseOffset += courseRateOfIncrease * Time.deltaTime;
    courseOffsetDisplay.text = "Course Offset: " + Mathf.FloorToInt(courseOffset);
  }
}
