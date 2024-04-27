using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

    ParticleSystem[] particles;

    // Start is called before the first frame update
    void Start() {
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {
        bool isEveryParticleDestroyed = true;

        foreach (var particle in particles) {
            if (particle == null) {
                continue;
            }

            if (particle.IsAlive() == false) {
                Destroy(particle.gameObject);
            } else {
                isEveryParticleDestroyed = false;
            }
        }

        if (isEveryParticleDestroyed) {
            Destroy(gameObject);
        }
    }
}
