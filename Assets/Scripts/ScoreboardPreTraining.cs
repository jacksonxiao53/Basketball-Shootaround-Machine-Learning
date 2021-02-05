using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreboardPreTraining : MonoBehaviour
{
    public TextMeshPro TextMeshPro;

    // Update is called once per frame
    void Update()
    {
        TextMeshPro.text = String.Format("FG % Without Training: {0:0.00}%",
            ((float)BallController.SuccessCount / (float)BallController.ShotCount) * 100f);
    }
}
