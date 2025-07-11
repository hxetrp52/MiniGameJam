using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public enum VolumeType { Master, BGM, SFX }
    public VolumeType type;
    private Slider slider;

    [Header("텍스트 추가")]
    public TMP_Text volumeDisplayText; // 연결 시 볼륨 텍스트 표시

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void Start()
    {
        switch (type)
        {
            case VolumeType.Master:
                slider.value = AudioManager.Instance.GetMasterVolume();
                break;
            case VolumeType.BGM:
                slider.value = AudioManager.Instance.GetBGMVolume();
                break;
            case VolumeType.SFX:
                slider.value = AudioManager.Instance.GetSFXVolume();
                break;
        }

        UpdateVolumeText(slider.value); // 초기 텍스트 표시
    }

    private void OnSliderChanged(float value)
    {
        switch (type)
        {
            case VolumeType.Master:
                AudioManager.Instance.SetMasterVolume(value);
                break;
            case VolumeType.BGM:
                AudioManager.Instance.SetBGMVolume(value);
                break;
            case VolumeType.SFX:
                AudioManager.Instance.SetSFXVolume(value);
                break;
        }

        UpdateVolumeText(value);
    }

    private void UpdateVolumeText(float volume)
    {
        if (volumeDisplayText != null)
        {
            int percent = Mathf.RoundToInt(volume * 100f);
            volumeDisplayText.text = $"{percent}%";
        }
    }
}
