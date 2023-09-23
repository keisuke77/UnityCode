using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

[System.Serializable]
public class DelayEvent
{
    public float DelayTime;
    public UnityEvent events;
public bool OnceCall;
bool once;
    public void Execute(){
        if (OnceCall)
        {
            if (!once)
            {once=true;
                keikei.delaycall(()=>events.Invoke(),DelayTime);
    
            }
                 }else
        {
             keikei.delaycall(()=>events.Invoke(),DelayTime);
  
        }

           }
  
}

[System.Serializable]
public class DelayEvents
{public List<DelayEvent> delayEvents;
 public void Execute(){
       delayEvents.ForEach(x=>x.Execute());
    }

}
public class talk : MonoBehaviour
{
    public Button button;
     public UnityEvent kaiwaendevents;
     public IconGenerate IconGenerate;
    GameObject Talker;
public ChatCameras ChatCameras;

public Transform LookTalkBefore;

    [Button( "TalkEvent", "実行")]
public int a;
public Quaternion DefaultRotation,DefaultRotationLookAtBefore;
       public bool isTalking;
    // Start is called before the first frame update
    void Start()
    {DefaultRotation=transform.rotation;
    if (LookTalkBefore!=null) DefaultRotationLookAtBefore=LookTalkBefore.rotation;
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
        isTalking=true;
         temp=Talker;
     
var BeforeParam=CameraManager.instance.CloneParam;
        ChatCameras.Execute(0);
          
        ChatCameras.EndCall=()=>{
        if (kaiwaendevents!=null)
        {  kaiwaendevents.Invoke();
            
        }
      
     CameraManager.instance.TweenPram(BeforeParam);
       isTalking=false;
                temp.Restart();
                temp.root().GetComponent<AnimBoolSet>().Stop = false; 
               transform.DORotate(DefaultRotation.eulerAngles, 1);
      
        };  Exit(false);
    
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
    public bool BeforeLookAt;
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
            if (BeforeLookAt&&Vector3.Angle(transform.forward,Talker.transform.position-transform.position)<100)
            {       Debug.Log(Vector3.Angle(transform.forward,Talker.transform.position-transform.position));
                LookTalkBefore?.DOLookAt(Talker.transform.position, 1, AxisConstraint.Y);
            }
               }else
               {
                if (!isTalking&&BeforeLookAt)LookTalkBefore?.DORotate(DefaultRotationLookAtBefore.eulerAngles, 1);
    
               }
    }
}
