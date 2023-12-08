public class Enemy_Tier_1 : Enemy
{
    protected override void Start()
    {
        base.Start();
        speed = 2.5f;
        health = 3f;
        damage = 1f;
        SetXP(1);
    }
}
