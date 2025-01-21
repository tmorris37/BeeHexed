using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(EndAnimation());
    }

    private IEnumerator EndAnimation() {
        yield return new WaitForSeconds(0.4f);
        End();
    }

    public void End() {
        Destroy(gameObject);
    }
}
