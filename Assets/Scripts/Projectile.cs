using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 100f)
            Destroy(gameObject);
        transform.Translate(0, Speed * Time.deltaTime, 0, Space.Self);
    }
}
