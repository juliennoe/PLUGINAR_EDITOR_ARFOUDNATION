using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class InteractManager : MonoBehaviour {

    public GameObject _openButton;
    public GameObject _exitButton;

    public GameObject showHidePlane;
    public GameObject showHideCloud;

    public GameObject _showHideAR;
    public bool _swapHideShow;

    void Start ()
    {

        if(FindObjectOfType<EventSystem>().tag == "GameController")
        {
            Debug.Log("Menu");
            ButtonMenu();
        
        }
        else
        {
            Debug.Log("AR");
            ButtonARScene();
        }
   
    }

    public void ButtonMenu()
    {
        _openButton = GameObject.FindWithTag("Player");
        _openButton.GetComponent<Button>().onClick.AddListener(OpenARScene);

        _exitButton = GameObject.FindWithTag("Respawn");
        _exitButton.GetComponent<Button>().onClick.AddListener(QuitApp);

    }

    public void ButtonARScene()
    {
        _swapHideShow = false;
        _showHideAR = GameObject.FindWithTag("Finish");
        _showHideAR.GetComponent<Button>().onClick.AddListener(ShowHide);
        showHidePlane = GameObject.FindWithTag("GameController");
        showHideCloud = GameObject.FindWithTag("GameController");

    }

    public void OpenARScene()
    {
        SceneManager.LoadScene("ARScene");

    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void ShowHide()
    {
        if(_swapHideShow == false)
        {

            SetAllPlanes(false);
            _swapHideShow = true;
        }

        else
        {
            SetAllPlanes(true);
            _swapHideShow = false;
        }


    }

    private void SetAllPlanes(bool value)
    {

        var _planeManager = showHidePlane.GetComponent<ARPlaneManager>();
        var _pointManager = showHideCloud.GetComponent<ARPointCloudManager>();

        foreach(var plane in _planeManager.trackables)
        {
         
            plane.gameObject.SetActive(value);
        }

        foreach(var point in _pointManager.trackables)
        {
            point.gameObject.SetActive(value);
        }
    }
}