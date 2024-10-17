namespace EnemySystem
{
    public class BossFlyEnemy : FlyEnemy
    {
        protected override void OnHPChanging() => Wave.Instance.ChangeBossHpBarFill(hp, DefaultHp);
    }
}