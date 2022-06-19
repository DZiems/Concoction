using UnityEngine;


[CreateAssetMenu(fileName = "New EnemyAttack", menuName = "Enemy Attack")]
public class EnemyAttack : ScriptableObject
{
    [SerializeField] private string attackName;
    [SerializeField] private float physicalDmgMultiplier = 1.0f;
    [SerializeField] private float magicalDmgMultiplier = 1.0f;
    [SerializeField] private float flatTrueDmg = 0f;
    [SerializeField] private float rangeCondition = 5f;
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private float restTime = 2f;



    private Enemy enemy;

    public float CurrentCooldown { get; private set; }

    public string AttackName => attackName;
    public float RestTime => restTime;


    public Damage AttackDamage => new Damage(
                enemy.CurrentDamage.physical * physicalDmgMultiplier,
                enemy.CurrentDamage.magical * magicalDmgMultiplier,
                flatTrueDmg);


    private bool isInitialized => enemy != null;

    public void Initialize(Enemy enemy)
    {
        this.enemy = enemy;
        CurrentCooldown = 0;
    }

    public void Use(string animationTrigger)
    {
        if (!isInitialized)
        {
            Debug.LogError($"{enemy.name} tried to use an uninitialized attack!");
        }

        Debug.Log($"{enemy.name} used {attackName}");

        enemy.Anim.SetTrigger(animationTrigger);
        CurrentCooldown = cooldown;
        enemy.LastUsedAttack = this;
    }

    public bool IsOffCooldown()
    {
        return CurrentCooldown <= 0f;
    }

    public void DecrementCooldown()
    {
        if (IsOffCooldown())
            return;

        CurrentCooldown -= Time.deltaTime;
        if (IsOffCooldown())
            Debug.Log($"{enemy.name} {attackName} cooldown over");
    }

    public bool IsWithinRange(Vector3 from)
    {
        Vector2 positionDelta = from - enemy.transform.position;

        float sqDistanceRemaining = Vector2.SqrMagnitude(positionDelta);

        return sqDistanceRemaining <= (rangeCondition * rangeCondition);
    }
}



