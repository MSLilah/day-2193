using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
  private Animator anim;
  private bool canPlay = false;
  
  public GameObject instructions;
  public GameObject title;

  void Start() {
    anim = gameObject.GetComponent<Animator>();
  }

	void Update() {
    if (canPlay && Input.GetKey(KeyCode.Space)) {
      anim.SetTrigger("FadeOut");
      title.GetComponent<Animator>().SetTrigger("FadeOut");
    }
  }

  public void StartAnimation() {
    anim.SetTrigger("Play");
  }

  public void AllowStart() {
    canPlay = true;
  }

  public void StartInstructions() {
    instructions.GetComponent<InstructionText>().StartAnimation();
  }
}
