using System;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

[Serializable]
public class Waypoint {
    public Vector3 position;
    public Quaternion direction = Quaternion.identity;
    public float speed = 1f;
}

public class PlayerController : MonoBehaviour {
    public Waypoint[] waypoints;

    public float rotateDuration = 1.5f;

    // Fusion Mode
    public bool isFusionMode = false;
    public float activeFusionDistance = 0.2f;
    public float deactiveFusionDistance = 0.3f;

    // Charge Mode
    public bool isChargeMode = false;
    public OVRInput.Button chargeButton = OVRInput.Button.One;
    public float chargeTime = 4f;
    public int energy = 0;
    public int maxEnergy = 1000;

    float chargeTimer = 0f;

    // Start is called before the first frame update
    void Start() {
        Move();
    }

    // Update is called once per frame
    void Update() {
        // Fusion Mode 로직
        if (CheckFusionMode()) {
            MoveFusionSpaceship();
        }

        // 차지 버튼을 누르면 에너지 사용
        if (OVRInput.GetDown(chargeButton)) {
            UseEnergy();
        }

        // Charge Mode 로직
        if (isChargeMode) {
            chargeTimer -= Time.deltaTime;

            if (chargeTimer <= 0) {
                SetChargeActive(false);
            }
        }

    }

    void MoveFusionSpaceship() {
        GameObject fusionSpaceship = GameManager.instance.fusionSpaceship;
        GameObject leftController = GameManager.instance.leftController;
        GameObject rightController = GameManager.instance.rightController;

        // 두 컨트롤러의 position의 중간 지점으로 적용
        Vector3 position = (leftController.transform.position + rightController.transform.position) / 2;
        fusionSpaceship.transform.position = position;

        // 두 컨트롤러의 rotation의 중간 지점으로 적용
        fusionSpaceship.transform.rotation = Quaternion.Slerp(leftController.transform.rotation, rightController.transform.rotation, 0.5f);
    }

    // Fusion 상태를 갱신하고 상태를 반환
    bool CheckFusionMode() {
        GameObject leftController = GameManager.instance.leftController;
        GameObject rightController = GameManager.instance.rightController;
        SpaceshipController leftSpaceship = GameManager.instance.leftSpaceship.GetComponent<SpaceshipController>();
        SpaceshipController rightSpaceship = GameManager.instance.rightSpaceship.GetComponent<SpaceshipController>();

        float distance = Vector3.Distance(leftController.transform.position, rightController.transform.position);

        bool bothAlive = !leftSpaceship.IsDead && !rightSpaceship.IsDead;

        if (isFusionMode) {
            if (distance > deactiveFusionDistance || bothAlive == false) {
                // 컨트롤러가 비활성화 거리에 도달하거나, 둘 중 하나라도 죽은 경우
                UpdateFusionMode(false);
            }
        } else if (distance < activeFusionDistance && bothAlive) {
            // 컨트롤러가 활성화 거리에 도달했고, 둘 다 살아있는 경우
            UpdateFusionMode(true);
        }

        return isFusionMode;
    }

    void UpdateFusionMode(bool value) {
        GameObject leftSpaceship = GameManager.instance.leftSpaceship;
        GameObject rightSpaceship = GameManager.instance.rightSpaceship;
        GameObject fusionSpaceship = GameManager.instance.fusionSpaceship;

        isFusionMode = value;

        if (value) {
            MoveFusionSpaceship();
            leftSpaceship.SetActive(false);
            rightSpaceship.SetActive(false);
            fusionSpaceship.SetActive(true);
        } else {
            leftSpaceship.SetActive(true);
            rightSpaceship.SetActive(true);
            fusionSpaceship.SetActive(false);
        }
    }

    void OnDrawGizmosSelected() {
        float size = 0.1f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, size);
        foreach (var waypoint in waypoints) {
            Gizmos.DrawSphere(waypoint.position, size);
        }
    }

    void Move(int index = 0) {
        // 배열 인덱스를 벗어난 경우
        if (waypoints.Length <= index) {
            return;
        }

        float distance = Vector3.Distance(transform.position, waypoints[index].position);
        float duration = distance / waypoints[index].speed;

        Tween.Position(transform, waypoints[index].position, duration, 0f, Tween.EaseLinear, Tween.LoopType.None, null, () => Move(index + 1));
        Tween.Rotation(transform, waypoints[index].direction, rotateDuration, 0f, Tween.EaseOut);
    }

    public void ChargeEnergy(int value) {
        energy += value;

        if (energy > maxEnergy) {
            energy = maxEnergy;
        }
    }

    public bool UseEnergy() {
        if (energy >= maxEnergy) {
            energy = 0;

            // Charge Mode 활성화
            SetChargeActive(true);
            return true;
        } else {
            return false;
        }
    }

    public void SetChargeActive(bool value) {
        isChargeMode = value;

        // ship의 weapon을 chargeWeapon으로 변경
        SpaceshipController leftSpaceship = GameManager.instance.leftSpaceship.GetComponent<SpaceshipController>();
        SpaceshipController rightSpaceship = GameManager.instance.rightSpaceship.GetComponent<SpaceshipController>();
        SpaceshipFusionController fusionSpaceship = GameManager.instance.fusionSpaceship.GetComponent<SpaceshipFusionController>();

        if (value) {
            chargeTimer = chargeTime;

            leftSpaceship.weaponController = leftSpaceship.chargeWeaponController;
            rightSpaceship.weaponController = rightSpaceship.chargeWeaponController;
            fusionSpaceship.weaponController = fusionSpaceship.chargeWeaponController;
        } else {
            leftSpaceship.weaponController = leftSpaceship.mainWeaponController;
            rightSpaceship.weaponController = rightSpaceship.mainWeaponController;
            fusionSpaceship.weaponController = fusionSpaceship.mainWeaponController;
        }
    }
}
