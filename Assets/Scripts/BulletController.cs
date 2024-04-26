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
        time += Time.deltaTime;

        transform.position += transform.forward * speed * Time.deltaTime;

        if (time > lifeTime) {
            DestroySelf();
        }
    }

    // 총알이 충돌했을 때
    void OnTriggerEnter(Collider other) {
        // 총알에 총알이 맞았을 때
        if (other.CompareTag("Bullet")) {
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
                EnemyController controller = other.gameObject.GetComponent<EnemyController>();
                controller.Hit(damage);
            }
        }

        // 플레이어에 총알이 맞았을 때
        if (other.CompareTag("Spaceship")) {
            if (isEnemyBullet == false) {
                // 플레이어의 총알이 플레이어에게 충돌한 경우, 무시
                return;
            } else {
                // 플레이어의 SpaceshipController에서 Hit() 함수를 호출
                SpaceshipController controller = other.gameObject.GetComponent<SpaceshipController>();
                controller.Hit(damage);
            }
        }

        // Bullet 게임 오브젝트 제거
        DestroySelf();
    }

    void DestroySelf() {
        // TODO: 폭발 효과 추가
        Destroy(gameObject);
    }
}
