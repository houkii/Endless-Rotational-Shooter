using System.Collections;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform muzzle;

    [SerializeField] private float RecoilPositionOffset = 0.16f;
    [SerializeField] private float RecoilRotationOffset = -5f;
    private Coroutine RecoilCoroutine = null;
    private Vector3 defaultLocalPosition;
    private Quaternion defaultLocalRotation;
    private Vector3 recoilPosition;
    private Quaternion recoilRotation;

    private void Awake()
    {
        this.defaultLocalPosition = transform.localPosition;
        this.defaultLocalRotation = transform.localRotation;
        this.recoilPosition = defaultLocalPosition - new Vector3(0, 0, RecoilPositionOffset);
        this.recoilRotation = defaultLocalRotation * Quaternion.Euler(RecoilRotationOffset, 0, 0);
    }

    public void Shoot()
    {
        var ball = Instantiate(projectilePrefab, muzzle.position, muzzle.transform.rotation);

        // let only one recoil coroutine running
        if(RecoilCoroutine != null)
        {
            StopCoroutine(RecoilCoroutine);
        }
        RecoilCoroutine = StartCoroutine(Recoil(.05f, .15f));
    }

    // attackTime - rising, releasTime - falling (as in signal envelopes)
    private IEnumerator Recoil(float attackTime, float releaseTime)
    {
        float currentTime = 0f;
        Vector3 startPosition = transform.localPosition;
        Quaternion startRotation = transform.localRotation;

        // move to recoil position
        // time will be "slightly" exceeded (1 frame) to get positions right 
        while (transform.localPosition != recoilPosition)
        {
            float step = Mathf.Clamp01(currentTime / attackTime);
            transform.localPosition = Vector3.Lerp(startPosition, this.recoilPosition, step);
            transform.localRotation = Quaternion.Slerp(startRotation, this.recoilRotation, step);
            currentTime += Time.deltaTime;
            yield return null;
        }

        currentTime = 0f;

        // move to default position
        while (transform.localPosition != defaultLocalPosition)
        {
            float step = Mathf.Clamp01(currentTime / releaseTime);
            transform.localPosition = Vector3.Lerp(this.recoilPosition, this.defaultLocalPosition, step);
            transform.localRotation = Quaternion.Slerp(this.recoilRotation, this.defaultLocalRotation, step);
            currentTime += Time.deltaTime;
            yield return null;
        }

        yield break;
    }
}
