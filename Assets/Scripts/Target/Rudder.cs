using UnityEngine;
using DGL.Math;
using System;

/// <summary>
/// An effector that deflects force underwater.
/// </summary>
public class Rudder : MonoBehaviour
{
   [Tooltip("How much steering force the rudder actually has")]
   [SerializeField][Range(0, 1)] float _effect = 0.1f;

   [Tooltip("Speed the rudder reaches full effect")]
   [SerializeField] [Range(0, 1)] float _shiftSpeed = 0.5f;

   Rigidbody _rigidbody;
   Vehicle _ship;
   private float _angleOfAttack = 0;
   private float _percentAoa = 0;
   private float _actualAoa = 0;

   // A speed that ships generally will never exceed
   public const float GeneralMaxSpeed = 200f;

   // Start is called before the first frame update
   void Awake()
   {
      GetParentItems();
   }

   private void Update()
   {
      _actualAoa = Mathf.Lerp(_actualAoa, _percentAoa, _shiftSpeed * Time.deltaTime);
   }

   public void GetParentItems()
   {
      _rigidbody = GetComponentInParent<Rigidbody>();
      _ship = GetComponentInParent<Vehicle>();
   }

   private void FixedUpdate()
   {
      // Find the total direction the ship is going as compared to the rudder
      Vector3 localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);
      _angleOfAttack = Vector3.Angle(new Vector3(0, 0, 1), localVelocity);
      // Vector3.Angle always returns a positive value, so add the sign back in
      _angleOfAttack *= Mathf.Sign(localVelocity.x);

      _percentAoa = Utility.Percent(_angleOfAttack, 90f, PercentMode.ClampNegative1To1);
      float percentSpeed = Utility.Percent(_ship.ForwardSpeed, GeneralMaxSpeed, PercentMode.ClampNegative1To1); // Negative speeds are backwards
      float massSquared = _rigidbody.mass * _rigidbody.mass;

      // Debug.Log("Force: " + (speedPercent * m_AngleOfAttack));

      _rigidbody.AddRelativeTorque(new Vector3(0, 1, 0) * _effect * _actualAoa * massSquared * Math.Abs(percentSpeed) * Time.deltaTime, ForceMode.Force); // The actual turning
      _rigidbody.AddRelativeTorque(new Vector3(0, 0, 1) * _effect * _actualAoa * _rigidbody.mass * percentSpeed * Time.deltaTime, ForceMode.Force); // Turning effect
   }
}
