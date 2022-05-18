using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private RectMask2D deltaFill;
    [SerializeField] private RectMask2D healthFill;

    [SerializeField] private Image deltaFillImage;

    private const float timeDeltaPause = 0.25f;
    private const float timeDeltaMove = 1f;

    RectTransform healthBarRectTransform;
    private float width;

    //health parameters
    private int maxHealth;
    private int preDeltaHealth;
    private int currentHealth;

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

    private void Start()
    {
        deltaFill.padding = new Vector4(0, 0, 0, 0);
        healthFill.padding = new Vector4(0, 0, 0, 0);
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

   

    public void Initialize(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        preDeltaHealth = maxHealth;
        width = (float)maxHealth / 100;

        healthBarRectTransform.sizeDelta = new Vector2(width, healthBarRectTransform.sizeDelta.y);
    }

    public void UpdateCurrentHealth(int newCurrentHealth)
    {
        preDeltaHealth = currentHealth;
        currentHealth = newCurrentHealth;

        currentTimeDeltaPause = 0f;
        currentTimeDeltaMove = 0f;
        isTakingDamage = false;
        isHealing = false;

        if (preDeltaHealth > currentHealth)
        {
            HandleTakeDamage();
        }
        else if (preDeltaHealth < currentHealth)
        {
            HandleHeal();
        }


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

   

    public void UpdateMaxHealth(int newMaxHealth, int newCurrentHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = newCurrentHealth;

        width = (float)maxHealth / 100;
        healthBarRectTransform.sizeDelta = new Vector2(width, healthBarRectTransform.sizeDelta.y);

        float healthRatio = (float)currentHealth / maxHealth;
        Debug.Log($"Health ratio is now: {healthRatio}");
        deltaFill.padding = new Vector4(0, 0, CalculateHealthBarOffset(), 0);
        healthFill.padding = new Vector4(0, 0, CalculateHealthBarOffset(), 0);
    }

    private float CalculateHealthBarOffset()
    {
        float healthRatio = (float)currentHealth / maxHealth;
        return (1f - healthRatio) * width;
    }


}
