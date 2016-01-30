using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

  private BoardManager boardManager;

  public GameObject wall;
  public GameObject floor;

  void StartGame() {
    Debug.Log("Hello there");
    boardManager.GenerateBoard(wall, floor);
  }

  void Awake() {
    boardManager = GetComponent<BoardManager>();
    StartGame();
  }
}
