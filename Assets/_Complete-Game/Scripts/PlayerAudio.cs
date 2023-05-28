using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] _attackClips;
    [SerializeField] private AudioClip[] _takeFoodClips;

    private Player _player;
    private AudioSource _audioSource;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _player.AttackEventHandler += OnAttack;
        _player.TakeFoodEventHandler += OnTakeFood;
    }

    private void OnDisable()
    {
        _player.AttackEventHandler -= OnAttack;
        _player.TakeFoodEventHandler -= OnTakeFood;
    }

    private void OnAttack() => PlayRandomClip(_attackClips);
    private void OnTakeFood(int value) => PlayRandomClip(_takeFoodClips);

    private void PlayRandomClip(AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        _audioSource.clip = clips[randomIndex];
        _audioSource.Play();
    }
}
