using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private RectMask2D deltaFill;
    [SerializeField] private RectMask2D healthFill;

    [SerializeField] private Image deltaFillImage;

    private const float timeDeltaPause = 0.5f;
    private const float timeDeltaMove = 1f;

    RectTransform healthBarRectTransform;
    private float width;

    //healthy wealthy
    HealthSystem health;
    float oldHealth;

    //states
    private bool isTakingDamage;
    private bool isHealing;

    //state time variables
    private float currentTimeDeltaPause;
    private float currentTimeDeltaMove;

    //cached values for lerp
    private float tempDeltaPadding;
    private float tempHealthPadding;


    private void Awake()
    {
        healthBarRectTransform = GetComponent<RectTransform>();
        isTakingDamage = false;
        isHealing = false;
    }


    private void Update()
    {
        if (isTakingDamage)
        {
            UpdateDamageAnim();
            return;
        }

        if (isHealing)
        {
            UpdateHealAnim();
            return;
        }
    }

   

    public void Initialize(HealthSystem health)
    {
        this.health = health;
        oldHealth = health.CurrentHealth;

        width = health.MaxHealth / 100;
        healthBarRectTransform.sizeDelta = new Vector2(width, healthBarRectTransform.sizeDelta.y);

        deltaFill.padding = new Vector4(0, 0, 0, 0);
        healthFill.padding = new Vector4(0, 0, 0, 0);

        health.onFlatDamaged += UpdateHealthBar;
        health.onDotDamaged += UpdateHealthBar;
        health.onHealed += UpdateHealthBar;
        health.onNewMaxHealth += UpdateMaxHealth;
    }

    public void UpdateHealthBar()
    {
        currentTimeDeltaPause = 0f;
        currentTimeDeltaMove = 0f;
        isTakingDamage = false;
        isHealing = false;

        if (health.CurrentHealth < oldHealth)
        {
            HandleTakeDamage();
        }
        else if (health.CurrentHealth > oldHealth)
        {
            HandleHeal();
        }

        oldHealth = health.CurrentHealth;
    }

    private void HandleTakeDamage()
    {
        deltaFillImage.color = ColorPallete.healthBarDamageColor;
        healthFill.padding = new Vector4(0, 0, CalculateHealthBarOffset(), 0);

        isTakingDamage = true;
        tempDeltaPadding = deltaFill.padding.z;
    }
    private void UpdateDamageAnim()
    {
        if (currentTimeDeltaPause < timeDeltaPause)
        {
            currentTimeDeltaPause += Time.deltaTime;
            return;
        }
        if (currentTimeDeltaMove < timeDeltaMove)
        {
            currentTimeDeltaMove += Time.deltaTime;
            float percentComplete = currentTimeDeltaMove / timeDeltaMove;
            float paddingAmount = Mathf.Lerp(tempDeltaPadding, healthFill.padding.z, percentComplete);
            deltaFill.padding = new Vector4(0, 0, paddingAmount, 0);
            return;
        }
        isTakingDamage = false;

    }

    private void HandleHeal()
    {
        deltaFillImage.color = ColorPallete.healthBarHealColor;
        deltaFill.padding = new Vector4(0, 0, CalculateHealthBarOffset(), 0);

        isHealing = true;
        tempHealthPadding = healthFill.padding.z;
    }
    private void UpdateHealAnim()
    {
        if (currentTimeDeltaPause < timeDeltaPause)
        {
            currentTimeDeltaPause += Time.deltaTime;
            return;
        }

        if (currentTimeDeltaMove < timeDeltaMove)
        {
            currentTimeDeltaMove += Time.deltaTime;
            float percentComplete = currentTimeDeltaMove / timeDeltaMove;
            float paddingAmount = Mathf.Lerp(tempHealthPadding, deltaFill.padding.z, percentComplete);
            healthFill.padding = new Vector4(0, 0, paddingAmount, 0);
            return;
        }
            
        isHealing = false;
    }

   

    public void UpdateMaxHealth()
    {
        width = (float)health.MaxHealth / 100;
        healthBarRectTransform.sizeDelta = new Vector2(width, healthBarRectTransform.sizeDelta.y);

        deltaFill.padding = new Vector4(0, 0, CalculateHealthBarOffset(), 0);
        healthFill.padding = new Vector4(0, 0, CalculateHealthBarOffset(), 0);
    }

    private float CalculateHealthBarOffset()
    {
        float healthRatio = health.CurrentHealth / health.MaxHealth;
        return (1f - healthRatio) * width;
    }


}
