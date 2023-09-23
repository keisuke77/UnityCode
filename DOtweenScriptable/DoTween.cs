using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;


public class DoTween : MonoBehaviour
{
   public bool enablePlay;
   public DoTweenSeri DoTweenSeri;
   public Sequence seq;
   public KeyCode PlayButtton;
 [Button("Play", "Play")]
    public int a;
    public void Play()
    {
      seq= DoTweenSeri.Play(transform);
    }  
   private void Awake() {
    if (enablePlay&&!seq.IsActive())
   {
      Play();
      
   }
   }
private void OnEnable() {
   if (enablePlay&&!seq.IsActive())
   {
      Play();
      
   }
}
public void Pause(){
   
 seq.TogglePause();
}
public void ReStart(){
    // トゥイーンの再開
    seq.Play();
}
private void Update() {
   if (PlayButtton.keydown())
   {
      Play();
   }
}
public void Repead(int num){
     seq.timeScale = num;  
}
}
