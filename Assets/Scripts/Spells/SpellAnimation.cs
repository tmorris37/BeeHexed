using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAnimation : MonoBehaviour {
    public float duration = 5f;
    public void End() {
        Destroy(gameObject);
    }

    void Start() {
        EndAnimation();
    }

    private IEnumerator EndAnimation() {
        yield return new WaitForSeconds(duration);
        End();
    }

}
