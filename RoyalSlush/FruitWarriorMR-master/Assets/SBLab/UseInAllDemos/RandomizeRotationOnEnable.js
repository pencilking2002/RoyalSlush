#pragma strict

var amplitude : Vector3;

function OnEnable () {
	transform.localRotation = transform.localRotation * Quaternion.Euler(Vector3(Random.Range(-amplitude.x, amplitude.x), Random.Range(-amplitude.y, amplitude.y), Random.Range(-amplitude.z, amplitude.z)));
}