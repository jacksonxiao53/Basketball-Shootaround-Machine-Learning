using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreboardPostTraining : MonoBehaviour
{
    public TextMeshPro TextMeshPro;

    // Update is called once per frame
    void Update()
    {
        TextMeshPro.text = String.Format("FG % With Training: {0:0.00}%",
            ((float)BallController.SuccessCountLR / (float)BallController.ShotCountLR) * 100f);

    }
}
