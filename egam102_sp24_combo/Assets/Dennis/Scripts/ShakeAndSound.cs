using UnityEngine;

public class ShakeAndSound : MonoBehaviour
{
    public float shakeDuration = 0.2f; // 震动持续时间
    public float shakeMagnitude = 0.1f; // 震动幅度

    public AudioSource audioSource; // 音效的AudioSource组件
    public AudioClip soundEffect; // 音效

    private Vector3 originalPosition; // 图片的原始位置
    private float shakeTimer; // 震动计时器

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shake();
            PlaySoundEffect();
        }

        if (shakeTimer > 0)
        {
            transform.position = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            shakeTimer = 0f;
            transform.position = originalPosition;
        }
    }

    private void Shake()
    {
        shakeTimer = shakeDuration;
    }

    private void PlaySoundEffect()
    {
        audioSource.PlayOneShot(soundEffect);
    }
}