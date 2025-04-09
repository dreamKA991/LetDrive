using UnityEngine;
using Zenject;
public class CameraFollow : MonoBehaviour {

	private Transform _carTransform;
	private float followSpeed = 10;
	private float lookSpeed = 10;
	private Vector3 initialCameraPosition;
	private Vector3 initialCarPosition;
	private Vector3 absoluteInitCameraPosition;
	private Vector3 _menuStartPosition = new Vector3(10f,3f, 6.4f);
	private Vector3 _gamePlayStartPosition = new Vector3(0, 0, 0);

	public void Init(ICharacterCar iCharacterCar, bool isUseGamePlayTransform)
	{
		_carTransform = iCharacterCar.GetTransform();
		if(isUseGamePlayTransform) transform.position = _gamePlayStartPosition;
		else transform.position = _menuStartPosition;
		StartWork();
	}

	private void StartWork(){
		initialCameraPosition = gameObject.transform.position;
		initialCarPosition = _carTransform.position;
		absoluteInitCameraPosition = initialCameraPosition - initialCarPosition;
	}

	private void FixedUpdate()
	{
		//Look at car
		Vector3 _lookDirection = (new Vector3(_carTransform.position.x, _carTransform.position.y, _carTransform.position.z)) - transform.position;
		Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);

		//Move to car
		Vector3 _targetPos = absoluteInitCameraPosition + _carTransform.transform.position;
		transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);

	}

}
