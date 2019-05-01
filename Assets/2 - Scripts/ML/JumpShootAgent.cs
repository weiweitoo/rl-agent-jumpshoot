using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;

public class JumpShootAgent : Agent
{
    public GameObject area;
    private PlayerScript playerComponent;
    // private Rigidbody2D rigidbody2dComponent;
    private JumpShootAcademy agentAcademy;
    private JumpShootArea agentArea;
    private RayPerception2D rayPerception;
    public TextMeshPro scoreText;
    private float reward;

    /// <summary>
    /// Initialize the agent
    /// </summary>
    public override void InitializeAgent()
    {
        StartCoroutine(WaitAwhile());
        base.InitializeAgent();
        agentAcademy = FindObjectOfType<JumpShootAcademy>();
        playerComponent = GetComponent<PlayerScript>();
        rayPerception = GetComponent<RayPerception2D>();
        // agentArea = area.GetComponent<JumpShootArea>();
    }

    IEnumerator WaitAwhile(){
        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// Collect all observations that the agent will use to make decisions
    /// </summary>
    public override void CollectObservations(){
        // float rayDistance = 6f;
        // float[] rayAngles = { 30f, 150f };
        // float rayDistanceShort = 4f;
        // float[] rayAnglesShort = { 60f, 120f, 90f };
        // float rayDistanceDown = 2.5f;
        // float[] rayAnglesDown = { 250f, 290f };
        // string[] detectableObjects = { "Ground" };
        // rayPerception = GetComponent<RayPerception2D>();
        
        // AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects));
        // AddVectorObs(rayPerception.Perceive(rayDistanceShort, rayAnglesShort, detectableObjects));
        // AddVectorObs(rayPerception.Perceive(rayDistanceDown, rayAnglesDown, detectableObjects));

        // GroundScript parent = transform.parent.GetComponent<GroundScript>();
        // float angle = 0;
        // if(parent != null){
        //     angle = parent.angle;
        // }
        if(agentArea == null){
            agentArea = area.GetComponent<JumpShootArea>();
        }
        AddVectorObs(agentArea.GetGround(playerComponent.stepCount-1).localPosition.x);
        AddVectorObs(agentArea.GetGround(playerComponent.stepCount-1).localPosition.y);
        AddVectorObs(Mathf.Sin(agentArea.GetGround(playerComponent.stepCount-1).GetComponent<GroundScript>().GetVelocity()));
        AddVectorObs(transform.localPosition.x);
        AddVectorObs(transform.localPosition.y);
        AddVectorObs(playerComponent.ParentVelocity());;
        AddVectorObs((int)playerComponent.currentPlayerState,3);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
       if(vectorAction[0] == 1){
           AddReward(playerComponent.InputJump());
           Debug.Log("I wanna jump");
       }
       else{
            Debug.Log("I dont like jump");
       }

    //    if(vectorAction[1] == 1){
    //        AddReward(playerComponent.InputFall());
    //        Debug.Log("I wanna Fall");
    //    }
    //    else{
    //         Debug.Log("I dont like fall");
    //    }

       if(playerComponent.currentPlayerState == PlayerScript.PlayerState.Jumping){
			// AddReward(0.1f);
		}
		else if(playerComponent.currentPlayerState == PlayerScript.PlayerState.Falling){
			// AddReward(0.01f);
		}
		else{
			AddReward(0.3f);
		}

        // Reward
        if (GetCumulativeReward() <= -15f){
            EspisodeDone();
        }
        else{
            // AddUpdateReward(-0.001f);
            // Each step minus some reward
        }

        AddReward(reward);
        area.GetComponent<JumpShootArea>().UpdateScore(GetCumulativeReward());
        reward = 0; 
    }

    public void AddUpdateReward(float r){
        reward += r;
    }

    public void EspisodeDone(){
        agentAcademy.AddDone();
    }

    private float MinMaxNormalize(float val, float max, float min){
        return (val - min)/(max - min);
    }
}

