using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

public class EnemyController : EntityController {
    // pivot을 원점이 아닌 다른 지점으로 세팅하기 위해
    // Enemy에서 hitbox 게임 오브젝트를 분리

    public bool trackPlayerRotation = true;
    public bool trackPlayerPosition = true;

    public float minFireRate = 0.8f;
    public float maxFireRate = 1.2f;

    public int minEnergy = 15;
    public int maxEnergy = 30;

    public GameObject fireEffect;
    public GameObject destroyEffect;

    public AudioClip destroySound;

    Transform hitboxTransform;
    Animator animator;
    EntityAnimationController entityAnimationController;
    AudioSource audioSource; // Ambient sound

    Quaternion lastRotation;

    float fireRate = 0f;
    float time = 0f;

    protected override void Awake() {
        base.Awake();
        weaponController = GetComponent<WeaponController>();
        animator = GetComponent<Animator>();

        lastRotation = transform.localRotation;
    }

    protected override void Start() {
        base.Start();

        // 엔티티 애니메이터 시작
        entityAnimationController = GetComponent<EntityAnimationController>();
        entityAnimationController.StartAwakeAnimation();
        EnemyEnable();

        hitboxTransform = transform.Find("Hitbox");

        if (hitboxTransform == null) {
            hitboxTransform = transform;
        }

        SetFireRate();

        if (weaponController) {
            weaponController.isEnemyWeapon = true;
        }
    }

    protected override void Update() {
        if (IsDead) {
            return;
        }

        base.Update();

        // trackPlayerRotation이 활성화 된 경우, hitboxTransform은 계속해서 target 방향으로 rotation 하도록 설정
        if (trackPlayerRotation) {
            GameObject target = FindNearstSpaceship();
            UpdateHitboxRotation();

            // Spaceship이 존재하는 경우, Spaceship을 바라보도록 설정
            if (target) {
                // target.transform을 smooth하게 바라보도록 지정
                Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - hitboxTransform.position);
                hitboxTransform.rotation = Quaternion.Slerp(hitboxTransform.rotation, targetRotation, Time.deltaTime * 4f);
            } else {
                // Spaceship이 존재하지 않는 경우, 플레이어 오브젝트를 바라보도록 설정
                GameObject player = GameManager.instance.player;
                hitboxTransform.LookAt(player.transform);
            }
        }

        if (Vector3.Distance(hitboxTransform.position, GameManager.instance.player.transform.position) < 6f) {
            // 플레이어와의 거리가 10f 이하인 경우, 발사
            CheckFire();
        }
    }

    // hitboxTransform의 rotation이 Enemy transform의 회전에 영향을 받지 않도록 함
    void UpdateHitboxRotation() {
        hitboxTransform.localRotation = Quaternion.Inverse(transform.localRotation) * lastRotation * hitboxTransform.localRotation;
        lastRotation = transform.localRotation;
    }

    public void EnemyEnable() {
        if (animator) {

            animator.SetTrigger("Spawn");
        }

        // trackPlayerPosition이 활성화 된 경우, parent를 player로 설정
        if (trackPlayerPosition) {
            // GameManager.instance가 초기화 되지 않은 상태에서 이 코드가 실행되는 경우가 있어서, 임시방편으로 GameObject.Find("Player")로 수정
            // transform.SetParent(GameManager.instance.player.transform);
            transform.SetParent(GameObject.Find("Player").transform);
        }
    }

    // Destroy(gameObject) 대신 Destroy 애니메이션을 실행
    public override void Die() {
        // Destroy 애니메이션 실행
        IsDead = true;

        if (animator) {
            animator.SetTrigger("Destroy");
        } else {
            Death();
        }

        if (audioSource) {
            audioSource.Stop();
        }

        if (destroySound) {
            AudioSource audioSource = GameManager.instance.player.GetComponent<AudioSource>();
            audioSource.PlayOneShot(destroySound);
        }

        entityAnimationController.StopAnimation();

        // energy를 랜덤하게 추가
        PlayerController playerController = GameManager.instance.player.GetComponent<PlayerController>();
        playerController.ChargeEnergy(Random.Range(minEnergy, maxEnergy));

        // 파괴 이펙트 생성
        Transform playerTransform = GameManager.instance.player.transform;

        Instantiate(destroyEffect, hitboxTransform.position, hitboxTransform.rotation, playerTransform);
    }

    // Die() 함수는 Animation Event로 실행
    // 여기서 Destroy(gameObject)를 실행
    public void Death() {
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

        if (fireEffect) {
            Transform firePoint = weaponController.firePoint.transform;
            Instantiate(fireEffect, firePoint.position, firePoint.rotation, firePoint);
        }

        base.Fire();
    }

    // hitboxTransform에서 가장 가까운 Spaceship을 반환하는 함수
    GameObject FindNearstSpaceship() {
        GameObject[] spaceships = { GameManager.instance.leftSpaceship, GameManager.instance.rightSpaceship, GameManager.instance.fusionSpaceship };
        GameObject nearestSpaceship = null;
        float minDistance = float.MaxValue;

        // 모든 Spaceship을 순회
        foreach (GameObject spaceship in spaceships) {
            // 비활성화된 Spaceship은 제외
            if (spaceship.activeSelf == false) {
                continue;
            }

            if (hitboxTransform == null) {
                Debug.LogError("Hitbox Transform is null, " + gameObject.name);
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
