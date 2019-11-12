public class DumbBomb : Projectile
{
   public override bool LaunchAuthority => true;

   public override float HitProbability => throw new System.NotImplementedException();
}
