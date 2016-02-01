using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeController : MonoBehaviour {
  public void StartGame() {
    SceneManager.LoadScene(Scenes.MAIN_GAME);
  }
}
