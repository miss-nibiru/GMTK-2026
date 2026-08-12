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
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        if (musicSource == null || music == null) return;
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayWater() => PlaySound(water);
    public void PlayPickup() => PlaySound(pickup);
    public void PlayRewindTime() => PlaySound(rewindTime);
    public void PlayOpenCaseFile() => PlaySound(openCaseFile);
    public void PlayCloseCaseFile() => PlaySound(closeCaseFile);
    public void PlayFootstep() => PlaySound(footstep, 0.4f);
    public void PlayOpenDoor() => PlaySound(openDoor);
    public void PlayOvenDing() => PlaySound(ovenDing);

    private void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (!clip || !sfxSource) return;
        sfxSource.PlayOneShot(clip, volume);
    }
    
    private void OnDestroy()
    {
        if (_instance == this) _instance = null;
    }
    
}