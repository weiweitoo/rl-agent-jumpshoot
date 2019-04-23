using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;

public class JumpShootArea : Area
{
	public GameObject playerPrefab;
	public GameObject playerParent;
	public GameObject groundPrefab;
	public GameObject groundHolder;
    public GameObject followCamera;
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
    public TextMeshPro scoreText;
    private int step;
    public TextMeshPro stepText;
    private int stepBest;
    public TextMeshPro stepBestText;
    private GameObject currentPlayer;

	public void InitGround(){
		foreach (Transform child in groundHolder.transform) {
			GameObject.Destroy(child.gameObject);
		}
		groundIndex = 0;

		Vector2 position = new Vector2(0,-3.5f);
		GameObject newGroundObj = Instantiate(groundPrefab,position,Quaternion.identity);
		newGroundObj.transform.SetParent(groundHolder.transform);
        SetGroundAttribute(newGroundObj);
        // newGroundObj.GetComponent<GroundScript>().velocity = 0;

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

	void SetGroundAttribute(GameObject obj,bool movement = false){

		// Random Type
		GroundScript.GroundType groundType = (GroundScript.GroundType)Random.Range(0, 3);
		float velocity = 0;
		Vector2 newScale = new Vector2(groundWidth,groundHeight);

		if(groundType == GroundScript.GroundType.Normal){
			velocity = Random.Range(minVelocity,maxVelocity);
			newScale = new Vector2(groundWidth,groundHeight);
		}
		else if(groundType == GroundScript.GroundType.JumpHigh){
			velocity = Random.Range(minVelocity,maxVelocity);
			obj.GetComponent<GroundScript>().SetColor(10,10,10);
			newScale = new Vector2(groundWidth * 0.8f,groundHeight * 0.75f);
		}
		else if(groundType == GroundScript.GroundType.TimeBomb){
			velocity = Random.Range(minVelocity * 1.25f,maxVelocity * 1.25f);
			newScale = new Vector2(groundWidth,groundHeight);
		}

		obj.GetComponent<GroundScript>().SetGround(newScale,0,0,velocity,groundType);
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

    /// <summary>
    /// Resets the area
    /// </summary>
    /// <param name="agents"></param>
    public override void ResetArea(){
        if(currentPlayer != null){
            Destroy(currentPlayer);
        }

        InitGround();
		Vector2 position = new Vector2(0,-2.6f);
        GameObject newPlayer = Instantiate(playerPrefab,position,Quaternion.identity);
		newPlayer.transform.SetParent(playerParent.transform);
        newPlayer.GetComponent<PlayerScript>().area = transform.gameObject;
        newPlayer.GetComponent<PlayerScript>().playerParent = transform.FindChild("PlayerParent").gameObject;
        newPlayer.GetComponent<JumpShootAgent>().area = transform.gameObject;
        followCamera.GetComponent<CameraFollowPlayerScript>().Reset();
        followCamera.GetComponent<CameraFollowPlayerScript>().player = newPlayer.gameObject;
        currentPlayer = newPlayer;

        step = 0;
    }

    public void UpdateScore(float score){
        scoreText.text = score.ToString("0.00");
    }

    public void StepSuccess(){
        step++;
        stepText.text = step.ToString();

        if(step > stepBest){
            stepBest = step;
            stepBestText.text = stepBest.ToString();
        }

        if(step > initialGround){
            currentPlayer.GetComponent<JumpShootAgent>().AddUpdateReward(1.0f);
            currentPlayer.GetComponent<JumpShootAgent>().EspisodeDone();
            Debug.Log("Reach the Top");
        }
    }
}
