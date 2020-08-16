using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Define a camera position inherited as static, inside, or follow, etc.
/// </summary>
public abstract class CameraEmplacement : MonoBehaviour {

   #region Properties

   [SerializeField] int _fieldOfView = 80;

   protected Camera _camera = null;

   #endregion


   #region Public Methods

   /// <summary>
   /// Attach a camera to this emplacement.
   /// </summary>
   public void Attach(Camera camera)
   {
      // Activate this camera emplacement
      enabled = true;
      _camera = camera;

      // Attach the camera to this object
      camera.transform.parent = transform;

      // Set the camera's properties to match the emplacement's
      MatchEmplacementProperties(camera);
   }

   // Deactivate this camera emplacement
   public void Detach()
   {
      _camera = null;
      enabled = false;
   }

   #endregion


   #region Private Methods

   /// <summary>
   /// Ensure that camera FOV and other properties are matched to those defined in this class.
   /// </summary>
   public virtual void MatchEmplacementProperties(Camera camera)
   {
      // Set the camera's position to this object
      camera.transform.localPosition = Vector3.zero;

      // Set the camera's rotation to that of this object
      camera.transform.localRotation = transform.localRotation;

      // FOV
      camera.fieldOfView = _fieldOfView;
   }

   #endregion
}
