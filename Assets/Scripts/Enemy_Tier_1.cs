public class Enemy_Tier_1 : Enemy
{
    protected override void Start()
    {
        base.Start();
        speed = 2f;
        health = 3f;
        SetXP(1);
    }
}
