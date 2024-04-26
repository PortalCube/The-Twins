using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolumetricLines;

public class LaserController : MonoBehaviour {

    VolumetricLineBehavior lineController;

    public float maxDistance = 100.0f;

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
            lineController.EndPos *= hit.distance;
        } else {
            lineController.EndPos *= maxDistance;
        }

        // TODO: EndPos에 레이저 부딫히는 파티클 추가
    }
}
