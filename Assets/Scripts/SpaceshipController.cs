using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : EntityController {
    public bool isPrimaryController = true;
    public OVRInput.Button fireButton = OVRInput.Button.PrimaryIndexTrigger;

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

    protected override void OnEnable() {
        base.OnEnable();

        if (IsDead) {
            gameObject.SetActive(false);
        }
    }

    public override void Die() {
        // Destroy가 아닌 SetActive(false)를 사용
        IsDead = true;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") || other.CompareTag("Bullet") || other.CompareTag("Level")) {
            // 적, 총알, 레벨 물체와 충돌하면 대미지를 입음
            Hit(1);
        }
    }
}
