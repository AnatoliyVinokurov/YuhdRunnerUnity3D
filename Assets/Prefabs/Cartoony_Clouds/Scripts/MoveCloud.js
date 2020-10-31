var moveDir : Vector3;
var random : float;
var moveSpeed : Vector3;
function Start () {
	var randomSpeed = Random.Range(0,random);
	moveDir.z += randomSpeed;
}

function Update () {
	transform.Translate(moveDir*Time.deltaTime);
	if(transform.localPosition.z < -1000){
		transform.localPosition.z = 1000;
	}
	if(transform.localPosition.z > 1000){
		transform.localPosition.z = -1000;
	}
}

