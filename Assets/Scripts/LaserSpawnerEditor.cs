using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LaserSpawner))]
public class LaserSpawnerEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        LaserSpawner laserSpawner = (LaserSpawner)target;

        if (GUI.changed == false) {
            return;
        }

        // 모든 레이저 오브젝트에 대해서
        for (int i = 0; i < 6; i++) {
            // index를 LaserDirection의 flag로 변환
            LaserSpawner.LaserDirection flag = (LaserSpawner.LaserDirection)(1 << i);
            GameObject laser = laserSpawner.laserObjects[i];

            // direction에 flag가 포함되어 있다면 레이저를 활성화
            bool active = laserSpawner.direction.HasFlag(flag);
            laser.SetActive(active);
        }

    }
}
