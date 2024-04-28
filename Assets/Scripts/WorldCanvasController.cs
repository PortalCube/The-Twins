using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasController : MonoBehaviour {

    public GameObject trackingObject;
    public float speed = 2f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (trackingObject) {

            // trackingObject의 y축 회전값을 smooth하게 이동
            transform.rotation = Quaternion.Slerp(transform.rotation, trackingObject.transform.rotation, Time.deltaTime * speed);
        }
    }
}
