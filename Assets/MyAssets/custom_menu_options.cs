using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class custom_menu_options : MonoBehaviour
{
    //var sceneCamera:Camera = SceneView.lastActiveSceneView.camera;
    public GameObject day_assets;
    public GameObject night_assets;
    //public Camera mainCam;
    // public GameObject night_cam;
    //Camera m_MainCamera;
    public GameObject playerPos;

    public GameObject day_btn;
    public GameObject night_btn;
    public GameObject entrance_area_btn;
    public GameObject rest_area_btn;
    public GameObject back_area_btn;
    public GameObject main_area_btn;

    public GameObject entrance_area_mkr;
    public GameObject rest_area_mkr;
    public GameObject back_area_mkr;
    public GameObject main_area_mkr;


    public void click_day()
    {
       // Camera.allCameras.clearFlags.Skybox;
        //m_MainCamera.clearFlags.Skybox;
        day_assets.SetActive(true);
       // day_cam.SetActive(true);
        night_assets.SetActive(false);
        //night_cam.SetActive(false);
    }

    public void click_night()
    {
        //Camera.allCameras.clearFlags.Skybox;
        day_assets.SetActive(false);
        //day_cam.SetActive(false);
        night_assets.SetActive(true);
       // night_cam.SetActive(true);
    }

    public void click_main_area()
    {
        playerPos.transform.position = main_area_mkr.transform.position;
    }

    public void click_back_area()
    {
        playerPos.transform.position = back_area_mkr.transform.position;
    }

    public void click_rest_area()
    {
        playerPos.transform.position = rest_area_mkr.transform.position;
    }

    public void click_entrance_area()
    {
        playerPos.transform.position = entrance_area_mkr.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
