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
   [SerializeField] GameObject marker; // temporary


   Vehicle _target;
   [SerializeField] float _minDistance = 100f;
   [SerializeField] float _maxDistance = 400f;
   float _distance;
   [SerializeField] float _maxHeight = 150.0f;
   float _height;
   [SerializeField] float _distanceDamping = 100.0f;
   [SerializeField] float _rotationDamping = 100.0f;

   void Start()
   {
      _distance = (_maxDistance - _minDistance) / 2;
      _target = GetComponentInParent<Vehicle>();
   }

   void Update()
   {
      // Show where the vector is
      marker.transform.position = _target.PositionInSeconds(3);

      // Calculate zoom distance
      if(UserInput.ScrollWheel > 0)
      {
         _distance -= 1000 * Time.deltaTime;
      }
      else if(UserInput.ScrollWheel < 0)
      {
         _distance += 1000 * Time.deltaTime;
      }
      _distance = Mathf.Clamp(_distance, _minDistance, _maxDistance);

      // Calculate height as percent of distance (higher points down, closer points forwards)
      _height = _maxHeight * Utility.Percent(_distance, _maxDistance, PercentMode.Clamp0To1);

      // Follow behind
      Vector3 wantedPosition = _target.transform.TransformPoint(0, _height, -_distance);
      transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * _distanceDamping);

      // Smooth rotation
      Quaternion wantedRotation = Quaternion.LookRotation(_target.PositionInSeconds(10) - transform.position, _target.transform.up);
      transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * _rotationDamping);
   }
}