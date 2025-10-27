using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParadoxRifle : BaseWeapon
{
    [Header("References")]
    public Transform firePoint;
    public Camera playerCam;
    public LineRenderer lineRenderer;
    public ParticleSystem chargeEffect;
    public ParticleSystem fireEffect;

    [Header("Zoom Settings")]
    public float zoomFOV = 40f;
    public float normalFOV = 60f;
    public float zoomSpeed = 8f;

    private bool isCharging = false;
    private bool isAiming = false;
    private float chargeProgress = 0f;

    private void Start()
    {
        playerCam = Camera.main;

        playerCam.fieldOfView = normalFOV;
        if (lineRenderer != null)
            lineRenderer.enabled = false;

        ChargeUIManager.Instance.SetActive(false); //초기 비활성화
    }

    private void Update()
    {
        HandleAim();
        HandleAttack();
    }

    void HandleAim()
    {
        //우클릭 시 조준
        isAiming = Input.GetMouseButton(1);

        float targetFOV = isAiming ? zoomFOV : normalFOV;
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);

        // UI 활성화 / 비활성화
        ChargeUIManager.Instance.SetActive(isAiming);
    }

    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && isAiming && !isCharging)
            Attack();
    }

    public override void Attack()
    {
        if (isCharging) return;
        StartCoroutine(ChargeAndFire());
    }

    IEnumerator ChargeAndFire()
    {
        isCharging = true;
        chargeProgress = 0f;

        if (chargeEffect != null)
            chargeEffect.Play();

        while (chargeProgress < weaponData.chargeTime)
        {
            chargeProgress += Time.deltaTime;

            float progressPercent = chargeProgress / weaponData.chargeTime;
            ChargeUIManager.Instance.UpdateChargeProgress(progressPercent);

            yield return null;
        }

        if (chargeEffect != null)
            chargeEffect.Stop();

        Fire();
        chargeProgress = 0f;
        ChargeUIManager.Instance.UpdateChargeProgress(0f);

        isCharging = false;
    }

    void Fire()
    {
        if (fireEffect != null)
            fireEffect.Play();

        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit[] hits = Physics.RaycastAll(ray, weaponData.maxRange);

        Vector3 endPoint = ray.origin + ray.direction * weaponData.maxRange;

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out IDamageable dmg))
            {
                dmg.TakeDamage(weaponData.damage);
                Debug.Log($"{hit.collider.name}에게 {weaponData.damage} 피해!");
            }

            endPoint = hit.point; // 가장 먼 피격 지점 갱신
        }

        if (lineRenderer != null)
            StartCoroutine(FireBeam(endPoint));
    }

    IEnumerator FireBeam(Vector3 hitPoint)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, hitPoint);

        yield return new WaitForSeconds(0.05f);
        lineRenderer.enabled = false;
    }
}