using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Vector3 Force;
    public float Distance;
    
	public static int SuccessCount = 0;
    public static int ShotCount = 0;
	public static ArrayList xDist = new ArrayList();
	public static ArrayList yForce = new ArrayList();
	public static int SuccessCountLR = 0;
	public static int ShotCountLR = 1;

    public Material MaterialBallScored;
    private Vector3 Scaler = new Vector3(1000, 1000, 1000);

    private bool hasBeenScored = false;
    // Start is called before the first frame update
    void Start()
    {
        var scaledForce = Vector3.Scale(Scaler, Force);
        GetComponent<Rigidbody>().AddForce(scaledForce);
        StartCoroutine(DoDespawn(30));
		if(!BallSpawner.FinishedCollectingData && !BallSpawner.DoneLinearRegression)
        {
			ShotCount++;
        }
        else if (BallSpawner.FinishedCollectingData && BallSpawner.DoneLinearRegression)
        {
			ShotCountLR++;
        }
    }
	IEnumerator DoDespawn(float delay)
	{
		yield return new WaitForSeconds(delay);
		Destroy(gameObject);
	}

	private bool hasTriggeredTop = false;

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.name == "Floor")
		{
			StartCoroutine(DoDespawn(2.5f));
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "TopTrigger")
		{
			hasTriggeredTop = true;
		}
		else if (other.name == "BottomTrigger")
		{
			if (hasTriggeredTop && !hasBeenScored)
			{
				GetComponent<Renderer>().material = MaterialBallScored;
				if(!BallSpawner.FinishedCollectingData && !BallSpawner.DoneLinearRegression)
                {
					SuccessCount++;
                }
                else
                {
					SuccessCountLR++;
                }
				xDist.Add(Distance);
				yForce.Add(Force.y);

			}
			hasBeenScored = true;
		}
	}

}
