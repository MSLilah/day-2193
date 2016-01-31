using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RestorationStationController : MonoBehaviour {

	public string name;
  public string resourceName;

  public float resourceReductionRate;
  public float resourceRestorationRate;

  public float resourceTotal;

  private float resourceGameOverValue;
  private float resourceMaxValue;
  private GameManager gm;

  void Start() {
    resourceGameOverValue = 0f;
    resourceMaxValue = 100f;
    gm = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameManager>();
  }

  void Update() {
    // Decrease the resource value
    DecreaseResourceTotal(resourceReductionRate * Time.deltaTime);
  }

  public void DecreaseResourceTotal(float reduction) {
    resourceTotal -= reduction;
    if (resourceTotal <= resourceGameOverValue) {
      gm.GameOver();
    }
  }

  public void RestoreResource() {
    float increase = resourceRestorationRate * Time.deltaTime;
    resourceTotal = Mathf.Min(resourceTotal + increase, resourceMaxValue);
  }

  public void DamageResource() {
    DecreaseResourceTotal(3 * resourceReductionRate * Time.deltaTime);
  }

  public string GenerateText() {
    return resourceName + ": " + Mathf.CeilToInt(resourceTotal);
  }
}
