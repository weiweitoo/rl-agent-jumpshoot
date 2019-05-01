using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class JumpShootAcademy : Academy
{
    private JumpShootArea[] areas;
    // public JumpShootAgent agent;
    public int doneCount = 0;
    public int areaCount;

    void Start(){
        areaCount = GameObject.FindObjectsOfType<JumpShootArea>().Length;
    }

    public void AddDone(){
        doneCount++;
    }

    public override void AcademyStep(){        
        if(doneCount >= areaCount)
        {
            Done();
            doneCount = 0;
        }
    }

    /// <summary>
    /// Reset the academy
    /// </summary>
    public override void AcademyReset(){
        StartCoroutine(Wait());
        
    }

    IEnumerator Wait(){
        // yield return new WaitForSecondsRealtime(2f);
        if (areas == null)
        {
            areas = GameObject.FindObjectsOfType<JumpShootArea>();
        }

        foreach (JumpShootArea area in areas)
        {
            area.groundWidth = resetParameters["ground_width"];
            area.initialGround = (int)resetParameters["initial_ground"];
            area.ResetArea();
        }
        yield break;
    }

}
