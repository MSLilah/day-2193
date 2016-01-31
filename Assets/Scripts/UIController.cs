using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {
  public Text[] uiDisplays;
  public Text healthDisplay;

  private GameObject[] restorationStations;
  private PlayerController pc;

  void Start() {
    restorationStations = GameObject.FindGameObjectsWithTag(Tags.RESTORATION_STATION);
    pc = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<PlayerController>();
  }

  void Update() {
    healthDisplay.text = "Health: " + Mathf.CeilToInt(pc.health);
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
