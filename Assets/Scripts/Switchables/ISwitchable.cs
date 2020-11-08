using Warglobe.Assets;

namespace Warglobe
{

   /// <summary>
   /// Which are of the HUD this switchable belongs to.
   /// </summary>
   public enum Group
   {
      Guns,
      Missiles,
      Accessories
   }

   public enum Function {
      Cannon,
      Ciws,
      Missile,
      Radar,
      Sonar,
      Light
   }

   /// <summary>
   /// Defines a toggle or weapon selection that is to appear on the bottom of the HUD with an icon, name, and keystroke.
   /// </summary>
   public interface ISwitchable
   {
      /// <summary>
      /// A reference to the name, icon, and keystroke scriptable object enum value that represents this thing.
      /// </summary>
      Switchable Switchable { get; }

      /// <summary>
      /// Whether the current setting or weapon is in use or ready to be used.
      /// For example, the radar is on, or the weapon is selected to fire.
      /// </summary>
      bool IsOnOrSelected { get; }
   }
}