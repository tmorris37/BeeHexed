using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DurationSpell : MonoBehaviour
{
  // how long the spell lasts
  private float spellDuration;
  // how long the spell has been in effect for
  private float currTime;
  // set duration and begin the effect
  void Start()
  {
    spellDuration = GetDuration();
    StartEffect();
  }
  // increase time framerate-independently each frame until duration is met, then end the effect
  void Update() {
    if (currTime <= spellDuration) {
      currTime += Time.deltaTime;
    } else {
      EndEffect();
      Destroy(gameObject);
    }
  }

  // Begin the spell's effect
  public abstract void StartEffect();
  // End the spell's effect
  public abstract void EndEffect();
  // get the spell's duration (for subclass to provide)
  public abstract float GetDuration();
}
