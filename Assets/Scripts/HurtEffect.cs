using System.Collections;
using UnityEngine;

public class HurtEffect : MonoBehaviour
{
    private const float SHAKE_DEFORM_COEF = .2f;

    [SerializeField] private bool isTechnicalTesting; //only for testing purposes

    private bool displayEffect = false;

    [SerializeField] private float shakeIntensity;
    [SerializeField] private float shakeDecay;
    private float currentShakeIntensity;
    private Vector3 originalPos;
    private Quaternion originalRot;

	[SerializeField] private Texture hurtTexture;
    private float alpha = 1f;

    private AudioScript audioPlayer;
    [SerializeField] AudioClip[] hurtSound = new AudioClip[0];

    private void Start()
    {
        audioPlayer = GetComponent<AudioScript>();
    }

    void Update()
    {
        if(isTechnicalTesting)
            Hit();
    }

    IEnumerator ApplyEffect()
    {
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            Shake();
            yield return null;
        }
        ResetEffect();
    }

    private void ResetEffect()
    {
        currentShakeIntensity = shakeIntensity;
        displayEffect = false;
        alpha = 1f;
    }

    private void Shake()
    {
        if (currentShakeIntensity > 0)
        {
            transform.position = originalPos + Random.insideUnitSphere * shakeIntensity;
            transform.rotation = new Quaternion(
               GetDeformedRotation(originalRot.x),
               GetDeformedRotation(originalRot.y),
               GetDeformedRotation(originalRot.z),
               GetDeformedRotation(originalRot.w)
            );

            currentShakeIntensity -= shakeDecay * Time.deltaTime;
        }
    }

    private float GetDeformedRotation(float axisValue)
    {
        return axisValue + Random.Range(-shakeIntensity, shakeIntensity) * SHAKE_DEFORM_COEF;
    }

	void OnGUI()
	{
		if (displayEffect == true)
		{
            // apply alpha for fade out
            Vector4 tempColor = GUI.color;
            tempColor.w = alpha;
            GUI.color = tempColor;
            
            //draw texture to GUI
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), hurtTexture, ScaleMode.StretchToFill);
        }
	}

    // call this function from the follower's (bee swarm) script, when the distance is close enough
    public void Hit()
    {
        if (!displayEffect)
        {
            displayEffect = true;

            originalPos = transform.position;
            originalRot = transform.rotation;

            int hurtSoundNumber = Random.Range(0, hurtSound.Length);

            audioPlayer.PlayEffect(hurtSound[hurtSoundNumber], false);
            StartCoroutine(ApplyEffect());
        }
    }
}
