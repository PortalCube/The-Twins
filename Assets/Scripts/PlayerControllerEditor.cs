using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor {
    void OnSceneGUI() {
        PlayerController playerController = (PlayerController)target;

        if (playerController.waypoints == null || playerController.waypoints.Length == 0) {
            return;
        }

        if (Application.isPlaying == false) {
            Handles.color = Color.yellow;
            Handles.DrawLine(playerController.transform.position, playerController.waypoints[0].position);

        }


        for (int i = 0; i < playerController.waypoints.Length; i++) {
            Waypoint waypoint = playerController.waypoints[i];

            Handles.color = Color.yellow;
            if (i < playerController.waypoints.Length - 1) {
                Waypoint nextWaypoint = playerController.waypoints[i + 1];
                Handles.DrawLine(waypoint.position, nextWaypoint.position);
            }

            Handles.color = Color.white;
            Handles.Label(waypoint.position + Vector3.up * 0.5f, string.Format("Waypoint {0}\n{1}\nSpeed: {2}", i + 1, waypoint.position, waypoint.speed));

            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = Handles.PositionHandle(waypoint.position, waypoint.direction);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(playerController, "Move Waypoint " + newPosition.ToString());
                waypoint.position = newPosition;
            }

            EditorGUI.BeginChangeCheck();
            Quaternion newRotation = Handles.RotationHandle(waypoint.direction, waypoint.position);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(playerController, "Rotate Waypoint " + newRotation.ToString());
                waypoint.direction = newRotation;
            }
        }
    }
}
