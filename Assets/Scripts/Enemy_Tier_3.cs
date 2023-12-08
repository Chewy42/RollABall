public class Enemy_Tier_3 : Enemy
{
    protected override void Start()
    {
        base.Start();
        speed = 1.75f;
        health = 7f;
        damage = 5f;
        SetXP(3);
    }
}
