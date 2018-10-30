using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour {

    bool isPlaying = false;
    public ParticleSystem ps;

    public void ToggleParticle()
    {
        //var em = ps.emission;
        if (isPlaying)
            ps.Stop();
        else
            ps.Play();

        isPlaying = !isPlaying;
    }
}
