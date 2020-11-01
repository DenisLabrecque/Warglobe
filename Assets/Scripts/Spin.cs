using UnityEngine;

public class Spin : MonoBehaviour
{
   [SerializeField] float _degreesPerSecond = 90f;
   [SerializeField] GameObject _spinee = null;
   [SerializeField] Vector3 _axis = new Vector3(0, 0, 1);
   bool _isOn = true;

   private void Awake()
   {
      if (_spinee == null)
         _spinee = gameObject;
   }

   // Update is called once per frame
   void Update()
   {
      if (_isOn)
         _spinee.transform.Rotate(_axis * _degreesPerSecond * Time.deltaTime, Space.Self);
   }

   public void Switch(bool onOff)
   {
      _isOn = onOff;
   }
}
