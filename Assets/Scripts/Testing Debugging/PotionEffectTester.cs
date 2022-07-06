using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEffectTester : MonoBehaviour
{
    Entity entity;
    public EnhMagicalDamageBlueprint magicalDamageBlueprint;
    public EnhCooldownModifierBlueprint cooldownBlueprint;
    public EnhMoveSpeedBlueprint moveSpeedBlueprint;
    public EnhShieldBlueprint shieldBlueprint;
    public EnhHealthRegenBlueprint healthRegenBlueprint;
    public EnhThornsBlueprint thornsBlueprint;
    public EnhImmunityBlueprint immunityBlueprint;

    public FlatDmgBlueprint flatDmgBlueprint;
    public DotDmgBlueprint dotDmgBlueprint;

    private EnhMagicalDamage magicalDamage;
    private EnhCooldownModifier cooldownModifier;
    private EnhMoveSpeed moveSpeed;
    private EnhShield shield;
    private EnhHealthRegen healthRegen;
    private EnhThorns thorns;
    private EnhImmunity immunity;

    private FlatDmg flatDmg;
    private DotDmg dotDmg;

    private void Awake()
    {
        entity = GetComponent<Entity>();

        if (entity == null)
            Debug.LogError("entity was null!");
        else
            Debug.Log($"PotionEffectTester attached to entity: {entity.name}");

        magicalDamage = new EnhMagicalDamage(magicalDamageBlueprint);
        cooldownModifier = new EnhCooldownModifier(cooldownBlueprint);
        moveSpeed = new EnhMoveSpeed(moveSpeedBlueprint);
        shield = new EnhShield(shieldBlueprint);
        healthRegen = new EnhHealthRegen(healthRegenBlueprint);
        thorns = new EnhThorns(thornsBlueprint);
        immunity = new EnhImmunity(immunityBlueprint);

        flatDmg = new FlatDmg(flatDmgBlueprint);
        dotDmg = new DotDmg(dotDmgBlueprint);
    }


    [ContextMenu("Apply Magic Damage Enhancement")]
    public void ApplyMagicalDamage()
    {
        magicalDamage.RunEffect(entity, entity);
    }
    [ContextMenu("Apply Cooldown Modifier Enhancement")]
    public void ApplyCooldownModifier()
    {
        cooldownModifier.RunEffect(entity, entity);
    }
    [ContextMenu("Apply Move Speed Enhancement")]
    public void ApplyMoveSpeed()
    {
        moveSpeed.RunEffect(entity, entity);
    }
    [ContextMenu("Apply Shield Enhancement")]
    public void ApplyShield()
    {
        shield.RunEffect(entity, entity);
    }
    [ContextMenu("Apply Health Regen Enhancement")]
    public void ApplyHealthRegen()
    {
        healthRegen.RunEffect(entity, entity);
    }
    [ContextMenu("Apply Thorns Enhancement")]
    public void ApplyThorns()
    {
        thorns.RunEffect(entity, entity);
    }
    [ContextMenu("Apply Immunity Enhancement")]
    public void ApplyImmunity()
    {
        immunity.RunEffect(entity, entity);
    }

    [ContextMenu("Apply Flat Damage")]
    public void ApplyFlatDamage()
    {
        flatDmg.RunEffect(entity, entity);
    }
    [ContextMenu("Apply Dot Damage")]
    public void ApplyDotDamage()
    {
        dotDmg.RunEffect(entity, entity);
    }
}
