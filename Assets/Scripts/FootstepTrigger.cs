using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class FootstepTrigger : MonoBehaviour
{
    [SerializeField] private FirstPersonController playerController;

    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip jumpSound; // the sound played when character leaves the ground.
    [SerializeField] private AudioClip landSound; // the sound played when character touches back on ground.

    private void OnTriggerEnter(Collider other)
    {
        if (footstepSounds != null && footstepSounds.Length > 0) playerController.SetFootstepSounds(footstepSounds);
        if(jumpSound != null) playerController.SetJumpSound(jumpSound);
        if(landSound != null) playerController.SetLandSound(landSound);

    }
}
