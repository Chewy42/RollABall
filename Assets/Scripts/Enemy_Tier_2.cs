public class Enemy_Tier_2 : Enemy
{
    protected override void Start()
    {
        base.Start();
        speed = 6f;
        health = 1f;
        damage = 3f;
        SetXP(2);
    }
}
