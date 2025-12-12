using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Music Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip tacticalMapMusic;
    public AudioClip combatMusic;

    private static MusicManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                PlayMusic(mainMenuMusic);
                break;

            case "TacticalMapScene":
                PlayMusic(tacticalMapMusic);
                break;

            case "CombatScene1":
                PlayMusic(combatMusic);
                break;
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null || audioSource.clip == clip)
            return;

        StopAllCoroutines();
        StartCoroutine(CrossfadeMusic(clip));
    }

    private System.Collections.IEnumerator CrossfadeMusic(AudioClip newClip)
    {
        float fadeTime = 1.5f;
        float startVolume = audioSource.volume;

        // Fade out
        for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in
        for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, startVolume, t / fadeTime);
            yield return null;
        }
    }

}
