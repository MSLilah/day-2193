using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

  public GameObject wall;
  public GameObject floor;
  public GameObject robot;
  public GameObject projectile;

  private BoardManager boardManager;

  public GameObject console;
  public GameObject oxygenStation;
  public GameObject resourceStation;
  public GameObject healthStation;

  private float timeLeft;

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
  }

  public void GameOver() {
    Debug.Log("Game Over");
    SceneManager.LoadScene(Scenes.GAME_OVER);
  }

  void Update() {
    timeLeft -= Time.deltaTime;
    if (timeLeft <= 0) {
      WinGame();
    }
  }

  void WinGame() {

  }

  void Awake() {
    boardManager = GetComponent<BoardManager>();
    StartGame();
  }
}
