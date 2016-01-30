using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

  private BoardManager boardManager;
  private int itemsTotal;
  private int oxygenTotal;
  private int steering;

  public GameObject wall;
  public GameObject floor;
  public GameObject robot;

  void StartGame() {
    boardManager.GenerateBoard(wall, floor);
  }

  void GameOver() {
  }

  void WinGame() {

  }

  void Awake() {
    boardManager = GetComponent<BoardManager>();
    StartGame();
  }
}
