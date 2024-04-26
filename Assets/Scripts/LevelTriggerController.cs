using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTriggerController : MonoBehaviour {

    public GameObject[] linkedObjects;

    // Start is called before the first frame update
    void Start() {
        foreach (GameObject linkedObject in linkedObjects) {
            linkedObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            foreach (GameObject linkedObject in linkedObjects) {
                linkedObject.SetActive(true);
            }
        }
    }

    void OnDrawGizmos() {
        Collider collider = GetComponent<Collider>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
    }
}
