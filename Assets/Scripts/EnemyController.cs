using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController {
    public GameObject parentObject;

    public Vector3 parentRotation;

    public bool trackPlayer = true;

    // Start is called before the first frame update
    protected override void Start() {
        // EntityController의 Start를 실행
        base.Start();

        // parentRotaion을 parentObject에서 가져와서 저장
        parentRotation = parentObject.transform.rotation.eulerAngles;

        // weaponController를 enemy로 설정
        if (weaponController) {
            weaponController.isEnemyWeapon = true;
        }
    }

    // Update is called once per frame
    protected override void Update() {
        // EntityController의 Update를 실행
        base.Update();

        // parentObject의 회전값을 가져와서 Entity의 회전값으로 설정
        parentObject.transform.rotation = Quaternion.Euler(parentRotation);

        // trackPlayer가 활성화 된 경우, Entity는 계속해서 target을 바라보도록 설정
        if (trackPlayer) {
            // 가장 가까운 Spaceship를 탐색
            GameObject target = FindNearstSpaceship();

            // Spaceship이 존재하는 경우, Spaceship을 바라보도록 설정
            if (target) {
                transform.LookAt(target.transform);
            }

        }


    }

    public override void Die() {
        // parentObject를 파괴
        Destroy(parentObject);

        // EntityController의 Die를 실행
        base.Die();
    }

    // 현재 GameObject에서 가장 가까운 Spaceship을 반환하는 함수
    GameObject FindNearstSpaceship() {
        // GameManager에서 Spaceship을 가져옴
        GameObject[] spaceships = { GameManager.instance.leftSpaceship, GameManager.instance.rightSpaceship };

        // 가장 가까운 Spaceship을 저장할 변수
        GameObject nearestSpaceship = null;

        // 가장 가까운 Spaceship과의 거리를 저장할 변수 - 초기값을 Max로 둔다
        float minDistance = float.MaxValue;

        // 모든 Spaceship을 순회하며 가장 가까운 Spaceship을 찾는다
        foreach (GameObject spaceship in spaceships) {

            // 파괴되어 비활성화된 Spaceship은 제외
            if (spaceship.activeSelf == false) {
                continue;
            }

            float distance = Vector3.Distance(transform.position, spaceship.transform.position);

            if (distance < minDistance) {
                minDistance = distance;
                nearestSpaceship = spaceship;
            }
        }

        // 가장 가까운 Spaceship을 반환
        return nearestSpaceship;
    }
}
