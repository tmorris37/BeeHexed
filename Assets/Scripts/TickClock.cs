using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickClock : MonoBehaviour
{
    // A boolean that outputs true when the clock is "high"
    [SerializeField] public bool Pulse;

    // Sets 1 Tick to 2 seconds (4 seconds / 1 complete cycle)
    [SerializeField] public float TickLength = 2.0f;

    // Internally tracks the time since last tick pulse.
    public float InternalClock;

    /*
     *      |-----| = TickLength
     *       _____       _____  <---- "Up-Tick" (Pulse == true)
     *      |     |     |     |
     *      |     |     |     |
     * _____|     |_____|     |_____  <---- "Down-Tick" (Pulse == false)
    */

    void Start()
    {
        this.Pulse = false;
        this.InternalClock = 0.0f;
    }

    void Update()
    {
        // Every TickLength seconds, the InternalClock is decremented and the
        // Pulse is inverted. Provides desired behavior
        if (this.InternalClock >= TickLength)
        {
            this.InternalClock -= TickLength;
            this.Pulse = !this.Pulse;
        }

        // Increments the internal clock
        this.InternalClock += Time.deltaTime;
    }
}
