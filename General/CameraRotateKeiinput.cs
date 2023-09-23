using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateKeiinput : MonoBehaviour
{
    public controll CameraControllX;
    
    public controll CameraControllY;
    public CameraManager cameraManager;
    [Range(-30,30)]
   public float SensiviltyX=8;
 [Range(-30,30)]
    public float SensiviltyY=8;


 [Range(0,180)]    public float YclampMax;  [Range(0,-180)]  public float YclampMin;

    // Update is called once per frame
   

    // Update is called once per frame
    void Update()
    {
    cameraManager.Param.angles.y+=SensiviltyX*keiinput.Instance.GetAxis(CameraControllX);
    cameraManager.Param.angles.x+=SensiviltyY*keiinput.Instance.GetAxis(CameraControllY);
    cameraManager.Param.angles.x=Mathf.Clamp(cameraManager.Param.angles.x,YclampMin,YclampMax);
    }
}
