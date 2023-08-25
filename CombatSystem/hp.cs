using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;
using DG.Tweening;

using ItemSystem;


public class hp : hpcore
{
    public flashscrean flashscrean;
    bool once;
    public itemdrop itemdrop;
    datamanage datamanage;
       // Start is called before the first frame update



    public override void SetUp()
    {
        datamanage = GetComponent<datamanage>();
   }

    public void hpitemheal()
    {
        hpheal(itemuse.instance.Itemkind.GetPower());
        itemuse.instance.itemused();
    }

    public override void OnDamage(int damage)
    {
        if (gameObject.pclass() != null&&killer!=null)
        {
            gameObject
                .pclass()
                .PlayerMoveAction(() => anim.gameObject.transform.LookAt(killer.transform));
        }
     
        if (flashscrean != null)
        {
            flashscrean?.damage();
        }
    }

    public override void OnDeath()
    {
        if (once)
            return;
        once = true;
   

        if (datamanage != null)
        {
            datamanage.HPreset();
        }
   
  
        keikei.delaycall(() => gametransition(), 3f);
    }

 
    public void gametransition()
    {
        Fade.LastScreenFade.FadeIn(1,()=>{SceneManage.Instance.ReloadScene();  Fade.LastScreenFade.FadeOut(1);});
        deatheffect.End();
       
        gameObject.pclass().gametransition("loss");
    }
}
