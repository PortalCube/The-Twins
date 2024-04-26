using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {
    public int maxHealth = 1000;
    public WeaponController weaponController;

    int health = 0;

    // Start is called before the first frame update
    protected virtual void Start() {
        health = maxHealth;
    }

    // Update is called once per frame
    protected virtual void Update() {

    }

    public virtual void Hit(int Damage) {
        health -= Damage;

        if (health <= 0) {
            Die();
        }
    }

    public virtual void Die() {
        Destroy(gameObject);
    }

    public virtual void Fire() {
        weaponController.Shoot();
    }
}
