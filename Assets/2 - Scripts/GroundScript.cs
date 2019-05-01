using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour {
	public float minDistanceFromScreen = 0f;
	public float maxDistanceFromScreen = 0f;
	public float wallAdjustment = 0.5f;
	public float shakeEffect = 0.4f;
	[ReadOnly] public float velocity = 1f;
	[ReadOnly] public float distance;
	[ReadOnly] public float screenWidth;
	[ReadOnly] public float angle = 0;
	[ReadOnly] public bool stepped = false;
	PlayerScript playerScript;
	public float xOffset;
	public float yOffset;
	private Vector3 prevPos;

	void Start(){
		distance = 4.5f;
	}

	void Update () {
		Move();
	}

	void Move(){
		prevPos = transform.position;
		transform.position = new Vector2((Mathf.Sin(angle) * distance)+xOffset, transform.position.y+yOffset);
		angle += (velocity / 1f) * (Time.deltaTime + 0.003F);
	}

	public float GetVelocity(){
		return (transform.position.x - prevPos.x) / Time.deltaTime;
	}

	public void SetGround(Vector2 scale,float minDistanceFromScreen, float maxDistanceFromScreen, float velocity){
		this.transform.localScale = scale;
		this.minDistanceFromScreen = minDistanceFromScreen;
		this.maxDistanceFromScreen = maxDistanceFromScreen;
		this.velocity = velocity;
	}

	public void Stepped(){
		stepped = true;
	}

	public bool GetStepped(){
		return stepped;
	}
}
