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

    public bool isFusionMode = false;

    public float activeFusionDistance = 0.2f;
    public float deactiveFusionDistance = 0.3f;

    // Start is called before the first frame update
    void Start() {
        Move();
    }

    // Update is called once per frame
    void Update() {
        if (CheckFusionMode()) {
            MoveFusionSpaceship();
        }
    }

    void MoveFusionSpaceship() {
        GameObject fusionSpaceship = GameManager.instance.fusionSpaceship;
        GameObject leftController = GameManager.instance.leftController;
        GameObject rightController = GameManager.instance.rightController;

        Vector3 position = (leftController.transform.position + rightController.transform.position) / 2;
        fusionSpaceship.transform.position = position;


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
                UpdateFusionMode(false);
            }
        } else if (distance < activeFusionDistance && bothAlive) {
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
}
