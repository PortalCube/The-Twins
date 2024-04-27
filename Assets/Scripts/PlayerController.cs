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

    // Start is called before the first frame update
    void Start() {
        Move();
    }

    // Update is called once per frame
    void Update() {

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
