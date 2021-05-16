using UnityEngine;
using DGL.Math;
using System;

namespace Warglobe
{
   /// <summary>
   /// An effector that deflects force underwater.
   /// </summary>
   public class Rudder : MonoBehaviour
   {
      [SerializeField] Axis _axis = Axis.y;
      [SerializeField] bool _flip = false;

      [Tooltip("How much steering force the rudder actually has")]
      [SerializeField] [Range(0, 100)] float _turnEffect = 10f;

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
         _rigidbody = GetComponentInParent<Rigidbody>();
         _ship = GetComponentInParent<Vehicle>();
      }

      private void Update()
      {
         _actualAoa = Mathf.Lerp(_actualAoa, _percentAoa, _shiftSpeed * Time.deltaTime);
      }

      private Vector3 AxisOf(Axis axis)
      {
         if (axis == Warglobe.Axis.x)
            return Vector3.right;
         else if (axis == Warglobe.Axis.y)
            return Vector3.up;
         else
            return Vector3.forward;
      }

      private void FixedUpdate()
      {
         int flip = _flip == true ? -1 : 1;

         // Find the total direction the ship is going as compared to the rudder
         Vector3 localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);
         if (_ship.ForwardSpeed > 0) // Forwards
            _angleOfAttack = Vector3.Angle(ControlSurface.AxisToVector(_axis), localVelocity);
         else // Backwards
            _angleOfAttack = Vector3.Angle(ControlSurface.AxisToVector(_axis) * -1, localVelocity);

         // Vector3.Angle always returns a positive value, so add the sign back in
         _angleOfAttack *= Mathf.Sign(localVelocity.x);

         _percentAoa = Utility.Percent(_angleOfAttack, 90f, PercentMode.ClampNegative1To1);
         float percentSpeed = Utility.Percent(_ship.ForwardSpeed, GeneralMaxSpeed, PercentMode.ClampNegative1To1); // Negative speeds are backwards
         float massSquared = _rigidbody.mass * _rigidbody.mass;

         // The actual turning; speed is absolute because going backwards flips control direction
         float torque = _turnEffect * _actualAoa * massSquared * Math.Abs(percentSpeed) * Time.deltaTime;
         _rigidbody.AddRelativeTorque(AxisOf(_axis) * torque * flip, ForceMode.Force);
      }
   }
}