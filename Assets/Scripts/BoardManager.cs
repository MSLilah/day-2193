using UnityEngine;
using System.Collections;

public class BoardManager : MonoBehaviour {

  private Transform boardTransform;
  private int columns;
  private int rows;

  private GameObject wall;
  private GameObject floor;

  void DrawBoard() {
    GameObject instObj;
    for (int i = 0; i < columns; i++) {
      for (int j = 0; j < rows; j++) {
        if ((i > 0 && j > 0) || (i < columns && j < rows)) {
          instObj = wall;
        } else {
          instObj = floor;
        }

        GameObject instance = Instantiate(instObj,
            new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
        instance.transform.SetParent(boardTransform);
      }
    }
  }

  public void GenerateBoard(GameObject w, GameObject f) {
    boardTransform = new GameObject("Board").transform;
    wall = w;
    floor = f;

    DrawBoard();
  }
}
