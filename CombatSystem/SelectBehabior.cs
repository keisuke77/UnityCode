using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SelectBehabior<T> : MonoBehaviour {
        public controll DecideKey;
    public controll AddKey;
    public controll DownKey;

public List<T> Elements;
public T CurrentElement;

public int active=1000000;
int tempactive=-1;
public virtual void DecideEvent(){}

public void CurrentElementChange(T element)
{
CurrentElement=Elements[Elements.IndexOf(element)];
}

public virtual void ChangeCallBack(){}
 void Update() {
    if (Elements!=null)
    {
   CurrentElement=Elements[active%Elements.Count];
    }
    if (active != tempactive)
        {
            tempactive = active;
            ChangeCallBack();
        }
        
    
    if (keiinput.Instance.GetKey(DecideKey))
    {
      DecideEvent();
    }
  if (keiinput.Instance.GetKey(AddKey))
    {
      active++;
    }
  if (keiinput.Instance.GetKey(DownKey))
    {
      active--;
         }

   

}

    
}