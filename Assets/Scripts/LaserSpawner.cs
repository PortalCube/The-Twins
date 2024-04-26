using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolumetricLines;

public class LaserSpawner : MonoBehaviour {
    // 레이저의 방향들을 나타내는 열거형
    [Flags]
    public enum LaserDirection {
        Right = 1 << 0, // 0, X+
        Left = 1 << 1, // 1, X-
        Up = 1 << 2, // 2, Y+
        Down = 1 << 3, // 4, Y-
        Forward = 1 << 4, // 8, Z+
        Back = 1 << 5, // 16, Z-
    }

    public GameObject[] laserObjects;

    public LaserDirection direction = (LaserDirection)31; // All directions

    // Start is called before the first frame update
    void Start() {

        // 모든 레이저 오브젝트에 대해서
        for (int i = 0; i < 6; i++) {
            // index를 LaserDirection의 flag로 변환
            LaserDirection flag = (LaserDirection)(1 << i);
            GameObject laser = laserObjects[i];

            // direction에 flag가 포함되어 있다면 레이저를 활성화
            bool active = direction.HasFlag(flag);
            laser.SetActive(active);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
