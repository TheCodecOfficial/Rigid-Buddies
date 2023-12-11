using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallParticles : MonoBehaviour
{
    private ParticleSystem ps;
    private Ball ball;
    void Start()
    {
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        ball = GetComponent<Ball>();
    }

    void Update()
    {
        var emission = ps.emission;
        emission.rateOverDistance = ball.vel.magnitude * 0.25f;

        // set material intensity
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material.SetFloat("_Intensity", ball.vel.magnitude * 0.25f);
    }
}
