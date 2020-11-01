using UnityEngine;
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

      RotateCameraWithCursor();
   }

   void CalculateZoomDistance()
   {
      if(UserInput.ScrollWheel > 0)
         _distance -= _zoomSpeed;
      else if(UserInput.ScrollWheel < 0)
         _distance += _zoomSpeed;

      _distance = Mathf.Clamp(_distance, _minDistance, _maxDistance);
   }

   // Calculate height as percent of distance (higher points down, closer points forwards)
   void CalculateHeight()
   {
      _height = _maxHeight * Utility.Percent(_distance, _maxDistance, PercentMode.Clamp0To1);
   }

   void RotateCameraWithCursor()
   {
      // Rotate around the vehicle
      Transform oldTransform = transform;
      transform.RotateAround(_target.transform.position, transform.up, Input.GetAxis("Mouse X") * _rotationDamping);
      Transform newTransform = transform;

      // Add distance
      Vector3 distance = _target.transform.position + (newTransform.TransformDirection(Vector3.forward) * -_distance) + (newTransform.TransformDirection(Vector3.up) * _height);

      // Lerp
      transform.position = Vector3.Lerp(oldTransform.position, distance, Time.deltaTime * _distanceDamping);
   }

   void RotateTowardsVector()
   {
      Quaternion wantedRotation = Quaternion.LookRotation(_target.PositionInSeconds(10) - transform.position, _target.transform.up);
      transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * _rotationDamping);
   }
}