using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance = null;
    [SerializeField] AudioClip audio1;
    [SerializeField] AudioClip audio2;
    [SerializeField] AudioClip audio3;
    private AudioClip audio;
    private AudioSource audioSource;
    Player player;
    int choise = 0;

    void Awake()
    {
        SetUpSingletone();
        player = FindObjectOfType<Player>();
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        transform.position = new Vector2(8f, 2f);
        PlayMusic();
    }
    private void OnLevelWasLoaded(int level)
    {
        player = FindObjectOfType<Player>();
        transform.position = new Vector2(8f, 2f);
    }

    private void Update()
    {
        if (player)
        {
            transform.position = player.transform.position;
        }
    }

    private void PlayMusic()
    {
        choise = Random.Range(0, 3);
        switch (choise)
        {
            case 0: 
                {
                    audio = audio1;
                    break; 
                }
            case 1:
                {
                    audio = audio2;
                    break; 
                }
            case 2:
                {
                    audio = audio3;
                    break; 
                }
        }
        audioSource.clip = audio;
        audioSource.volume = 0.05f;
        audioSource.Play();
        StartCoroutine(MusicAwait());
    }

    private IEnumerator MusicAwait()
    {
        yield return new WaitForSeconds(audio1.length+2);
        PlayMusic();
    }

    private void SetUpSingletone()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Resume()
    {
        audioSource.Play();
    }
}
