using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ActiveRagdollFeetController : MonoBehaviour
{
    [SerializeField] private GameObject[] feet;
    public GameObject[] Feet { get { return this.feet; } }
    [Header("SFX")]
    [SerializeField] private AudioClip[] clips;
    private AudioSource audioSource;
    [SerializeField] private GameObject particle;
    public GameObject Particle { get { return this.particle; } }

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(audioSource.clip);
    }
}