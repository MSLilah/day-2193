using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InstructionText : MonoBehaviour {

	public void StartAnimation() {
    gameObject.GetComponent<Animator>().SetTrigger("Play");
  }

  public void StartGame() {
    SceneManager.LoadScene(Scenes.MAIN_GAME);
  }
}
