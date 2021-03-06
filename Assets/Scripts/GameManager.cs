﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

  public GameObject wall;
  public GameObject floor;
  public GameObject WallUpper;
  public GameObject WallLower;
  public GameObject robot;
  public GameObject projectile;

  public GameObject enemy;
  private List<EnemyController> enemies;
  private float enemySpawnCooldown;

  private BoardManager boardManager;

  public GameObject console;
  public GameObject resourceStation;
  public GameObject healthStation;

  public GameObject oxygenStationCory;
  public GameObject oxygenStationBrian;
  public GameObject oxygenStationPreben;
  public GameObject oxygenStationJoseph;

  public float timeLeft;

  private const int MAX_COURSE_OFFSET = 100;

  private GameObject player;
  private PlayerController pc;

  public int oxygenStationTotal;

  private AudioSource audio;
  public AudioClip enemySpawnVent;
  public AudioClip enemySpawnGlass;
  public AudioClip enemySpawnPot;

  void StartGame() {
    boardManager.PlaceObj(robot, new Vector3(2.0f, 17.0f, 0f));

    boardManager.PlaceObj(console, new Vector3(7.0f, 3.0f, 0f));
    boardManager.PlaceObj(resourceStation, new Vector3(-12f, 13f, 0f));
    boardManager.PlaceObj(healthStation, new Vector3(7.0f, 22f, 0f));

    boardManager.PlaceObj(oxygenStationBrian, new Vector3(19f, 18f, 0f));
    boardManager.PlaceObj(oxygenStationPreben, new Vector3(26f, 18f, 0f));
    boardManager.PlaceObj(oxygenStationCory, new Vector3(26f, 9f, 0f));
    boardManager.PlaceObj(oxygenStationJoseph, new Vector3(20f, 9f, 0f));

    timeLeft = 90f;

    player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
    pc = player.GetComponent<PlayerController>();

    boardManager.GenerateBoard(wall, floor, WallUpper, WallLower);

    enemySpawnCooldown = 0f;

    oxygenStationTotal = 4;

    audio = GetComponent<AudioSource>();
  }

  public void killOxygenStation() {
    if (oxygenStationTotal > 1) {
      oxygenStationTotal--;
    } else {
      GameOver();
    }
  }

  public void GameOver() {
    Debug.Log("Game Over");
    SceneManager.LoadScene(Scenes.GAME_OVER);
  }

  void WinGame() {
    Debug.Log("You win!");
    SceneManager.LoadScene(Scenes.GAME_WIN);
  }

  void Awake() {
    boardManager = GetComponent<BoardManager>();
    StartGame();
  }

  void Update() {
    timeLeft -= Time.deltaTime;

    if (timeLeft <= 0f) {
      WinGame();
    }

    if (enemySpawnCooldown >= 0) {
      enemySpawnCooldown -= Time.deltaTime;
    }

    if (timeLeft <= 80f) {
      if (Mathf.FloorToInt(timeLeft) % 8 == 0 && enemySpawnCooldown < 0) {
        SpawnEnemies(Random.Range(1, 3), (BoardManager.Locations)Random.Range(0,3));
        enemySpawnCooldown = 8f;
      }
    }
  }

  void SpawnEnemies(int total, BoardManager.Locations location) {
    //enemies.Clear();

    for (int i = 0; i < total; i++) {
      SpawnEnemy(location);
    }

    switch (location) {
      case BoardManager.Locations.Cockpit:
        audio.PlayOneShot(enemySpawnVent, 0.7F);
        break;
      case BoardManager.Locations.Oxygen:
        audio.PlayOneShot(enemySpawnPot, 0.7F);
        break;
      case BoardManager.Locations.Items:
        audio.PlayOneShot(enemySpawnGlass, 0.7F);
        break;
      case BoardManager.Locations.Health:
        audio.PlayOneShot(enemySpawnVent, 0.7F);
        break;
      default:
        audio.PlayOneShot(enemySpawnVent, 0.7F);
        break;
    }
  }

  void SpawnEnemy(BoardManager.Locations location) {
    Vector3 position;

    float minX = 0f;
    float maxX = 0f;
    float minY = 0f;
    float maxY = 0f;

    switch (location) {
      case BoardManager.Locations.Cockpit:
        Debug.Log("Cockpit");
        minX = 1;
        maxX = 13;
        minY = 1;
        maxY = 12;
        break;
      case BoardManager.Locations.Oxygen:
        Debug.Log("Oxygen");
        minX = 16;
        maxX = 28;
        minY = 9;
        maxY = 19;
        break;
      case BoardManager.Locations.Items:
        Debug.Log("Items");
        minX = -14;
        maxX = -2;
        minY = 9;
        maxY = 19;
        break;
      case BoardManager.Locations.Health:
        Debug.Log("Health");
        minX = 0;
        maxX = 14;
        minY = 15;
        maxY = 28;
        break;
      default:
        // in case of a default, just go to the cockpit
        minX = 1;
        maxX = 13;
        minY = 1;
        maxY = 12;
        break;
    }

    position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0f);
    Debug.Log("position: " + position + " location: " + location);
    Instantiate(enemy, position, Quaternion.identity);
  }
}
