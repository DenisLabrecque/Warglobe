using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using DGL;
using DGL.Math;

class SpeedFollowCamera2 : CameraEmplacement
{
   Vehicle _target;
   [SerializeField] float _minDistance = 100f;
   [SerializeField] float _maxDistance = 400f;
   float _distance;
   [SerializeField] float _maxHeight = 150.0f;
   float _height;
   [SerializeField] float _distanceDamping = 4f;
   [SerializeField] float _rotationDamping = 4f;
   [SerializeField] float _zoomSpeed = 100f;

   void Start()
   {
      _distance = (_maxDistance - _minDistance) / 2;
      _target = GetComponentInParent<Vehicle>();
   }

   void Update()
   {
      CalculateZoomDistance();
      CalculateHeight();

      

      FollowBehind();
      RotateCameraWithCursor();
      //RotateTowardsVector();
   }

   void RotateCameraWithCursor()
   {
      // Find rotation plane
      //Vector3 rotationPlane = _target.transform.TransformPoint(Vector3.zero);

      transform.RotateAround(_target.transform.position, transform.up, Input.GetAxis("Mouse X") * _rotationDamping);
   }

   void CalculateZoomDistance()
   {
      if(UserInput.ScrollWheel > 0)
      {
         Debug.Log("Distance is " + _distance);
         _distance -= _zoomSpeed;
      }
      else if(UserInput.ScrollWheel < 0)
      {
         Debug.Log("Distance is " + _distance);
         _distance += _zoomSpeed;
      }
      _distance = Mathf.Clamp(_distance, _minDistance, _maxDistance);
   }

   void CalculateHeight()
   {
      // Calculate height as percent of distance (higher points down, closer points forwards)
      _height = _maxHeight * Utility.Percent(_distance, _maxDistance, PercentMode.Clamp0To1);
   }

   void FollowBehind()
   {
      Vector3 wantedPosition = _target.transform.TransformPoint(0, _height, -_distance);
      transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * _distanceDamping);
   }

   void RotateTowardsVector()
   {
      Quaternion wantedRotation = Quaternion.LookRotation(_target.PositionInSeconds(10) - transform.position, _target.transform.up);
      transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * _rotationDamping);
   }
}