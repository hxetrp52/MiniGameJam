using UnityEngine;

public class KartPlayer : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 100f;
    public float driftTurnMultiplier = 1.5f;   // 드리프트 시 회전 속도 배수
    public float driftTiltAngle = 15f;         // 드리프트 시 기울기 각도
    public float tiltLerpSpeed = 5f;

    private float currentRotation = 0f;
    private float visualTilt = 0f;             // 시각적 기울기용

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool isDrifting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        float turnMultiplier = isDrifting ? driftTurnMultiplier : 1f;

        // 전진 중일 때만 조향 허용
        if (v != 0f)
        {
            float turnAmount = h * turnSpeed * turnMultiplier * Time.deltaTime;
            currentRotation += turnAmount;
        }

        // 회전 적용
        transform.rotation = Quaternion.Euler(0f, 0f, -currentRotation);

        // 이동 방향 적용
        Vector3 forward = transform.up;
        if (v > 0f)
            transform.position += forward * moveSpeed * Time.deltaTime;
        else if (v < 0f)
            transform.position -= forward * moveSpeed * Time.deltaTime;

        // 시각적 기울기 (드리프트 감성 효과)
        float targetTilt = isDrifting ? -h * driftTiltAngle : 0f;
        visualTilt = Mathf.Lerp(visualTilt, targetTilt, tiltLerpSpeed * Time.deltaTime);

        // 실제 시각적 회전은 자식 오브젝트에 적용 (있을 경우)
        if (transform.childCount > 0)
        {
            Transform body = transform.GetChild(0); // 첫 번째 자식이 비주얼
            body.localRotation = Quaternion.Euler(0f, 0f, visualTilt);
        }
    }
}
