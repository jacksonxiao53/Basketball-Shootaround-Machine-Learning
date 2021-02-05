using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class BallSpawner : MonoBehaviour


{
	public Transform TransformGoal = null;
	public GameObject PrefabBall;

	public static bool FinishedCollectingData = false;
	public static bool DoneLinearRegression = false;

	private double linearRegressionSlope;
	private double linearRegressionYIntercept;
	
	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(Shoot());
    }


	IEnumerator Shoot()
    {
		while(true)
        {
			Debug.Log(BallController.SuccessCount);
			if(BallController.SuccessCount == 1000)
            {
				FinishedCollectingData = true;
            }
			if(!FinishedCollectingData && !DoneLinearRegression)
            {
				var hoopVector2 = new Vector2(
				TransformGoal.position.x,
				TransformGoal.position.z);

				var playerVector2 = new Vector2(
					transform.position.x, transform.position.z);

				var dir = (hoopVector2 - playerVector2).normalized;
				var dist = (hoopVector2 - playerVector2).magnitude;
				var arch = 0.5f;

				var closeness = Math.Min(10f, dist) / 10f;

				float force = GetForceRandomly(dist);

				var ball = Instantiate(PrefabBall, transform.position, Quaternion.identity);
				var ballController = ball.GetComponent<BallController>();
				ballController.Force = new Vector3(
					dir.x * arch * closeness,
					force,
					dir.y * arch * closeness
				);
				ballController.Distance = dist;

				yield return new WaitForSeconds(0.02f);
				MoveToRandomDistance();
			}
			else if (FinishedCollectingData && !DoneLinearRegression)
            {
				DoLinearRegression();
				yield return new WaitForSeconds(1f);
				DoneLinearRegression = true;

				var xList = BallController.xDist;
				var yList = BallController.yForce;

				for(int i = 0; i < xList.Count; i++)
                {
					Debug.Log(xList[i] + " " + yList[i]);
                }
				Debug.Log("----------------------");
				Debug.Log(linearRegressionSlope);
				Debug.Log(linearRegressionYIntercept);
            }
            else
            {
				var hoopVector2 = new Vector2(
				TransformGoal.position.x,
				TransformGoal.position.z);

				var playerVector2 = new Vector2(
					transform.position.x, transform.position.z);

				var dir = (hoopVector2 - playerVector2).normalized;
				var dist = (hoopVector2 - playerVector2).magnitude;
				var arch = 0.5f;

				var closeness = Math.Min(10f, dist) / 10f;

				float force = (float) GetForceLinearRegression(dist);

				var ball = Instantiate(PrefabBall, transform.position, Quaternion.identity);
				var ballController = ball.GetComponent<BallController>();
				ballController.Force = new Vector3(
					dir.x * arch * closeness,
					force,//* (1f / closeness),// Optional: Uncomment this to experiment with artificial shot arcs!
					dir.y * arch * closeness
				);
				ballController.Distance = dist;

				yield return new WaitForSeconds(0.02f);
				MoveToRandomDistance();
			}
			
		}
    }

	float GetForceRandomly(float distance)
	{
		return Random.Range(0f, 1f);
	}

	void MoveToRandomDistance()
	{
		var newPosition = new Vector3(TransformGoal.position.x + Random.Range(2.5f, 23f), transform.parent.position.y, TransformGoal.position.z);
		transform.parent.position = newPosition;
	}

	void DoLinearRegression()
	{
		var xList = BallController.xDist;
		var yList = BallController.yForce;

		double count = Convert.ToDouble(xList.Count);

		double xSum = 0.0;
		double ySum = 0.0;
		double xySum = 0.0;
		double xxSum = 0.0;
		double yySum = 0.0;
		

		for(int i = 0; i < xList.Count; i++)
        {
			double xVal = Convert.ToDouble(xList[i]);
			double yVal = Convert.ToDouble(yList[i]);

			xSum += xVal;
			ySum += yVal;

			xySum += (xVal * yVal);

			xxSum += (xVal * xVal);

			yySum += (yVal * yVal);
        }

		double slopeNum = (count * xySum) - (xSum * ySum);
		double slopeDen = (count * xxSum) - (xSum * xSum);
		linearRegressionSlope = slopeNum / slopeDen;

		linearRegressionYIntercept = (ySum - (linearRegressionSlope * xSum)) / count;
	}

	public double GetLinearRegressionSlope()
    {
		return linearRegressionSlope;
    }

	public double GetForceLinearRegression(float distance)
    {
		return linearRegressionSlope * distance + linearRegressionYIntercept;
    }


}
