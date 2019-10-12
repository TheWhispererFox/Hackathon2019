using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public GameObject PanelExit;
    private void Update()
    {
        if (PanelExit == false || Input.GetKeyDown(KeyCode.Escape))
        {
            PanelExit.SetActive(true);
        }
       
        
    }
    public void OnClickPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void NoExit()
    {
        PanelExit.SetActive(false);
    }
    public void Exit_menu()
    {
        Application.Quit();
    }
    public void OnClickExit()
    {
        PanelExit.SetActive(true);
    }


    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
