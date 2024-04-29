using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolumetricLines;

public class LaserController : MonoBehaviour {

    public float maxDistance = 100.0f;
    public GameObject sparkEffect;

    VolumetricLineBehavior lineController;

    // Start is called before the first frame update
    void Start() {
        lineController = GetComponent<VolumetricLineBehavior>();
        lineController.StartPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        lineController.EndPos = Vector3.forward;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, LayerMask.GetMask("Level"))) {
            // 벽에 맞음
            lineController.EndPos *= hit.distance;

            // sparkEffect의 위치를 충돌 지점으로, 방향을 충돌 지점의 법선 벡터로 설정
            sparkEffect.transform.position = hit.point;
            sparkEffect.transform.rotation = Quaternion.LookRotation(hit.normal);

            // sparkEffect 활성화
            sparkEffect.SetActive(true);

        } else {
            lineController.EndPos *= maxDistance;

            // sparkEffect 비활성화
            sparkEffect.SetActive(false);
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance)) {
            if (hit.collider.CompareTag("Spaceship")) {
                SpaceshipController controller = hit.collider.GetComponent<SpaceshipController>();
                SpaceshipFusionController fusionController = hit.collider.GetComponent<SpaceshipFusionController>();

                if (controller) {
                    controller.Hit(1);
                } else {
                    fusionController.Hit(1);
                }
            }
        }
    }
}
