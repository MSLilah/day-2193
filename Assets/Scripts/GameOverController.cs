using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverController : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
	  if (Input.GetKeyDown(KeyCode.R)) {
      SceneManager.LoadScene(0);
    }
	}
}
