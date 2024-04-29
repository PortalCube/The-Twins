using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float speed = 1.0f;
    public int damage = 10;
    public float lifeTime = 5.0f;
    public bool isEnemyBullet = false;

    float time = 0.0f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (GameManager.instance.IsGameOver) {
            // 게임 오버 - 총알 제거
            Destroy(gameObject);
        }

        time += Time.deltaTime;

        Vector3 newPosition = transform.position + transform.forward * speed * Time.deltaTime;

        // Raycast를 이용해서 충돌 검사
        if (isEnemyBullet == false) {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, speed * Time.deltaTime);
            foreach (var hit in hits) {
                // 충돌한 대상이 Enemy인 경우
                if (hit.collider.CompareTag("Enemy")) {
                    // 제대로 충돌하도록 위치 조정
                    newPosition = hit.point;
                }
            }
        }

        transform.position = newPosition;

        if (time > lifeTime) {
            Destroy(gameObject);
        }
    }

    // 총알이 충돌했을 때
    void OnTriggerEnter(Collider other) {
        // 총알에 총알이 맞았거나, Trigger에 맞은 경우
        if (other.CompareTag("Bullet") || other.CompareTag("Trigger")) {
            // 무시
            return;
        }

        // 적에 총알이 맞았을 때
        if (other.CompareTag("Enemy")) {
            if (isEnemyBullet) {
                // 적의 총알이 적에게 충돌한 경우, 무시
                return;
            } else {
                // 적의 EnemyController에서 Hit() 함수를 호출
                EnemyController controller = other.gameObject.GetComponentInParent<EnemyController>();
                controller.Hit(damage);
            }
        }

        if (other.CompareTag("Spaceship") && isEnemyBullet == false) {
            // 플레이어의 총알이 플레이어에게 충돌한 경우, 무시
            return;
        }

        if (other.CompareTag("Player")) {
            return;
        }

        // 적의 총알이 맞는 경우는, spaceshipcontroller에서 처리

        // Bullet 게임 오브젝트 제거
        Destroy(gameObject);
    }
}
