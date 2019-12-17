using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class DropdownMouseoverSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
   [SerializeField] AudioClip m_Hover;
   [SerializeField] AudioClip m_Click;
   [SerializeField] Image m_Image; // Image that changes colour
   [SerializeField] TextMeshProUGUI m_Text; // Text that changes colour
   [SerializeField] Color m_HoverColour;
   [SerializeField] Color m_ActiveColour;

   private TMP_Dropdown m_Dropdown;
   private AudioSource m_AudioSource;
   private Color m_NormalColour;

   private void Awake()
   {
      m_AudioSource = GetComponent<AudioSource>();
      m_Dropdown = GetComponent<TMP_Dropdown>();
      m_NormalColour = m_Image.color;
   }

   public void OnPointerEnter(PointerEventData eventData)
   {
      m_AudioSource.clip = m_Hover;
      m_AudioSource.Play();
      m_Image.color = m_HoverColour;
      m_Text.color = m_HoverColour;
   }

   public void OnPointerDown(PointerEventData eventData)
   {
      m_AudioSource.clip = m_Click;
      m_AudioSource.Play();
      m_Image.color = m_ActiveColour;
      m_Text.color = m_ActiveColour;
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      m_Image.color = m_NormalColour;
      m_Text.color = m_NormalColour;
   }
}
