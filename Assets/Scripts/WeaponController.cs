using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public GameObject bulletPrefab;
    public GameObject firePoint;

    public float fireRate = 0.1f;
    public float bulletSpeed = 10.0f;
    public int bulletDamage = 10;
    public float bulletLifeTime = 4.0f;
    public int bulletCount = 1;
    public float bulletAngleSpread = 0.0f;
    public float bulletSpeedSpread = 0.0f;

    public bool isEnemyWeapon = false;

    float time = 0.0f;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        // 쿨타임 갱신
        if (time < fireRate) {
            time += Time.deltaTime;
        }
    }

    // 무기를 발사하는 함수 - 발사 성공 여부를 반환
    public bool Shoot() {

        // 아직 Cooldown이 끝나지 않았다면 발사 실패
        if (time < fireRate) {
            return false;
        }

        // Cooldown 시작
        time = 0f;

        for (int i = 0; i < bulletCount; i++) {
            // bulletAngleSpread에 따라 angle을 생성
            float x = Random.Range(-bulletAngleSpread, bulletAngleSpread);
            float y = Random.Range(-bulletAngleSpread, bulletAngleSpread);
            float z = Random.Range(-bulletAngleSpread, bulletAngleSpread);

            // bulletSpeedSpread에 따라 speed를 생성
            float speed = bulletSpeed + Random.Range(-bulletSpeedSpread, bulletSpeedSpread);

            // 총알 생성
            GameObject bulletObject = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);

            // 총알 회전
            bulletObject.transform.Rotate(x, y, z);

            // 총알 설정
            BulletController bulletController = bulletObject.GetComponent<BulletController>();
            bulletController.speed = speed;
            bulletController.damage = bulletDamage;
            bulletController.lifeTime = bulletLifeTime;
            bulletController.isEnemyBullet = isEnemyWeapon;
        }

        // 발사 성공
        return true;
    }
}