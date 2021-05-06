using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip woodChopSoundEffect; //Chopping sound effect
    /// <summary>
    /// Called when worker hits the tree
    /// </summary>
    public void OnTreeHitAnimation()
    {
        animator.SetTrigger("treeHit");
        audioSource.PlayOneShot(woodChopSoundEffect);
    }
}
