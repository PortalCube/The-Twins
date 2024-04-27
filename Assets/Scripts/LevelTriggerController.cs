using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTriggerController : MonoBehaviour {

    public GameObject[] linkedObjects;

    void Awake() {
        foreach (GameObject linkedObject in linkedObjects) {
            if (linkedObject == null) {
                Debug.LogError("Linked Object is null");
                continue;
            }

            linkedObject.SetActive(false);
        }
    }

    void Start() {

    }

    void Update() {

    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            // Trigger를 Player의 위치로 이동
            // Trigger의 자식 Entity들이 활성화 된 후 Player의 자식이 되었을 때, Player가 트리거를 어느 위치에서 충돌하던 동일한 위치로 이동하기 위함
            Debug.Log(transform.position);
            Debug.Log(other.transform.position);
            transform.position = other.transform.position;

            foreach (GameObject linkedObject in linkedObjects) {
                if (linkedObject == null) {
                    Debug.LogError("Linked Object is null");
                    continue;
                }

                linkedObject.SetActive(true);

                EnemyController enemyController = linkedObject.GetComponent<EnemyController>();
                if (enemyController) {
                    enemyController.EnemyEnable();
                }
            }

            Destroy(gameObject);
        }
    }

    void OnDrawGizmos() {
        Collider collider = GetComponent<Collider>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
    }
}
