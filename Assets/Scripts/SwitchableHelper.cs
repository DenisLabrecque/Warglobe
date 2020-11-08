namespace Warglobe
{
   public static class SwitchableHelper
   {
      public static Group GroupFromFunction(Function function)
      {
         switch(function)
         {
            case Function.Cannon:
            case Function.Ciws:
               return Group.Guns;
            case Function.Missile:
               return Group.Missiles;
            case Function.Radar:
            case Function.Sonar:
            case Function.Light:
               return Group.Accessories;
            default:
               return Group.Accessories;
         }
      }
   }
}