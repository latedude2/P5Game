using System.Collections;
using UnityEngine;

public class HurtEffect : MonoBehaviour
{
    private readonly float SHAKE_DEFORM_COEF = .2f;

    [SerializeField] private bool isTechnicalTesting; //only for testing purposes

    [SerializeField] private bool isShaking;
    [SerializeField] private float shakeIntensity;
    [SerializeField] private float shakeDecay;
    private float currentShakeIntensity;
    private Vector3 originalPos;
    private Quaternion originalRot;

	[SerializeField] private Texture hurtEffect;
	private bool displayHurtEffect = false;
    private float alpha = 1f;

    private AudioScript audioPlayer;
    [SerializeField] AudioClip hurtSound;

    private void Start()
    {
        audioPlayer = GetComponent<AudioScript>();
    }

    void Update()
    {
        if(isTechnicalTesting)
            TestHit();
    }

    IEnumerator ApplyEffect()
    {
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            Shake();
            yield return null;
        }
        displayHurtEffect = false;
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
        else if (isShaking)
        {
            isShaking = false;
        }
    }

    private float GetDeformedRotation(float axisValue)
    {
        return axisValue + Random.Range(-shakeIntensity, shakeIntensity) * SHAKE_DEFORM_COEF;
    }

    private void TestHit()
    {
        Hit();
    }

    // call this function from the follower's (bee swarm) script, when the distance is close enough
    public void Hit()
    {
        if (!displayHurtEffect && !isShaking)
        {
            displayHurtEffect = true;


            currentShakeIntensity = shakeIntensity;
            originalPos = transform.position;
            originalRot = transform.rotation;

            isShaking = true;
            audioPlayer.PlayEffect(hurtSound, false);
            StartCoroutine(ApplyEffect());
        }
    }

	void OnGUI()
	{
		if (displayHurtEffect == true)
		{
            //The hurt effect is displyed using a DrawTexture and the texture is stretched to fill
            //the screen.
            Vector4 tempColor = GUI.color;
            tempColor.w = alpha;
            GUI.color = tempColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), hurtEffect, ScaleMode.StretchToFill);
        }
	}
}
