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

    protected virtual void OnDisable() {

    }

    public virtual void Heal(int health) {
        Health += health;

        if (Health > maxHealth) {
            Health = maxHealth;
        }
    }

    public virtual void Revive(bool active = true) {
        Health = maxHealth;
        IsDead = false;
        if (active) {
            gameObject.SetActive(true);
        }
    }

    public virtual void Hit(int damage) {
        if (IsDead) {
            return;
        }

        Health -= damage;

        if (Health <= 0) {
            Die();
        }
    }

    public virtual void Die() {
        IsDead = true;
        Destroy(gameObject);
    }

    public virtual void Fire() {
        weaponController.Shoot();
    }
}
