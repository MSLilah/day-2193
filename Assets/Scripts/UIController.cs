using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {
  public Text[] uiDisplays;
  public Text healthDisplay;
  public Text timeDisplay;

  private GameObject[] restorationStations;
  private PlayerController pc;
  private GameManager gm;

  void Start() {
    restorationStations = GameObject.FindGameObjectsWithTag(Tags.RESTORATION_STATION);
    pc = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<PlayerController>();
    gm = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameManager>();
  }

  void Update() {
    healthDisplay.text = "Health: " + Mathf.CeilToInt(pc.health);

    timeDisplay.text = "Time: " + gm.timeLeft.ToString("F2");

    int i = 0;
    foreach (GameObject obj in restorationStations) {
      RestorationStationController rsc = obj.GetComponent<RestorationStationController>();
      if (rsc.name != RestorationStations.HEALTH_STATION) {
        uiDisplays[i].text = rsc.GenerateText();
        i++;
      }
    }
  }
}
