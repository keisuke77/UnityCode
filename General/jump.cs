using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Jump : MonoBehaviour,IMove {
    public float jumpSpeed = 10f; // Adjust as needed
    public float maxJumpSpeed;
    [Header("max時の点滅")]
    public FlickerModel flickerModel;
    public float gravity = 9.81f; // Adjust as needed
      public float moveSpeed = 5f; // Adjust as needed
    Camera cam;
    private NavMeshAgent agent;
    private Vector3 velocity;
    [HideInInspector]
    public bool isJumping;
   
   public bool MaxAutoJump;
  public GameObject Dust;
public GroundCast groundCast;
    public float speed { get; set; }
    public bool Stop { get; set; }
  Rigidbody rb;
  Animator anim;

public float EndAnimDistanse=0.5f;
public float DefaultJumpSpeed=3;
  public Image image;

public Vector3 ChargeScale;
Vector3 defaultScale;
  public float JumpCoolDownTime=0.5f;
    void Start() {  anim= GetComponent<Animator>();// Assuming the ground check position is at the character's feet
 defaultScale=transform.localScale;
               cam = Camera.main;
             rb=GetComponent<Rigidbody>();
       agent = GetComponent<NavMeshAgent>();
        }
Vector3 LastPos;
public float NeedMaxJumpSeconds=1;
public float jumpOverDelayTime=0.1f;

Tween tween,tween2;
private void Update() {
   if (Stop)return;

    if (groundCast.Custom(EndAnimDistanse) && velocity.y < 0) {
         JumpEndAnim();
                   }
                   if (groundCast.isGrounded && velocity.y < 0)
                   {
                      JumpEnd();
   
                   }
    

        if (keiinput.Instance.jump && groundCast.isGrounded && !isJumping&&!Charge) {
           JumpPrepair();
tween=transform.DOScale(ChargeScale,NeedMaxJumpSeconds);

tween2=transform.DOMove((ChargeScale-defaultScale)/2,NeedMaxJumpSeconds).SetRelative(true);
        }
         if ((keiinput.Instance.jumpup||(MaxAutoJump&&jumpSpeed==maxJumpSpeed))&&Charge && groundCast.isGrounded && !isJumping) {
            tween.Kill(true);   tween2.Kill(true); 
              JumpNow();
transform.DOScale(defaultScale,0.2f);
          
          
        }

}
     private void FixedUpdate() {
      if (Stop)return;

   if (image!=null)
{    float lerpValue =1- Mathf.InverseLerp(DefaultJumpSpeed, maxJumpSpeed, jumpSpeed);
  
  image.fillAmount=lerpValue;
}

  if (Charge)
  {
          transform.position=LastPos;
    if (jumpSpeed<maxJumpSpeed)
    {
        jumpSpeed+=Time.deltaTime*(maxJumpSpeed-DefaultJumpSpeed)/NeedMaxJumpSeconds;

    }else if(jumpSpeed>maxJumpSpeed)
    {
     jumpSpeed=maxJumpSpeed;
  flickerModel?.damagecolor();
      
    }
  
  }else
  {
    jumpSpeed=DefaultJumpSpeed;
  }
        if (isJumping) {
                // Handle gravity
            velocity.y -= gravity * Time.deltaTime;

            // Handle horizontal movement
          
       Vector3 move =  transform.CameraDirection(cam,keiinput.Instance.directionkey);
          Debug.Log(move);            

          transform.position += (velocity+(move * moveSpeed)) * Time.deltaTime;   
      rb.useGravity=false;
       }else
       {
                     rb.useGravity=true;
       }

          LastPos=transform.position;
    }
bool Charge;
public void JumpEndAnim(){  

  anim.SetBool("JumpEndAnim",true);
    
 }
public void JumpEnd(){        

  anim.SetBool("JumpStart",false);
     anim.SetBool("Jump",false);
       anim.SetBool("JumpEnd",true);
     
     Instantiate(Dust,groundCast.hit.point,Quaternion.identity);
        Stop=true;
         keikei.delaycall(()=>Stop=false,JumpCoolDownTime);
          
   velocity.y = 0f;
            isJumping = false;
            if(agent)
            agent.enabled = true;
     
   
}
    public void JumpPrepair() {
      anim.SetBool("JumpStart",true);
       anim.SetBool("JumpEnd",false);
               anim.SetBool("JumpEndAnim",false);
  
      Charge=true;
    } public void JumpNow() {
     
  
       anim.SetBool("Jump",true);
        anim.SetBool("JumpEnd",false);
       Charge=false;
        isJumping = true;
        velocity.y = jumpSpeed;
         if(agent) agent.enabled = false;
    }
}