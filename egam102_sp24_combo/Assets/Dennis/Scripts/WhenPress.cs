using UnityEngine;

public class ShakeAndSound : MonoBehaviour
{
    public float shakeDuration = 0.2f; // �𶯳���ʱ��
    public float shakeMagnitude = 0.1f; // �𶯷���

    public AudioSource audioSource; // ��Ч��AudioSource���
    public AudioClip soundEffect; // ��Ч

    private Vector3 originalPosition; // ͼƬ��ԭʼλ��
    private float shakeTimer; // �𶯼�ʱ��

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