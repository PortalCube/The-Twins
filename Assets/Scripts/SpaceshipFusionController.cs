using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipFusionController : EntityController {
    public OVRInput.Button fireButton = OVRInput.Button.PrimaryIndexTrigger & OVRInput.Button.SecondaryIndexTrigger;

    public WeaponController mainWeaponController;
    public WeaponController chargeWeaponController;

    public GameObject fusionEffect;
    public GameObject hitEffect;

    public GameObject modelObject;

    public AudioSource audioSource;
    public AudioClip hitSound;

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

        Transform playerTransform = GameManager.instance.player.transform;

        Instantiate(fusionEffect, transform.position, transform.rotation, playerTransform);
    }

    protected override void OnDisable() {
        base.OnDisable();

        Transform playerTransform = GameManager.instance.player.transform;

        Instantiate(fusionEffect, transform.position, transform.rotation, playerTransform);
    }

    public override void Heal(int health) {
        SpaceshipController leftSpaceship = GameManager.instance.leftSpaceship.GetComponent<SpaceshipController>();
        SpaceshipController rightSpaceship = GameManager.instance.rightSpaceship.GetComponent<SpaceshipController>();

        leftSpaceship.Heal(health);
        rightSpaceship.Heal(health);
    }

    public override void Hit(int damage) {
        // 무적 상태면 무시
        if (isInvincible) {
            return;
        }

        SpaceshipController leftSpaceship = GameManager.instance.leftSpaceship.GetComponent<SpaceshipController>();
        SpaceshipController rightSpaceship = GameManager.instance.rightSpaceship.GetComponent<SpaceshipController>();

        leftSpaceship.Hit(damage);
        rightSpaceship.Hit(damage);

        // 공격을 맞고도 둘 다 살아 있다면
        if (!leftSpaceship.IsDead && !rightSpaceship.IsDead) {
            // 무적 발동
            SetInvincible(true);
            Instantiate(hitEffect, transform.position, transform.rotation, transform.parent);
            audioSource.PlayOneShot(hitSound);
        }
    }

    public override void Die() {
        // SpaceshipFusionController는 죽지 않음
        // leftSpaceship과 rightSpaceship을 죽임

        SpaceshipController leftSpaceship = GameManager.instance.leftSpaceship.GetComponent<SpaceshipController>();
        SpaceshipController rightSpaceship = GameManager.instance.rightSpaceship.GetComponent<SpaceshipController>();

        leftSpaceship.Die();
        rightSpaceship.Die();
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
        if (other.CompareTag("Enemy") || other.CompareTag("Level")) {
            // 적, 레벨 물체와 충돌하면 대미지를 입음
            Hit(1);
        } else if (other.CompareTag("Bullet")) {
            BulletController bulletController = other.GetComponent<BulletController>();
            if (bulletController.isEnemyBullet) {
                // 적이 발사한 총알에 충돌하면 대미지를 입음
                Hit(1);
            }
        }
    }
}
