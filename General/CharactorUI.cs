using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharactorUI : MonoBehaviour
{
  
public static CharactorUI MainCharactor;
    public charactorResist charactorResist;
    public Text nameText;
    public Text ExplainText;
    public Image IconImage;
    public Image Hpvar;
    public Text HpText;


public bool isMainCharactor;

void MainLoop(){
  nameText.text="◄"+charactorResist.character.name;
        IconImage.sprite=charactorResist.character.Icon;
          Hpvar.fillAmount=charactorResist.character.HPRate();
   

    if (HpText!=null)
      {
            HpText.text=(charactorResist.character.HPRate()*100).ToString("F0")+"%";
   
      } 
       if (ExplainText!=null)
      {
        ExplainText.text=charactorResist.character.Explain;
     
      }
        

   
}
public void MainSet(){
    var temp=MainCharactor.charactorResist;
 

MainCharactor.charactorResist=charactorResist; 
if (MainCharactor!=null)
    {
        
charactorResist=temp;
    }
MainCharactor.charactorResist.Set();  
MainLoop();
}
private void Awake() {

  if (isMainCharactor
)
{
        MainCharactor=this;
      keikei.delaycall(MainSet,0.3f); 
}
}
  
     void OnEnable(){
      MainLoop();
    }


}
