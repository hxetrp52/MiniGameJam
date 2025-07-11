using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixerGroups mixerGroups;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void SetMasterVolume(float volume)
    {
        mixerGroups.audioMixer.SetFloat(mixerGroups.masterVolumeParam, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void SetBGMVolume(float volume)
    {
        mixerGroups.audioMixer.SetFloat(mixerGroups.bgmVolumeParam, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        mixerGroups.audioMixer.SetFloat(mixerGroups.sfxVolumeParam, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public float GetMasterVolume() => PlayerPrefs.GetFloat("MasterVolume", 1f);
    public float GetBGMVolume() => PlayerPrefs.GetFloat("BGMVolume", 1f);
    public float GetSFXVolume() => PlayerPrefs.GetFloat("SFXVolume", 1f);
}
