using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class JumpShootAcademy : Academy
{
    private JumpShootArea[] areas;
    // public JumpShootAgent agent;
    public bool done = false;

    public override void AcademyStep(){
        if(done)
        {
            Done();
        }
    }

    /// <summary>
    /// Reset the academy
    /// </summary>
    public override void AcademyReset(){
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
        done = false;
    }

}
