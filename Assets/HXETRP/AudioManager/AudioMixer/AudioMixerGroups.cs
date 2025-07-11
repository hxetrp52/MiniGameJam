using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioMixerGroups", menuName = "Audio/AudioMixerGroups")]
public class AudioMixerGroups : ScriptableObject
{
    public AudioMixer audioMixer;

    [Header("Exposed Parameters")]
    public string masterVolumeParam = "MasterVolume";
    public string bgmVolumeParam = "BGMVolume";
    public string sfxVolumeParam = "SFXVolume";
}
