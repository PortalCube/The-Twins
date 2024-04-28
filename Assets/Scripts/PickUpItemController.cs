using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemController : MonoBehaviour {
    public bool heal = false;
    public int energy = 0;

    public GameObject destroyEffect;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Spaceship") {
            EntityController entity = other.gameObject.GetComponent<EntityController>();

            if (heal) {
                entity.Heal(entity.maxHealth);
            }

            if (energy > 0) {
                GameManager.instance.player.GetComponent<PlayerController>().ChargeEnergy(energy);
            }

            Instantiate(destroyEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
