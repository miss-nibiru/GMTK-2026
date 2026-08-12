using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip water;
    [SerializeField] private AudioClip pickup;
    [SerializeField] private AudioClip rewindTime;
    [SerializeField] private AudioClip openCaseFile;
    [SerializeField] private AudioClip closeCaseFile;
    [SerializeField] private AudioClip footstep;
    [SerializeField] private AudioClip openDoor;
    [SerializeField] private AudioClip ovenDing;

    private void Awake()
    {
        if (!Instance) _instance = this;
        else 
            Destroy(gameObject);
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (!clip) return;
        sfxSource.PlayOneShot(clip, volume);
    }
    
}
