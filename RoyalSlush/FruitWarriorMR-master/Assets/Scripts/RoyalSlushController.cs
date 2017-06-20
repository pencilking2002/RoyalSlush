using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoyalSlushController : MonoBehaviour, IInputClickHandler
{
   
    public void OnInputClicked(InputEventData eventData)
    {
       // SceneManager.LoadScene("Main");
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OnWatermelon();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            OnGrapes();
        }

		if (Input.GetKeyDown(KeyCode.I))
		{
			OnStrawberry();
		}
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnBroccolli();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnBanana();
        }
    }

    void OnStart()
    {
        SceneManager.LoadScene("Main");
    }


    void OnWatermelon()
    {
		Instantiate(Resources.Load("Watermelon"), transform.position + randomOffset(), Random.rotation );
    }

    void OnGrapes()
    {
		Instantiate(Resources.Load("Grapes"), transform.position + randomOffset(), Random.rotation);
    }

	void OnStrawberry()
	{
		Instantiate(Resources.Load("Strawberry"), transform.position + randomOffset(), Random.rotation);
	}

    void OnBroccolli()
    {
        Instantiate(Resources.Load("Broccolli"), transform.position + randomOffset(), Random.rotation);
    }

    void OnBanana()
    {
        Instantiate(Resources.Load("Banana"), transform.position + randomOffset(), Random.rotation);
    }

    Vector3 randomOffset(){

		float num = 0.02f;

		return new Vector3(Random.Range(-1 * num, num),Random.Range(-1 * num, num),Random.Range(-1 * num, num ));
	}
}
