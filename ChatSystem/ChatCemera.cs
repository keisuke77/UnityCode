using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;


[System.Serializable]
public class ChatCamera {
      public ChatData ChatData;
  public CameraManager.Parameter Parameter;
 public Camera CutCamera;
 public UnityEvent unityEvent;

 public void Execute(System.Action ac=null){
  
   if (CutCamera)
   {
    CutCamera.enabled=true;
   }
         ChatExecute.instance.Execute(
            ChatData,
            () =>
            {  
                   if (ac!=null)
             {
                ac();
                if (unityEvent!=null)
                {
                     unityEvent.Invoke();
                }
               
             }
            });  if ( CameraManager.instance!=null)
    {
         CameraManager.instance?.UpdatePram(Parameter);
    }
 }
}
[System.Serializable]
public class ChatCameras {
    public List<ChatCamera> chatCameras;
    public int interval;

   public System.Action EndCall=null;
 public void Execute(int num=0){
if (num>=chatCameras.Count)
{if (EndCall!=null) EndCall();
   return;
}
foreach (var item in chatCameras)
{if (item.CutCamera)
{
       item.CutCamera.enabled=false;

}
 }
 chatCameras[num].Execute(()=>keikei.delaycall(()=>this.Execute(num+1),Time.deltaTime*interval));  
 }
}

public class ChatCemera : MonoBehaviour {
    
public ChatCameras ChatCameras;
private void Start() {
    keikei.delaycall(()=>  ChatCameras.Execute(0),1);
   
}
     
}