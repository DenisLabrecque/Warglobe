using UnityEngine;

/// <summary>
/// Control a rotating part (eg. airplane aileron).
/// Has an audio source that plays a sound representing the motor behind the control surface.
/// 
/// Bryan Hernandez
/// Denis Labrecque, January 2019
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ControlSurface : MonoBehaviour
{
   #region Enumerations

   enum Axis { x, y, z }
   enum Mode { swivel, open } // An aileron swivels while a door opens (the aileron has negative deflection)
   enum Direction { down = -1, neutral = 0, up = 1, jammed = 2 }

   #endregion


   #region Member Variables

//   [SerializeField] string testing = string.Empty;

	[Header("Deflection")]
	[Tooltip("Deflection with max positive input at max control authority")]
	[SerializeField] float m_MaxAngle = 30.0f;

	[Tooltip("Speed of the control surface deflection.")]
	[SerializeField] float m_MoveSpeed = 60.0f;

   [Tooltip("How much exponential affects deflection (factor).")]
   [SerializeField][Range(0,1)] float m_Exponential = 0.3f;

   [Header("Control Type Settings")]
   [SerializeField] Axis m_Axis = Axis.x;

   [Tooltip("Whether this control surface can have negative deflections")]
   [SerializeField] Mode m_Mode = Mode.swivel;

   [SerializeField] AudioClip m_UpSound;
   [SerializeField] AudioClip m_DownSound;
   [SerializeField] AudioClip m_JammedSound;

	private Quaternion m_StartLocalRotation = Quaternion.identity;
   private Vector3 m_TurnAxis;
   private AudioSource m_AudioSource;

	private float m_DeflectionInput = 0.0f;
   private float m_CurrentAngle = 0.0f; // Angle, in degrees, off the original angle
   private float m_PreviousAngle = 0.0f;
   private Direction m_PreviousDirection = Direction.neutral;
   private Direction m_CurrentDirection = Direction.neutral;

   #endregion


   #region Properties

   /// <summary>
   /// Set deflection as a percent of the maximum deflection.
   /// Clamped either from -1 to 1 or from 0 to 1 depending on the mode.
   /// </summary>
   public float DeflectionPercent {
      get { return m_DeflectionInput; }
      set {
         if(m_Mode == Mode.swivel)
            m_DeflectionInput = Mathf.Clamp(value, -m_MaxAngle, m_MaxAngle);
         else
            m_DeflectionInput = Mathf.Clamp(value, 0f, m_MaxAngle);
      }
   }

   public float CurrentAngle {
      get { return m_CurrentAngle; }
   }

   #endregion


   #region Unity Methods

   private void Awake()
   {
      m_AudioSource = GetComponent<AudioSource>();
      m_AudioSource.playOnAwake = false;
      m_AudioSource.spatialBlend = 1f; // 2D-3D
      m_AudioSource.rolloffMode = AudioRolloffMode.Linear;
   }

	private void Start()
	{
		// Dirty hack so that the rotation can be reset before applying the deflection.
		m_StartLocalRotation = transform.localRotation;

      // Set the proper rotation direction
      switch(m_Axis)
      {
         case (Axis.x):
            m_TurnAxis = Vector3.right;
            break;
         case (Axis.y):
            m_TurnAxis = Vector3.up;
            break;
         case (Axis.z):
            m_TurnAxis = Vector3.forward;
            break;
         default:
            m_TurnAxis = Vector3.right;
            break;
      }
	}

	private void FixedUpdate()
	{
      // Move the control surface with exponential motion.
      float nonExponential = 1f - m_Exponential;
      float expoMoveSpeed = m_CurrentAngle * m_CurrentAngle / m_MaxAngle * m_MaxAngle;
      float finalMoveSpeed = (expoMoveSpeed * m_Exponential) + (nonExponential * m_MoveSpeed);
		m_CurrentAngle = Mathf.MoveTowards(m_CurrentAngle, m_DeflectionInput * m_MaxAngle, finalMoveSpeed * Time.deltaTime);
      m_CurrentDirection = FindDirection();

      // Play the sound for the current action
      EvaluateSound();

		// Hacky way to do this!
		transform.localRotation = m_StartLocalRotation;
		transform.Rotate(m_TurnAxis, m_CurrentAngle, Space.Self);

      // Save the angle and direction for the next update
      m_PreviousAngle = m_CurrentAngle;
      m_PreviousDirection = m_CurrentDirection;
	}

   #endregion


   #region Private Methods

   /// <summary>
   /// Find which direction a control surface is rotating.
   /// </summary>
   /// <returns>Up, down, or neutral</returns>
   private Direction FindDirection()
   {
      if(m_PreviousAngle == 0 && m_CurrentAngle == 0)
         return Direction.neutral;
      else if(m_PreviousAngle > m_CurrentAngle)
         return Direction.down;
      else if(m_PreviousAngle < m_CurrentAngle)
         return Direction.up;
      else if(m_PreviousAngle == m_CurrentAngle)
         return m_CurrentDirection;
      else
         return Direction.jammed;
   }

   /// <summary>
   /// Evaluate what the control surface is doing and play the appropriate sound.
   /// </summary>
   private void EvaluateSound()
   {
      // Is at neutral; play no sound
      if(m_CurrentAngle == 0 && m_PreviousAngle == 0)
      {
         m_AudioSource.Stop();

         //if(testing == "test")
//            Debug.Log(m_CurrentDirection.ToString() + " " + m_PreviousDirection.ToString() + " " + Mathf.RoundToInt(m_CurrentAngle) + " " + Mathf.RoundToInt(m_PreviousAngle));
      }

      // Play an up sound
      else if(m_CurrentDirection == Direction.up && m_PreviousDirection != Direction.up)
      {
         m_AudioSource.Stop();
         m_AudioSource.clip = m_UpSound;
         m_AudioSource.loop = false;
         m_AudioSource.Play();

         //if(testing == "test")
//            Debug.Log(m_CurrentDirection.ToString() + " " + m_PreviousDirection.ToString() + " " + (m_CurrentAngle) + " " + (m_PreviousAngle));
      }

      // Play a down sound
      else if(m_CurrentDirection == Direction.down && m_PreviousDirection != Direction.down)
      {
         m_AudioSource.Stop();
         m_AudioSource.clip = m_DownSound;
         m_AudioSource.loop = false;
         m_AudioSource.Play();

         //if(testing == "test")
//            Debug.Log(m_CurrentDirection.ToString() + " " + m_PreviousDirection.ToString() + " " + (m_CurrentAngle) + " " + (m_PreviousAngle));
      }

      // Play a maxed out sound
      else if(m_CurrentAngle == m_MaxAngle || m_CurrentAngle == -m_MaxAngle)
      {
         m_AudioSource.Stop();
         m_AudioSource.clip = m_JammedSound;
         m_AudioSource.loop = true;
         m_AudioSource.Play();

         //if(testing == "test")
//            Debug.Log(m_CurrentDirection.ToString() + " " + m_PreviousDirection.ToString() + " " + Mathf.RoundToInt(m_CurrentAngle) + " " + Mathf.RoundToInt(m_PreviousAngle));
      }

      // We don't know what happened
      else
      {
         //if(testing == "test")
//            Debug.Log(m_CurrentDirection.ToString() + " " + m_PreviousDirection.ToString() + " " + (m_CurrentAngle) + " " + (m_PreviousAngle));
      }
   }
   
   #endregion
}