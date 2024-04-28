using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : EntityController {
    public bool isPrimaryController = true;
    public OVRInput.Button fireButton = OVRInput.Button.PrimaryIndexTrigger;

    public WeaponController mainWeaponController;
    public WeaponController chargeWeaponController;

    public GameObject hitEffect;
    public GameObject destroyEffect;

    public GameObject modelObject;

    public float invincibleTime = 1f;
    public float blinkInterval = 0.1f;
    bool isInvincible = false;
    float invincibleTimer = 0f;

    protected override void Start() {
        base.Start();

        // mainWeaponController를 weaponController로 설정
        weaponController = mainWeaponController;

        // weaponController를 player로 설정
        weaponController.isEnemyWeapon = false;
    }

    protected override void Update() {
        base.Update();

        if (OVRInput.Get(fireButton)) {
            Fire();
        }

        if (invincibleTimer > 0) {
            invincibleTimer -= Time.deltaTime;

            // blinkInterval마다 모델을 껐다 켰다 하도록 설정
            int blinkCount = (int)(invincibleTimer / blinkInterval);
            modelObject.SetActive(blinkCount % 2 == 0);

            if (invincibleTimer <= 0) {
                // 무적 종료
                SetInvincible(false);
            }
        }
    }


    protected override void OnEnable() {
        base.OnEnable();

        // 무적 상태 초기화
        SetInvincible(false);

        if (IsDead) {
            gameObject.SetActive(false);
        }
    }

    public override void Hit(int damage) {
        base.Hit(damage);

        if (IsDead) {
            // 사망 이펙트가 재생되므로 재생하지 않고 종료
            return;
        }

        // 무적 발동
        SetInvincible(true);

        Instantiate(hitEffect, transform.position, transform.rotation, transform.parent);
    }

    public override void Die() {
        // Destroy가 아닌 SetActive(false)를 사용
        IsDead = true;
        Transform playerTransform = GameManager.instance.player.transform;
        Instantiate(destroyEffect, transform.position, transform.rotation, playerTransform);
        gameObject.SetActive(false);
    }

    void SetInvincible(bool value) {
        isInvincible = value;
        if (isInvincible) {
            invincibleTimer = invincibleTime;
        } else {
            invincibleTimer = 0;
            modelObject.SetActive(true);
        }
    }


    void OnTriggerEnter(Collider other) {
        // 무적 상태면 무시
        if (isInvincible) {
            return;
        }

        if (other.CompareTag("Enemy") || other.CompareTag("Bullet") || other.CompareTag("Level")) {
            // 적, 총알, 레벨 물체와 충돌하면 대미지를 입음
            Hit(1);
        }
    }
}
