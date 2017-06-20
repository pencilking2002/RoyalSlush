#pragma strict

var speed : Vector3;
var useFixedUpdate : boolean;

function FixedUpdate () {
	if(useFixedUpdate){
		transform.Rotate(speed * Time.fixedDeltaTime);
	}
}
function Update () {
	if(!useFixedUpdate){
		transform.Rotate(speed * Time.deltaTime);
	}
}