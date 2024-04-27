using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipFusionController : EntityController {
    public OVRInput.Button fireButton = OVRInput.Button.PrimaryIndexTrigger & OVRInput.Button.SecondaryIndexTrigger;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

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

    public override void Die() {
        // Destroy가 아닌 SetActive(false)를 사용
        gameObject.SetActive(false);
    }
}
