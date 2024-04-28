using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipFusionController : EntityController {
    public OVRInput.Button fireButton = OVRInput.Button.PrimaryIndexTrigger & OVRInput.Button.SecondaryIndexTrigger;

    public WeaponController mainWeaponController;
    public WeaponController chargeWeaponController;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        // mainWeaponController를 weaponController로 설정
        weaponController = mainWeaponController;

        // weaponController를 player로 설정
        weaponController.isEnemyWeapon = false;
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();

        if (OVRInput.Get(fireButton)) {
            Fire();
        }
    }

    public override void Heal(int health) {
        SpaceshipController leftSpaceship = GameManager.instance.leftSpaceship.GetComponent<SpaceshipController>();
        SpaceshipController rightSpaceship = GameManager.instance.rightSpaceship.GetComponent<SpaceshipController>();

        leftSpaceship.Heal(health);
        rightSpaceship.Heal(health);
    }

    public override void Hit(int damage) {
        SpaceshipController leftSpaceship = GameManager.instance.leftSpaceship.GetComponent<SpaceshipController>();
        SpaceshipController rightSpaceship = GameManager.instance.rightSpaceship.GetComponent<SpaceshipController>();

        leftSpaceship.Hit(damage);
        rightSpaceship.Hit(damage);
    }

    public override void Die() {
        // SpaceshipFusionController는 죽지 않음
        // leftSpaceship과 rightSpaceship을 죽임

        SpaceshipController leftSpaceship = GameManager.instance.leftSpaceship.GetComponent<SpaceshipController>();
        SpaceshipController rightSpaceship = GameManager.instance.rightSpaceship.GetComponent<SpaceshipController>();

        leftSpaceship.Die();
        rightSpaceship.Die();
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") || other.CompareTag("Bullet") || other.CompareTag("Level")) {
            // 적, 총알, 레벨 물체와 충돌하면 대미지를 입음
            Hit(1);
        }
    }
}
