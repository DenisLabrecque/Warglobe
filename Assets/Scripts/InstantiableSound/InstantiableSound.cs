using UnityEngine;
using UnityEngine.Audio;

namespace Warglobe
{
   [CreateAssetMenu(menuName ="Denis/Instantiable Sound")]
   public class InstantiableSound : ScriptableObject
   {
      [SerializeField] AudioClip _clip;
      [SerializeField] AudioMixerGroup _mixer;

      /// <summary>
      /// Instantiate a game object at the said position with this audio source and sound.
      /// Also plays the sound and takes care of destroying it.
      /// </summary>
      public void Instantiate(Transform parent)
      {
         GameObject gameObject = new GameObject("InstantiableSound");
         AudioSource source = gameObject.AddComponent<AudioSource>();
         source.clip = _clip;
         source.playOnAwake = true;
         source.outputAudioMixerGroup = _mixer;

         GameObject instance = Instantiate(gameObject, parent);
         Destroy(instance, 3);
      }
   }
}