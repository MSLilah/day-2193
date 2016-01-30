using UnityEngine;
using System.Collections;

public class BoardManager : MonoBehaviour {

  private Transform boardTransform;

  private GameObject wall;
  private GameObject floor;

  void DrawBoard(int columns, int rows, int x, int y) {
    GameObject instObj;

    for (int i = x; i < columns; i++) {
      for (int j = y; j < rows; j++) {
        if (i == x || j == y || i == columns-1 || j == rows-1) {
          instObj = wall;
          //Debug.Log("Wall: " + i + ", " + j);
        } else {
          instObj = floor;
          //Debug.Log("Floor: " + i + ", " + j);
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

    // Cockpit
    DrawBoard(15, 15, 0, 0);

    // Item Room
    DrawBoard(15, 15, -15, 0);
    // People/Oxygen Room
    // Health Station
  }
}
