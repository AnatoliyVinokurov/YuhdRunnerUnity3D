var cloud : Transform;
var dist : float;
var amount : int;
var hightRandom : float = 100;
@HideInInspector
@SerializeField
private var clouds : GameObject[];

function New () {
	DeleteClouds();
	clouds = new GameObject[amount];
	for(i=0;i<clouds.length;i++){
		var newCloud = Instantiate(cloud,transform.position+Vector3(Random.Range(-dist,dist),Random.Range(-hightRandom,hightRandom),Random.Range(-dist,dist)),transform.rotation);
		newCloud.parent = transform;
		
		
		clouds[i] = newCloud.gameObject;
	}
	
}

function DeleteClouds () {
	for(i=0;i<clouds.length;i++){
		DestroyImmediate(clouds[i].gameObject);
		
	}
	clouds = new GameObject [0];
}