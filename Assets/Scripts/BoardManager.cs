using UnityEngine;
using System.Collections;

public class BoardManager : MonoBehaviour {

  private Transform boardTransform;

  private GameObject wall;
  private GameObject floor;

  void DrawBoard(int x, int y) {
    GameObject instObj;

    int columns = 15;
    int rows = 15;

    for (int i = 0; i < columns; i++) {
      for (int j = 0; j < rows; j++) {
        if (i == 0 || j == 0 || i == columns-1 || j == rows-1) {
          instObj = wall;
          //Debug.Log("Wall: " + i + ", " + j);
        } else {
          instObj = floor;
          //Debug.Log("Floor: " + i + ", " + j);
        }

        GameObject instance = Instantiate(instObj,
            new Vector3((i+x), (j+y), 0f), Quaternion.identity) as GameObject;
        instance.transform.SetParent(boardTransform);
      }
    }
  }

  public void GenerateBoard(GameObject w, GameObject f) {
    boardTransform = new GameObject("Board").transform;
    wall = w;
    floor = f;

    // Cockpit
    DrawBoard(0, 0);
    // Item Room
    DrawBoard(-15, 7);
    // People/Oxygen Room
    DrawBoard(15, 7);
    // Health Station
    DrawBoard(0, 15);
  }
}
