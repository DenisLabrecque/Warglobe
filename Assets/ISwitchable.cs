/// <summary>
/// Which are of the HUD this switchable belongs to.
/// </summary>
public enum Group
{
   Guns,
   Missiles,
   Accessories
}

/// <summary>
/// Defines a toggle or weapon selection that is to appear on the bottom of the HUD with an icon, name, and keystroke.
/// </summary>
public interface ISwitchable
{
   /// <summary>
   /// Name of the toggle or weapon.
   /// </summary>
   string Name { get; }

   /// <summary>
   /// Whether the current setting or weapon is in use or ready to be used.
   /// For example, the radar is on, or the weapon is selected to fire.
   /// </summary>
   bool IsOnOrSelected { get; }
   
   /// <summary>
   /// The key the user must press to access this switchable.
   /// For example, "R" for "Radar".
   /// </summary>
   string Keystroke { get; }

   /// <summary>
   /// The HUD area to which this switchable belongs.
   /// </summary>
   Group Group { get; }
}