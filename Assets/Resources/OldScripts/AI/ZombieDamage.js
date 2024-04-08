var maximumHitPoints = 100.0;
var hitPoints = 100.0;
var deadReplacement : Rigidbody;
var GOPos : GameObject;
var scoreManager : ScoreManager;

function Start(){	
	scoreManager = gameObject.Find("ScoreManager").GetComponent("ScoreManager");
}

function ApplyDamage (damage : float) {
	if (hitPoints <= 0.0)
		return;

	// Apply damage
	hitPoints -= damage;
	scoreManager.DrawCrosshair();
	// Are we dead?
	if (hitPoints <= 0.0)
		Replace(); 
}

function Replace() {

	// If we have a dead barrel then replace ourselves with it!
	if (deadReplacement) {
		var dead : Rigidbody = Instantiate(deadReplacement, GOPos.transform.position, GOPos.transform.rotation);
		scoreManager.addScore(20);
		// For better effect we assign the same velocity to the exploded barrel
		dead.rigidbody.velocity = rigidbody.velocity;
		dead.angularVelocity = rigidbody.angularVelocity;
		 //Debug.Log(dead);
		//Destroy(gameObject);
		 //Destroy(dead, 1); 
		//  Destroy (dead, 1.0f);
    }
   
	// Destroy ourselves
	Destroy(gameObject);

	Destroy(GameObject.Find("zombiePref3(Clone)"), 5);
	Destroy(GameObject.Find("FatRagdoll1(Clone)"), 5);
	Destroy(GameObject.Find("FatRagdoll2(Clone)"), 5);
	Destroy(GameObject.Find("FatRagdoll3(Clone)"), 5);
	Destroy(GameObject.Find("FatRagdoll4(Clone)"), 5);
	Destroy(GameObject.Find("FatRagdoll5(Clone)"), 5);
	Destroy(GameObject.Find("FemaleRagdoll(Clone)"), 5);
	Destroy(GameObject.Find("MaleRagdoll(Clone)"), 5);
	
}




