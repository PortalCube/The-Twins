using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController {
    // pivot을 원점이 아닌 다른 지점으로 세팅하기 위해
    // Enemy에서 hitbox 게임 오브젝트를 분리

    public bool trackPlayerRotation = true;
    public bool trackPlayerPosition = true;

    public float minFireRate = 0.8f;
    public float maxFireRate = 1.2f;

    Transform hitboxTransform;
    Animator animator;

    float fireRate = 0f;
    float time = 0f;

    protected override void Awake() {
        base.Awake();
        weaponController = GetComponent<WeaponController>();
        animator = GetComponent<Animator>();
    }

    protected override void Start() {
        base.Start();
        animator.SetFloat("IdleOffset", Random.Range(0f, 0.5f));

        hitboxTransform = transform.Find("Hitbox").transform;

        SetFireRate();

        if (weaponController) {
            weaponController.isEnemyWeapon = true;
        }
    }

    protected override void Update() {
        base.Update();

        // trackPlayerRotation이 활성화 된 경우, hitboxTransform은 계속해서 target 방향으로 rotation 하도록 설정
        if (trackPlayerRotation) {
            GameObject target = FindNearstSpaceship();

            // Spaceship이 존재하는 경우, Spaceship을 바라보도록 설정
            if (target) {
                hitboxTransform.LookAt(target.transform);
            }
        }

        CheckFire();
    }

    protected override void OnEnable() {
        base.OnEnable();

        animator.SetTrigger("Spawn");

        // trackPlayerPosition이 활성화 된 경우, parent를 player로 설정
        if (trackPlayerPosition) {
            // GameManager.instance가 초기화 되지 않은 상태에서 이 코드가 실행되는 경우가 있어서, 임시방편으로 GameObject.Find("Player")로 수정
            // transform.SetParent(GameManager.instance.player.transform);
            transform.SetParent(GameObject.Find("Player").transform);
        }
    }

    public override void Die() {
        Destroy(gameObject);
        animator.SetTrigger("Destroy");

        base.Die();
    }

    // fireRate를 랜덤으로 설정하는 함수
    void SetFireRate() {
        if (maxFireRate <= 0f) {
            fireRate = -1f;
        }

        fireRate = Random.Range(minFireRate, maxFireRate);
    }

    // Fire를 시도하는 함수
    void CheckFire() {
        if (fireRate < 0f) {
            return;
        }

        time += Time.deltaTime;

        if (time < fireRate) {
            return;
        }

        animator.SetTrigger("Fire");

        // 발사 애니메이션이 진행되는 동안 Fire가 발생하지 않도록 지정
        fireRate = 999f;
        time = 0f;
    }

    // Fire 로직 재정의
    // Fire() 함수는 Animation Event로 실행
    public override void Fire() {
        SetFireRate();
        time = 0f;

        base.Fire();
    }

    // hitboxTransform에서 가장 가까운 Spaceship을 반환하는 함수
    GameObject FindNearstSpaceship() {
        GameObject[] spaceships = { GameManager.instance.leftSpaceship, GameManager.instance.rightSpaceship };
        GameObject nearestSpaceship = null;
        float minDistance = float.MaxValue;

        // 모든 Spaceship을 순회
        foreach (GameObject spaceship in spaceships) {
            // 파괴되어 비활성화된 Spaceship은 제외
            if (spaceship.activeSelf == false) {
                continue;
            }

            float distance = Vector3.Distance(hitboxTransform.position, spaceship.transform.position);

            if (distance < minDistance) {
                minDistance = distance;
                nearestSpaceship = spaceship;
            }
        }

        return nearestSpaceship;
    }
}
