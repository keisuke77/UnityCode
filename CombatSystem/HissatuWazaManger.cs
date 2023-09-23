using UnityEngine;using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HissatuWaza {
    public string Name;
    public Image icon;
   public float mpUseAmount;
    public mp mp;
  
  public DelayEvent delayEvent;
}
public class HissatuWazaManger : SelectBehabior<HissatuWaza> {
    
    public Animator anim;
   
public override void ChangeCallBack(){

     foreach (var item in Elements)
     {if (item.icon)
     {
        item.icon.gameObject.SetActive(false);
     
     }
        
     }
     CurrentElement.icon.gameObject.SetActive(true);
      
}

public override void DecideEvent(){
   
    if (CurrentElement.mp.mpuse(CurrentElement.mpUseAmount))
    {
          anim.Play(CurrentElement.Name);
          CurrentElement.delayEvent.Execute();
    }
    
}
   




}