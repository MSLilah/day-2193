using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameStartController : MonoBehaviour {
  // Update is called once per frame
  void Update () {
    if (Input.GetKeyDown(KeyCode.Space)) {
      SceneManager.LoadScene(Scenes.MAIN_GAME);
    }
  }
}
