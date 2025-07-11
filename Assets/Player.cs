using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KartPlayer : MonoBehaviour
{
    [Header("이동 및 회전")]
    public float moveSpeed = 5f;
    public float turnSpeed = 100f;

    [Header("드리프트 설정")]
    public float driftTurnMultiplier = 1.5f;   // 드리프트 시 회전 배수
    public float driftTiltAngle = 15f;         // 드리프트 시 시각적 기울기 각도
    public float tiltLerpSpeed = 5f;

    [Header("부스터 설정")]
    public float maxBoosterCharge = 2f;        // 부스터 충전 최대치 (2)
    public float normalChargeRate = 0.2f;      // 일반 충전 속도 (초당)
    public float driftChargeRate = 0.8f;       // 드리프트 시 충전 속도 (초당)
    public float driftPenaltyRate = 0.6f;      // 드리프트 중 벽 충돌 시 60% 손실
    public int maxBoosterStock = 2;             // 최대 부스터 저장 개수
    public float boostDuration = 3f;            // 부스터 지속 시간
    public float boostMultiplier = 1.5f;        // 부스터 시 속도 배수

    [Header("게임 시작 딜레이")]
    public float startDelay = 3f;               // 게임 시작 후 대기 시간(초)

    public float boosterCharge = 0f;          // 현재 부스터 충전량
    public int boosterStock = 0;               // 현재 부스터 저장 개수

    private bool isBoosting = false;
    private float boostTimer = 0f;

    private float currentRotation = 0f;
    private float visualTilt = 0f;

    private float startTimer;


    public Slider boosterSlider;
    public GameObject[] Booster;
    public TextMeshProUGUI countText;

    void Start()
    {
        startTimer = startDelay;
    }

    void FixedUpdate()
    {
        if (startTimer > 0f)
        {
            startTimer -= Time.deltaTime;
            return; // 딜레이 시간 동안 조작 불가
        }

        // 입력 감지
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool isDrifting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool isForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool isTurning = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ||
                         Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        // 부스터 충전
        if (isForward && !isDrifting)
        {
            boosterCharge += normalChargeRate * Time.deltaTime;
        }
        else if (isForward && isDrifting && isTurning)
        {
            boosterCharge += driftChargeRate * Time.deltaTime;
        }

        // 부스터 최대 충전 시 부스터 스톡 1 증가, 충전량 초기화
        if (boosterCharge >= maxBoosterCharge)
        {
            boosterCharge = 0f;
            if (boosterStock < maxBoosterStock)
            {
                boosterStock += 1;
                Debug.Log("Booster Stock +1! 현재 보유: " + boosterStock);
            }
        }

        if (!isForward && isBoosting)
        {
            boostTimer = 0;
            isBoosting = false;
        }

        // 부스터 지속 및 종료 처리
        float currentSpeed = moveSpeed;
        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;
            currentSpeed = moveSpeed * boostMultiplier;

            if (boostTimer <= 0f)
            {
                isBoosting = false;
                Debug.Log("부스터 종료");
            }
        }

        // 전진 중에만 조향 (그립 주행)
        if (v != 0f)
        {
            float turnAmount = h * turnSpeed * (isDrifting ? driftTurnMultiplier : 1f) * Time.deltaTime;
            currentRotation += turnAmount;
        }

        // 회전 적용
        transform.rotation = Quaternion.Euler(0f, 0f, -currentRotation);

        // 이동 적용
        Vector3 forward = transform.up;
        if (v > 0f)
            transform.position += forward * currentSpeed * Time.deltaTime;
        else if (v < 0f)
            transform.position -= forward * currentSpeed * Time.deltaTime;

        // 시각적 기울기 (드리프트 시 몸 기울임)
        float targetTilt = isDrifting ? -h * driftTiltAngle : 0f;
        visualTilt = Mathf.Lerp(visualTilt, targetTilt, tiltLerpSpeed * Time.deltaTime);

        // 시각적 기울기 적용 (자식 오브젝트에 적용)
        if (transform.childCount > 0)
        {
            Transform body = transform.GetChild(0);
            body.localRotation = Quaternion.Euler(0f, 0f, visualTilt);
        }
    }

    void Update()
    {
        Booster[0].SetActive(false);
        Booster[1].SetActive(false);
        boosterSlider.value = boosterCharge;

        int a = (int)startTimer;

        countText.text = a.ToString();
        if(startTimer <= 0f)
        {
            countText.text = "";
        }

        for (int i = 0; i < boosterStock; i++)
        {
            Booster[i].SetActive(true);
        }

        if (startTimer > 0f)
            return;  // 대기 시간 동안 부스터 사용 제한

        if (!isBoosting && boosterStock > 0 && Input.GetKeyDown(KeyCode.LeftControl))
        {
            isBoosting = true;
            boostTimer = boostDuration;
            boosterStock -= 1;
            Debug.Log("부스터 사용! 남은 부스터: " + boosterStock);
        }
    }

    // 드리프트 중 벽과 충돌 시 부스터 손실
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isDrifting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (isDrifting && collision.gameObject.CompareTag("Wall"))
        {
            boosterCharge *= (1f - driftPenaltyRate);
            Debug.Log("벽과 충돌! 부스터 일부 손실, 현재 부스터 차지: " + boosterCharge);
        }
    }
}
