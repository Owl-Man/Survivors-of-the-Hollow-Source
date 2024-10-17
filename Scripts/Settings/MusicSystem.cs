using System.Collections;
using DataBase;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class MusicSystem : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicSource;
    [SerializeField] private AudioClip attackStateSource;
    [SerializeField] private bool isInOrder, isAttackFunc;

    private AudioSource _audioSource, _audioSourceAttack;

    private int _previousMusicID = -1, _chosenMusic = 0;

    public static MusicSystem Instance;

    private void Awake() => Instance = this;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        
        if (isAttackFunc)
        {
            _audioSourceAttack = gameObject.AddComponent<AudioSource>();
            _audioSourceAttack.clip = attackStateSource;
        }
        
        UpdateState();
    }

    public void UpdateState()
    {
        if (!DB.Access.IsMusicOn)
        {
            StopAllCoroutines();
            _audioSource.enabled = false;
            if (isAttackFunc) _audioSourceAttack.enabled = false;
        }
        else
        {
            _audioSource.enabled = true;
            if (isAttackFunc) _audioSourceAttack.enabled = true;
            StartCoroutine(MusicPlaying());
        }
    }

    private IEnumerator MusicPlaying()
    {
        if (musicSource.Length != 1 && !isInOrder)
        {
            _chosenMusic = Random.Range(0, musicSource.Length);

            while (_chosenMusic == _previousMusicID) _chosenMusic = Random.Range(0, musicSource.Length);
        }

        _audioSource.clip = musicSource[_chosenMusic];
        Play(_audioSource);
        
        _previousMusicID = _chosenMusic;
        yield return new WaitForSeconds(musicSource[_chosenMusic].length);
        if (isInOrder)
        {
            if (_chosenMusic != musicSource.Length) _chosenMusic++;
            else _chosenMusic = 0;
        }
        StartCoroutine(MusicPlaying());
    }

    public void SwitchAudio(bool isForAttack)
    {
        if (_audioSource.enabled == false) return;
        
        StopAllCoroutines();
        
        if (isForAttack)
        {
            Stop(_audioSource);
            Play(_audioSourceAttack);
        }
        else
        {
            StartCoroutine(MusicPlaying());
            Stop(_audioSourceAttack);
        }
    }
    
    public void Play(AudioSource audioSource) => StartCoroutine(StartPlaying(audioSource));

    private IEnumerator StartPlaying(AudioSource audioSource)
    {
        audioSource.volume = 0;
        audioSource.Play();
        
        while (audioSource.volume != 1)
        {
            audioSource.volume += 0.002f;
            yield return null;
        }
    }

    public void Stop(AudioSource audioSource) => StartCoroutine(StopPlaying(audioSource));

    private IEnumerator StopPlaying(AudioSource audioSource)
    {
        while (audioSource.volume != 0)
        {
            audioSource.volume -= 0.002f;
            yield return null;
        }
        
        audioSource.Stop();
    }
}