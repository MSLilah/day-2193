using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartText : MonoBehaviour {

  public GameObject pressToStart;

  public void FadeNextText() {
    pressToStart.GetComponent<StartGame>().StartAnimation();
  }
}
