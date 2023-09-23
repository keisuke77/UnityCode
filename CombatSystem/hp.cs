using System.Collections;
using System.Collections.Generic;
using DG.Tweening;


public class hp : hpcore
{
    public flashscrean flashscrean;
    bool once;
  
       // Start is called before the first frame update



    public override void SetUp()
    {
     
   }

 
    public override void OnDamage(int damage)
    {
     
     
        if (flashscrean != null)
        {
            flashscrean?.damage();
        }
    }

    public override void OnDeath()
    {
        if (once) return;
        once = true;
        keikei.delaycall(() => gametransition(), 3f);
    }

 
    public void gametransition()
    {
        Fade.LastScreenFade.FadeIn(1,()=>{SceneManage.Instance.ReloadScene();  Fade.LastScreenFade.FadeOut(1);});
        deatheffect.End();
       
    }
}
