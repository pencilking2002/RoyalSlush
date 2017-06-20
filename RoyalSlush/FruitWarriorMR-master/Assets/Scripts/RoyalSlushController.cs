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
    }

    void OnStart()
    {
        SceneManager.LoadScene("Main");
    }


    void OnWatermelon()
    {
        Instantiate(Resources.Load("Watermelon"), transform.position, Quaternion.identity);
    }

    void OnGrapes()
    {
        Instantiate(Resources.Load("Grapes"), transform.position, Quaternion.identity);
    }
}
