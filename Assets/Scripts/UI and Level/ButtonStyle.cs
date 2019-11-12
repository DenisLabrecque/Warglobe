using UnityEngine;

/// <summary>
/// Defines colours and sounds a button will use.
/// </summary>
[CreateAssetMenu(fileName = "ButtonStyle", menuName = "Denis/Button Style")]
public class ButtonStyle : ScriptableObject
{
   [SerializeField] float m_EffectDuration = 0.1f;
   [SerializeField] float m_Volume = 0.9f;
   [SerializeField] Color m_HoverColour = Color.grey;
   [SerializeField] Color m_ClickColour = Color.black;
   [SerializeField] AudioClip m_HoverAudio;
   [SerializeField] AudioClip m_ClickAudio;

   public float EffectDuration { get { return m_Volume; } }
   public float Volume { get { return m_Volume; } }
   public Color HoverColour { get { return m_HoverColour; } }
   public Color ClickColour { get { return m_ClickColour; } }
   public AudioClip HoverAudio { get { return m_HoverAudio; } }
   public AudioClip ClickAudio { get { return m_ClickAudio; } }
}
