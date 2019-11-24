using UnityEngine;

public class SimpleWing : MonoBehaviour
{
	[Tooltip("Size of the wing. The bigger the wing, the more lift it provides.")]
	public Vector2 m_Dimensions = new Vector2(5.0f, 2.0f);

	[Tooltip("When true, wing forces will be applied only at the center of mass.")]
	public bool m_ForcesToCenter = false;

	[Tooltip("Lift coefficient curve.")]
	public WingCurves m_WingCurve;
	[Tooltip("The higher the value, the more lift the wing applies at a given angle of attack.")]
	public float m_LiftMultiplier = 1.0f;
	[Tooltip("The higher the value, the more drag the wing incurs at a given angle of attack.")]
	public float m_DragMultiplier = 1.0f;

	private Rigidbody m_Rigidbody;

	private float m_LiftCoefficient = 0.0f;
	private float m_DragCoefficient = 0.0f;
	private float m_Lift = 0.0f;
	private float m_Drag = 0.0f;
	private float m_AngleOfAttack = 0.0f;

   private float m_PreviousLiftForce = 0.0f;
   private float m_PreviousDragForce = 0.0f;
   private float m_MaxPercentChange = 0.2f;

   private Vector3 appliedforce;
   private Vector3 appliedPosition;

   /// <summary>
   /// Set the lift multiplier (default of 1)
   /// </summary>
   public float Lift {
      set {
         m_LiftMultiplier = value;
      }
   }

   /// <summary>
   /// Set the drag multiplier (default of 1)
   /// </summary>
   public float Drag {
      set {
         m_DragMultiplier = value;
      }
   }
   public float LiftCoefficient { get { return m_LiftCoefficient; } }
   public float LiftForce { get { return m_Lift; } }
   public float DragCoefficient { get { return m_DragCoefficient; } }
   public float DragForce { get { return m_Drag; } }

   public float AngleOfAttack
   {
		get
		{
			if (m_Rigidbody != null)
			{
				Vector3 localVelocity = transform.InverseTransformDirection(m_Rigidbody.velocity);
				return m_AngleOfAttack * -Mathf.Sign(localVelocity.y);
			}
			else
			{
				return 0.0f;
			}
		}
	}

	public float WingArea
	{
		get { return m_Dimensions.x * m_Dimensions.y; }
	}

	public Rigidbody Rigidbody
	{
      get { return m_Rigidbody; }
		set { m_Rigidbody = value; }
	}

	private void Awake()
	{
      AssignParentRigidbody();
	}

   public void AssignParentRigidbody() {
      // I don't especially like doing this, but there are many cases where wings might not
      // have the rigidbody on themselves (e.g. they are on a child gameobject of a plane).
      m_Rigidbody = GetComponentInParent<Rigidbody>();
   }

	private void Start()
	{
		if (m_Rigidbody == null)
		{
			Debug.LogError(name + ": SimpleWing has no rigidbody on self or parent!");
		}

		if (m_WingCurve == null)
		{
			Debug.LogError(name + ": SimpleWing has no defined wing curves!");
		}
	}

	private void Update()
	{
		// Show editor gizmos
		if (m_Rigidbody != null)
		{
			Debug.DrawRay(transform.position, transform.up * m_Lift, Color.blue);
			Debug.DrawRay(transform.position, -m_Rigidbody.velocity.normalized * m_Drag, Color.red);
		}
	}

   private void FixedUpdate()
   {
      // Prevent division by zero.
      if (m_Dimensions.x <= 0.0f)
         m_Dimensions.x = 0.001f;
      if (m_Dimensions.y <= 0.0f)
         m_Dimensions.y = 0.001f;

      if (m_Rigidbody != null && m_WingCurve != null)
      {
         Vector3 forceApplyPos = (m_ForcesToCenter) ? m_Rigidbody.transform.TransformPoint(m_Rigidbody.centerOfMass) : transform.position;

         Vector3 localVelocity = transform.InverseTransformDirection(m_Rigidbody.GetPointVelocity(transform.position));
         localVelocity.x = 0.0f;

         // Angle of attack is used as the look up for the lift and drag curves.
         m_AngleOfAttack = Vector3.Angle(Vector3.forward, localVelocity);
         m_LiftCoefficient = m_WingCurve.GetLiftAtAaoA(m_AngleOfAttack);
         m_DragCoefficient = m_WingCurve.GetDragAtAaoA(m_AngleOfAttack);

         // Calculate lift/drag.
         float airDensity = Planet.Singleton.AirDensity(gameObject);
         m_Lift = localVelocity.sqrMagnitude * m_LiftCoefficient * WingArea * m_LiftMultiplier * airDensity * (Time.deltaTime * 2);
         m_Drag = localVelocity.sqrMagnitude * m_DragCoefficient * WingArea * m_DragMultiplier * airDensity * (Time.deltaTime * 2);

         // Vector3.Angle always returns a positive value, so add the sign back in.
         m_Lift *= -Mathf.Sign(localVelocity.y);

         // Smooth lift and drag
         // Prevent rapid sign inversion
         //if ((m_Lift > 0 && m_PreviousLiftForce < 0) || m_Lift < 0 && m_PreviousLiftForce > 0)
         //   m_Lift = 0;
         //if ((m_Drag > 0 && m_PreviousDragForce < 0) || m_Drag < 0 && m_PreviousDragForce > 0)
         //   m_Drag = 0;

         // Lift is always perpendicular to air flow.
         Vector3 liftDirection = Vector3.Cross(m_Rigidbody.velocity, transform.right).normalized;
         m_Rigidbody.AddForceAtPosition(liftDirection * m_Lift, forceApplyPos, ForceMode.Force);

         // Drag is always opposite of the velocity.
         m_Rigidbody.AddForceAtPosition(-m_Rigidbody.velocity.normalized * m_Drag, forceApplyPos, ForceMode.Force);

         m_PreviousLiftForce = m_Lift;
         m_PreviousDragForce = m_Drag;
      }
   }

	private void OnDrawGizmosSelected()
	{
		Matrix4x4 oldMatrix = Gizmos.matrix;

		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
		Gizmos.DrawWireCube(Vector3.zero, new Vector3(m_Dimensions.x, 0.0f, m_Dimensions.y));

		Gizmos.matrix = oldMatrix;
	}
}