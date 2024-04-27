using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {
    public int maxHealth = 1000;
    public WeaponController weaponController;

    public int Health { get; protected set; } = 0;
    public bool IsDead { get; protected set; } = false;

    protected virtual void Awake() {

    }

    protected virtual void Start() {
        Health = maxHealth;
    }

    protected virtual void Update() {

    }

    protected virtual void OnEnable() {

    }

    public virtual void Hit(int Damage) {
        if (IsDead) {
            return;
        }

        Health -= Damage;

        if (Health <= 0) {
            IsDead = true;
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
