public class BossAnimation : MobAnimation
{
    protected const string BOSS_IDLE = "Idle";
    protected const string BOSS_MOVE = "Move";
    protected const string BOSS_ATTACK = "Attack";
    protected const string BOSS_HURT = "Hurt";
    protected const string BOSS_DIE = "Die";

    public override void Start()
    {
        ChangeAnimationState(BOSS_IDLE);
    }
}
