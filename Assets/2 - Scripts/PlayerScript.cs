using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public enum PlayerState{
		Standing,Jumping,Falling
	}
	
    public GameObject area;
	public GameObject playerParent;
	public GameObject shootEffectPrefab;
	public GameObject landingEffectPrefab;
	public GameObject wallEffectPrefab;
	public float jumpSpeed = 10f;
	public float alwaysLeftSpeed = 3f;
	public float alwaysRightSpeed = 3f;
	public float throwSpeed = 1.5f;
	public float fallSpeed = 20f;
	public float stunTime = 1f;
	public float wallAdjustment;
	[ReadOnly] public bool isDead = false;
	[ReadOnly] public float previousPosXParent;
	[ReadOnly] public float ScreenRadiusInWorldX;
	[ReadOnly] public PlayerState currentPlayerState;
	Rigidbody2D rigidBody2DComponent;
	BoxCollider2D boxCollider2D;
	JumpShootAgent agent;
	public int stepCount = 0;
	
	void Awake () {
		rigidBody2DComponent = GetComponent<Rigidbody2D>();
		boxCollider2D = GetComponent<BoxCollider2D>();
		agent = GetComponent<JumpShootAgent>();
		currentPlayerState = PlayerState.Jumping;
	}

	void Start(){
	}
	
	void Update () {
		GetInput();
		FallChecking();
		// BounceAtWall();
		GetPreviousPositionOfParent();
		DeadCheck();
	}

	void OnCollisionEnter2D(Collision2D target){
		if(isDead){
			return;
		}

		// Get Standing Groud Type
		GroundScript groundScriptComponent = target.gameObject.GetComponent<GroundScript>();

		rigidBody2DComponent.velocity = Vector2.zero;
		currentPlayerState = PlayerState.Standing;
		transform.SetParent(target.gameObject.transform);
		GetPreviousPositionOfParent();
		if(groundScriptComponent.GetStepped() == false){
			groundScriptComponent.Stepped();
			agent.AddUpdateReward(0.5f);
			Debug.Log("Step success");
			stepCount++;
			CheckReachTop();
		}
		else
		{
			isDead = true;
			Debug.Log("Step Repeated");
			agent.AddUpdateReward(-1f);
			agent.EspisodeDone();
			StopPlayer();
		}
		
		GameObject landingEffect = Instantiate(landingEffectPrefab,transform.position, Quaternion.identity);
		Destroy(landingEffect,0.1f);
		
	}

	IEnumerator OnCollisionExit2D(Collision2D target){
		// yield return new WaitForSeconds(0.2f);
		// if(currentPlayerState == PlayerState.Jumping){
			// area.GetComponent<JumpShootArea>().GenerateGround();
			// agent.AddUpdateReward(0.1f);
			// try{
				// Debug.Log(target.gameObject);
				// Destroy(target.gameObject);
				// Debug.Log("Destroy");
			// }
			// catch{
				// do nothing. just want to hide the error
			// }
		// }
		yield break;
	}

	void CheckReachTop(){
		if(stepCount > area.GetComponent<JumpShootArea>().initialGround){
			PlayerWin();
			agent.AddUpdateReward(1f);
			agent.EspisodeDone();
		}
	}

	void GetPreviousPositionOfParent(){
		previousPosXParent = transform.parent.transform.position.x;
	}

	public float ParentVelocity(){
		return (transform.parent.transform.position.x - previousPosXParent) * throwSpeed / Time.deltaTime;
	}

	void GetInput(){
		if (Input.GetMouseButtonDown(0)){
			if(currentPlayerState == PlayerState.Jumping){
				StartCoroutine(Fall());
			}
			else if(currentPlayerState == PlayerState.Standing){
				Jump();
			}
		}
	}

	public float InputJump(){
		if(currentPlayerState == PlayerState.Standing){
			Jump();
			return 0;
		}
		return 0;
	}
	public float InputFall(){
		if(currentPlayerState == PlayerState.Standing){
			StartCoroutine(Fall());
			return 0;
		}
		return 0;
	}

	void Jump(){
		boxCollider2D.enabled = false;
		currentPlayerState = PlayerState.Jumping;

		rigidBody2DComponent.velocity = new Vector2(0,jumpSpeed);
		// rigidBody2DComponent.velocity = new Vector2(ParentVelocity(),jumpSpeed);
	
		transform.SetParent(playerParent.transform);
	}

	void DeadCheck(){
		if(isDead == false && Camera.main.transform.position.y - transform.position.y > 10){
			isDead = true;
			StopPlayer();
			agent.AddUpdateReward(-1f);
			agent.EspisodeDone();
		}
	}

	void StopPlayer(){
		rigidBody2DComponent.isKinematic = true;
		rigidBody2DComponent.velocity = new Vector2(0,0);
		// rigidBody2DComponent.gravityScale = 0;
		// rigidBody2DComponent.constraints = RigidbodyConstraints2D.FreezePositionY;
		GetComponent<SpriteRenderer>().color = new Color(1f,0.30196078f, 0.30196078f);
	}

	void PlayerWin(){
		rigidBody2DComponent.isKinematic = true;
		rigidBody2DComponent.velocity = new Vector2(0,0);
		rigidBody2DComponent.gravityScale = 0;
		// rigidBody2DComponent.constraints = RigidbodyConstraints2D.FreezePositionY;
		GetComponent<SpriteRenderer>().color = new Color(0.30196078f,1f, 0.30196078f);
	}		
	
	public void RevivePlayer(){
		ContinuePlayer();
		isDead = false;
	}

	void ContinuePlayer(){
		rigidBody2DComponent.isKinematic = false;
	}

	void PlayWallBounceEffect(){
		if(currentPlayerState == PlayerState.Jumping){
			GameObject wallEffect = Instantiate(wallEffectPrefab,transform.position, Quaternion.identity);
			Destroy(wallEffect,0.2f); 
		}
	}
	void FallChecking(){
		if(rigidBody2DComponent.velocity.y <= 0){
			rigidBody2DComponent.isKinematic = false;
			boxCollider2D.enabled = true;
		}
	}

	IEnumerator Fall(){
		currentPlayerState = PlayerState.Falling;
		boxCollider2D.enabled = true;

		rigidBody2DComponent.isKinematic = true;
		rigidBody2DComponent.velocity = new Vector2(0,0);

		yield return new WaitForSeconds(stunTime);

		rigidBody2DComponent.isKinematic = false;
		rigidBody2DComponent.velocity = new Vector2(0,-fallSpeed);
		yield break;
	}


}
