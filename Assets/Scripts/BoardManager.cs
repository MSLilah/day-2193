using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

  private Transform boardTransform;

  private GameObject wall;
  private GameObject floor;
  private GameObject WallUpper;
  private GameObject WallLower;

  public enum Locations {Cockpit, Oxygen, Items, Health, Unknown};

  public static Locations DetermineLocation(Vector2 location) {
    if ((location.x >= 0 && location.x < 15) &&
        (location.y >= 0 && location.y < 15)) {
      return Locations.Cockpit;
    } else if ((location.x >= -15 && location.x < 0) &&
               (location.y >= 7 && location.y < 23)) {
      return Locations.Items;
    } else if ((location.x >= 15 && location.x < 30) &&
               (location.y >= 7 && location.y < 23)) {
      return Locations.Oxygen;
    } else if ((location.x >= 0 && location.x < 15) &&
               (location.y >= 15 && location.y < 30)) {
      return Locations.Health;
    } else {
      return Locations.Unknown;
    }
  }

  public void PlaceObj(GameObject obj, Vector3 location) {
    Instantiate(obj, location, Quaternion.identity);
  }

  void DrawBoard(int x, int y, List<Vector2> doors) {
    GameObject instObj;

    int columns = 15;
    int rows = 15;

    float xLoc;
    float yLoc;

    for (int i = 0; i < columns; i++) {
      for (int j = 0; j < rows; j++) {
        xLoc = i+x;
        yLoc = j+y;

        Vector2 vec = new Vector2(xLoc, yLoc);

        if (doors.Contains(vec)) {
          instObj = floor;
        } else if (j == rows-1 && i != 0 && i != columns-1) {
          instObj = WallUpper;
        } else if (j == rows-2 && i != 0 && i != columns-1) {
          instObj = WallLower;
        } else if (i == 0 || j == 0 || i == columns-1 || j == rows-1) {
          instObj = wall;
          //Debug.Log("Wall: " + i + ", " + j);
        } else {
          instObj = floor;
          //Debug.Log("Floor: " + i + ", " + j);
        }

        GameObject instance = Instantiate(instObj,
            new Vector3(xLoc, yLoc, 0f), Quaternion.identity) as GameObject;
        instance.transform.SetParent(boardTransform);
      }
    }
  }


  public void GenerateBoard(GameObject w, GameObject f, GameObject wu, GameObject wl) {
    boardTransform = new GameObject("Board").transform;
    wall = w;
    floor = f;
    WallUpper = wu;
    WallLower = wl;

    List<Vector2> doors = new List<Vector2>();

    // Cockpit
    doors.Add(new Vector2(0f, 10f)); // To Items
    doors.Add(new Vector2(0f, 9f));

    doors.Add(new Vector2(7f, 14f)); // To Health
    doors.Add(new Vector2(8f, 14f));
    doors.Add(new Vector2(7f, 13f));
    doors.Add(new Vector2(8f, 13f));

    doors.Add(new Vector2(14f, 9f)); // To Oxygen
    doors.Add(new Vector2(14f, 10f));
    DrawBoard(0, 0, doors);
    // Item Room
    doors.Add(new Vector2(-1f, 10f)); // To Cockpit
    doors.Add(new Vector2(-1f, 9f));
    DrawBoard(-15, 7, doors);
    // People/Oxygen Room
    doors.Add(new Vector2(15f, 9f)); // To Cockpit
    doors.Add(new Vector2(15f, 10f));
    DrawBoard(15, 7, doors);
    // Health Station
    doors.Add(new Vector2(7f, 15f)); // To Health
    doors.Add(new Vector2(8f, 15f));
    DrawBoard(0, 15, doors);
  }
}
