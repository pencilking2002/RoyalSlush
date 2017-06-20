using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoyalSlushController : MonoBehaviour, IInputClickHandler
{
   	public Customer currentCustomer;
   	public RoyalSlushController Instance;

   	private void Awake()
   	{
   		InitInstance();
   	}


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

        if (Input.GetKeyDown(KeyCode.K))
        {
        	SpawnCustomer();
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

    void OnCabbage()
    {
        Instantiate(Resources.Load("Cabbage"), transform.position + randomOffset(), Random.rotation);
    }

    void OnPumpkin()
    {
        Instantiate(Resources.Load("Pumpkin"), transform.position + randomOffset(), Random.rotation);
    }

    void OnCorn()
    {
        Instantiate(Resources.Load("Banana"), transform.position + randomOffset(), Random.rotation);
    }

    void OnCarrot()
    {
        Instantiate(Resources.Load("Banana"), transform.position + randomOffset(), Random.rotation);
    }

    void OnPear()
    {
        Instantiate(Resources.Load("Banana"), transform.position + randomOffset(), Random.rotation);
    }



    Vector3 randomOffset(){

		float num = 0.02f;

		return new Vector3(Random.Range(-1 * num, num),Random.Range(-1 * num, num),Random.Range(-1 * num, num ));
	}

	public void SpawnCustomer()
	{
		CanvasController.Instance.ClearText();

		if (currentCustomer != null)
			Destroy(currentCustomer.gameObject);

		var randNum = UnityEngine.Random.Range(1,7);
		GameObject customerPrefab = Resources.Load("CustomerPrefabs/" + randNum) as GameObject;

		float randomRot = UnityEngine.Random.Range(170,240);

		GameObject c = Instantiate(customerPrefab, new Vector3(0.7f, -0.7f, 3.5f), Quaternion.Euler(new Vector3(0,randomRot,0)));

		c.transform.localScale = c.transform.localScale * 0.4f;
		currentCustomer = c.GetComponent<Customer>();
	}

	private void InitInstance()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy (gameObject);
	}
}
