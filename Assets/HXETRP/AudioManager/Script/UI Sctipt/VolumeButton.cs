using UnityEngine;
using TMPro;

public class VolumeButton : MonoBehaviour
{
    public enum VolumeType { Master, BGM, SFX }
    public VolumeType type;
    public float step = 0.1f;

    [Header("선택 사항")]
    public TMP_Text volumeDisplayText; // 여러 버튼이 공유 가능!

    public void IncreaseVolume()
    {
        float vol = Mathf.Clamp(GetVolume() + step, 0.0001f, 1f);
        SetVolume(vol);
        UpdateVolumeText(); // 여기서 최신 상태를 직접 가져옴
    }

    public void DecreaseVolume()
    {
        float vol = Mathf.Clamp(GetVolume() - step, 0.0001f, 1f);
        SetVolume(vol);
        UpdateVolumeText();
    }

    private float GetVolume()
    {
        return type switch
        {
            VolumeType.Master => AudioManager.Instance.GetMasterVolume(),
            VolumeType.BGM => AudioManager.Instance.GetBGMVolume(),
            VolumeType.SFX => AudioManager.Instance.GetSFXVolume(),
            _ => 1f,
        };
    }

    private void SetVolume(float value)
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
    }

    private void UpdateVolumeText()
    {
        if (volumeDisplayText != null)
        {
            float volume = GetVolume(); // 최신 값 가져오기
            int percent = Mathf.RoundToInt(volume * 100f);
            volumeDisplayText.text = $"{percent}%";
        }
    }
}
