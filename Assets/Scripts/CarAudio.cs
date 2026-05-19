using UnityEngine;

public class CarAudio : MonoBehaviour
{
    [Header("Engine")]
    [SerializeField] private AudioSource engineSource;
    [SerializeField] private AudioClip engineStartClip;

    [Header("Voiceover")]
    [SerializeField] private AudioSource voiceoverSource;
    [SerializeField] private AudioClip voiceoverClip;

    private bool engineOn;

    public void ToggleEngine()
    {
        if (engineSource == null || engineStartClip == null) return;

        // THE FIX: If the car is hidden/disabled, do nothing!
        if (!gameObject.activeInHierarchy || !engineSource.gameObject.activeInHierarchy) return;

        engineSource.enabled = true;

        if (engineOn)
        {
            engineSource.Stop();
            engineOn = false;
        }
        else
        {
            engineSource.clip = engineStartClip;
            engineSource.loop = false;
            engineSource.Play();
            engineOn = true;
        }
    }

    void Update()
    {
        if (engineOn && engineSource != null && !engineSource.isPlaying)
            engineOn = false;
    }

    public void PlayVoiceover()
    {
        if (voiceoverSource == null || voiceoverClip == null) return;

        // THE FIX: If the car is hidden/disabled, do nothing!
        if (!gameObject.activeInHierarchy || !voiceoverSource.gameObject.activeInHierarchy) return;

        voiceoverSource.enabled = true;

        if (voiceoverSource.isPlaying) return;
        voiceoverSource.clip = voiceoverClip;
        voiceoverSource.loop = false;
        voiceoverSource.Play();
    }

    public void StopEngine()
    {
        if (engineSource != null) engineSource.Stop();
        engineOn = false;
    }
}