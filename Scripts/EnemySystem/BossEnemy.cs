namespace EnemySystem
{
    public class BossEnemy : DefaultEnemy
    {
        //private void Start() => IsBlowHit = true;

        protected override void OnHPChanging() => Wave.Instance.ChangeBossHpBarFill(hp, DefaultHp);

        protected override void AfterAttack() => Shelter.Instance.cameraShake.StrongShake();
    }
}