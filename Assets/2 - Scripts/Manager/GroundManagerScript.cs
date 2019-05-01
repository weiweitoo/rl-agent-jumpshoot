﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManagerScript : MonoBehaviour {

	public GameObject groundPrefab;
	public GameObject groundHolder;
	public int initOffSet = -4;
	public int distanceToNextGround = 8;
	public int initialGround = 5;
	public float groundWidth = 3;
	public float groundHeight = 0.6f;
	public float minVelocity = 1f;
	public float maxVelocity = 2f;

	[ReadOnly] public int groundIndex = 0;
	[Header("Difficulty Setting")]
	public int difficultyGrowRate = 10;
	public float widthGrow = 0.05f;
	public float velocityGrow = 0.05f;	
	public float widthGrowRateAdjust = 0.1f;
	public float velocityGrowRateAdjust = 0.1f;	
	[ReadOnly] public int difficultyLevel = 1;

	public void InitGround(){
		foreach (Transform child in groundHolder.transform) {
			GameObject.Destroy(child.gameObject);
		}
		groundIndex = 0;

		Vector2 position = new Vector2(0,0);
		GameObject newGroundObj = Instantiate(groundPrefab,position,Quaternion.identity);
		newGroundObj.transform.SetParent(groundHolder.transform);

		for(int i = 0; i < initialGround;i++){
			GenerateGround();
		}
	}

	public void GenerateGround(){
		DifficultyCalculation();
		Vector2 position = new Vector2(0,initOffSet + groundIndex * distanceToNextGround);
		GameObject newGroundObj = Instantiate(groundPrefab,position,Quaternion.identity);
		newGroundObj.transform.SetParent(groundHolder.transform);
		SetGroundAttribute(newGroundObj);
		groundIndex++;
	}

	void SetGroundAttribute(GameObject obj){
		// Random Type
		float velocity = 0;
		Vector2 newScale = new Vector2(groundWidth,groundHeight);

		velocity = Random.Range(minVelocity,maxVelocity);
		newScale = new Vector2(groundWidth,groundHeight);

		obj.GetComponent<GroundScript>().SetGround(newScale,0,0,velocity);
	}

	void DifficultyCalculation(){
		if(groundIndex > difficultyLevel * difficultyGrowRate){
			difficultyLevel = 1 + (groundIndex / difficultyGrowRate);
			float widthDifference = (1 - Mathf.Sin(Deg2Rad(difficultyLevel * widthGrowRateAdjust))) * widthGrow;
			float velocityDifference = (1 - Mathf.Sin(Deg2Rad(difficultyLevel * velocityGrowRateAdjust))) * velocityGrow;
			groundWidth -= widthDifference;
			minVelocity += velocityDifference;
			maxVelocity += velocityDifference;
		}
	}

	float Deg2Rad(float deg){
		return deg * Mathf.PI / 180;
	}
}
