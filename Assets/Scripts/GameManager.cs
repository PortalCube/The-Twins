using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject player;
    public GameObject leftSpaceship;
    public GameObject rightSpaceship;
    public GameObject fusionSpaceship;
    public GameObject leftController;
    public GameObject rightController;

    public GameObject[] leftHealths;
    public GameObject[] rightHealths;

    public Image energyGauge;
    public TMP_Text[] energyTexts;

    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    public float energyDisplaySpeed = 2f;

    public bool IsGameOver { get; private set; } = false;

    // UI 표시용으로만 쓰이는 energy 값
    float energyDisplayValue = 0;

    // Singleton 초기화
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        UIUpdateLeftHealth();
        UIUpdateRightHealth();

        if (player.GetComponent<PlayerController>().isChargeMode) {
            UITrackCharge();
        } else {
            UITrackEnergy();
        }
    }

    public void Revive() {
        SpaceshipController leftSpaceshipController = leftSpaceship.GetComponent<SpaceshipController>();
        SpaceshipController rightSpaceshipController = rightSpaceship.GetComponent<SpaceshipController>();
        PlayerController playerController = GameManager.instance.player.GetComponent<PlayerController>();

        bool active = playerController.isFusionMode == false;

        leftSpaceshipController.Revive(active);
        rightSpaceshipController.Revive(active);

    }

    public void UIUpdateLeftHealth() {
        int value = leftSpaceship.GetComponent<SpaceshipController>().Health;

        foreach (var healthWrapper in leftHealths) {
            if (healthWrapper.activeSelf == false) {
                continue;
            }

            for (int i = 0; i < 3; i++) {
                GameObject heartObject = healthWrapper.transform.Find("Heart " + (i + 1)).gameObject;
                Image heartImage = heartObject.GetComponent<Image>();

                if (i < value) {
                    heartImage.sprite = fullHeartSprite;
                } else {
                    heartImage.sprite = emptyHeartSprite;
                }
            }
        }
    }

    public void UIUpdateRightHealth() {
        int value = rightSpaceship.GetComponent<SpaceshipController>().Health;

        foreach (var healthWrapper in rightHealths) {
            if (healthWrapper.activeSelf == false) {
                continue;
            }

            for (int i = 0; i < 3; i++) {
                GameObject heartObject = healthWrapper.transform.Find("Heart " + (i + 1)).gameObject;
                Image heartImage = heartObject.GetComponent<Image>();

                if (i < value) {
                    heartImage.sprite = fullHeartSprite;
                } else {
                    heartImage.sprite = emptyHeartSprite;
                }
            }
        }
    }

    void UITrackEnergy() {
        PlayerController playerController = player.GetComponent<PlayerController>();
        float energy = playerController.energy;
        float maxEnergy = playerController.maxEnergy;

        // energyDisplayValue를 energy 값으로 smooth하게 변경
        energyDisplayValue = Mathf.Lerp(energyDisplayValue, energy, Time.deltaTime * energyDisplaySpeed);

        // gauge나 percentage는 energyDisplayValue를 기준으로 표시
        energyGauge.fillAmount = energyDisplayValue / maxEnergy;
        string message = string.Format("{0:0.0}%", energyDisplayValue / maxEnergy * 100);

        // 만약, 차지샷이 준비되었다면 텍스트를 "CHARGED"로 변경
        if (energy == maxEnergy) {
            message = "CHARGED";
            energyTexts[0].color = new Color(0.086f, 0.792f, 0f);
        }

        foreach (var energyText in energyTexts) {
            energyText.text = message;
        }

    }

    void UITrackCharge() {
        PlayerController playerController = player.GetComponent<PlayerController>();
        float percentage = playerController.chargeTimer / playerController.chargeTime;

        // energyDisplayValue를 현재 차지샷 상태로 즉시 변경
        energyDisplayValue = playerController.maxEnergy * percentage;

        energyGauge.fillAmount = percentage;
        string message = string.Format("{0:0.0}%", percentage * 100);
        energyTexts[0].color = Color.black;

        foreach (var energyText in energyTexts) {
            energyText.text = message;
        }
    }

    public void GameOver() {
        IsGameOver = true;

        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine() {
        // 3초 후 재시작
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene(0);
    }
}
