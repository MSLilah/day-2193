using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RestorationStationController : MonoBehaviour {

  public string name;
  public string resourceName;

  public float resourceReductionRate;
  public float resourceRestorationRate;
  public float damageFromEnemy;

  public float resourceTotal;

  private float resourceGameOverValue;
  private float resourceMaxValue;
  private GameManager gm;

  private AudioSource audio;
  public float damageCooldown;
  private float initDamageCooldown;
  public AudioClip damage;
  public float restoreCooldown;
  private float initRestoreCooldown;
  public AudioClip restore;

  public GameObject bar;
  private SpriteRenderer barSR;
  public Sprite[] barSprites;

  void Start() {
    resourceGameOverValue = 0f;
    resourceMaxValue = 100f;
    gm = GameObject.FindGameObjectWithTag(Tags.GAME_CONTROLLER).GetComponent<GameManager>();
    audio = GetComponent<AudioSource>();
    initDamageCooldown = damageCooldown;
    initRestoreCooldown = restoreCooldown;

    if (name == RestorationStations.OXYGEN_STATION) {
      barSR = bar.GetComponent<SpriteRenderer>();

      barSR.sprite = barSprites[10];
    }
  }

  void Update() {
    // Decrease the resource value
    if (!(RestorationStations.RESOURCE_STATION == name)) {
      DecreaseResourceTotal(resourceReductionRate * Time.deltaTime);
    }

    if (restoreCooldown <= initRestoreCooldown) {
      restoreCooldown += Time.deltaTime;
    }

    if (damageCooldown <= initDamageCooldown) {
      damageCooldown += Time.deltaTime;
    }

    if (name == RestorationStations.OXYGEN_STATION) {
      DrawBar();
    }
  }

  void DrawBar() {
    int barPercent = Mathf.FloorToInt(resourceTotal) / 10;
    barSR.sprite = barSprites[barPercent];
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

    if (restoreCooldown >= initRestoreCooldown) {
      audio.PlayOneShot(restore, 0.7F);
      restoreCooldown = 0f;
    }
  }

  public void DamageResource() {
    DecreaseResourceTotal(damageFromEnemy);
    if (damageCooldown >= initDamageCooldown) {
      audio.PlayOneShot(damage, 0.7F);
      damageCooldown = 0f;
    }
  }

  public string GenerateText() {
    return resourceName + ": " + Mathf.CeilToInt(resourceTotal);
  }
}
