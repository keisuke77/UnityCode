using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.Experimental.XR.Interaction;

public class talk : MonoBehaviour
{
    public Button button;
     public UnityEvent kaiwaendevents;
     public IconGenerate IconGenerate;
    GameObject Talker;
public ChatCameras ChatCameras;

    [Button( "TalkEvent", "実行")]
public int a;
public Quaternion DefaultRotation;
       
    // Start is called before the first frame update
    void Start()
    {DefaultRotation=transform.rotation;
      IconGenerate.SetUp(gameObject);
        Exit();
    }


	GameObject temp;

     public void TalkEvent(GameObject obj){
Talker=obj;
TalkEvent();
     }
     
    public void TalkEvent()
    {    
        Talker.transform.DOLookAt(transform.position, 1, AxisConstraint.Y);
         transform.DOLookAt(Talker.transform.position, 1, AxisConstraint.Y);
        
        Talker.Stop();
          Talker.root().GetComponent<AnimBoolSet>().Stop = true; 
        
         temp=Talker;
     ChatCameras.EndCall=()=>{
        if (kaiwaendevents!=null)
        {  kaiwaendevents.Invoke();
            
        }
      
  CameraManager.instance.DefaultPram();
       
                temp.Restart();
                Talker.root().GetComponent<AnimBoolSet>().Stop = false; 
               transform.DORotate(DefaultRotation.eulerAngles, 1);
      
        };

        ChatCameras.Execute(0);
        Exit(false);
    }

    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.proottag())
        {   
            Talker = collisionInfo.gameObject; 
   
              if (button != null)
            {
                button.gameObject.SetActive(true);
                button.onClick.AddListener(() =>
                {
                    TalkEvent();
                    return;
                });
            }
            IconGenerate.On();
        }
    }

    public void Exit(bool OriRotate=true)
    {   if (OriRotate)
    {
          transform.DORotate(DefaultRotation.eulerAngles, 1);
     
    } 
        
        IconGenerate.Off();
        Talker = null;
           if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.proottag())
        {
            Exit();

        }
    }
public controll controll;
    // Update is called once per frame
    void Update()
    {
        if (Talker != null)
        {
            if (keiinput.Instance.GetKey(controll))
            {
                TalkEvent();
            }
          transform.DOLookAt(Talker.transform.position, 1, AxisConstraint.Y);
          }
    }
}
