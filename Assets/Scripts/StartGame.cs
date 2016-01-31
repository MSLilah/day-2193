using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGame : MonoBehaviour {
  private Animator anim;
  private bool canPlay = false;
  
  
  void Start() {
    anim = gameObject.GetComponent<Animator>();
  }

	void Update() {
    if (canPlay && Input.GetKey(KeyCode.Space)) {
      SceneManager.LoadScene(Scenes.MAIN_GAME);
    }
  }

  public void StartAnimation() {
    anim.SetTrigger("Play");
  }

  public void AllowStart() {
    print("Allowing start");
    canPlay = true;
  }
}
