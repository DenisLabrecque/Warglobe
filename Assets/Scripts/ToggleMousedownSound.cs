using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ToggleMousedownSound : MonoBehaviour, IPointerDownHandler
{
   [SerializeField] AudioClip m_ClickSound;

   private AudioSource m_AudioSource;

   private void Awake()
   {
      m_AudioSource = GetComponent<AudioSource>();
      m_AudioSource.clip = m_ClickSound;
   }

   public void OnPointerDown(PointerEventData eventData)
   {
      m_AudioSource.Play();
   }
}
