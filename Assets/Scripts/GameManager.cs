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
    Debug.Log("Hello there");
    boardManager.GenerateBoard(wall, floor);
  }

  void GameOver() {
    if (oxygenTotal == 0 || steering == 0) {
      
    }
  }

  void WinGame() {

  }

  void Start() {
    boardManager = GetComponent<BoardManager>();
    StartGame();
  }
}
