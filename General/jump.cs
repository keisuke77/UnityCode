using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.AI;using UnityEngine.UI;
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

  public float JumpCoolDownTime=0.5f;
    void Start() {  anim= GetComponent<Animator>();// Assuming the ground check position is at the character's feet
 
               cam = Camera.main;
             rb=GetComponent<Rigidbody>();
       agent = GetComponent<NavMeshAgent>();
        }
Vector3 LastPos;
public float jumpOverDelayTime=0.1f;
     private void FixedUpdate() {
      
   if (image!=null)
{
  image.fillAmount=(float)jumpSpeed-DefaultJumpSpeed/maxJumpSpeed-DefaultJumpSpeed;
}

  if (Charge)
  {
          transform.position=LastPos;
    if (jumpSpeed<maxJumpSpeed)
    {
        jumpSpeed+=Time.deltaTime;

    }else if(jumpSpeed>maxJumpSpeed)
    {
     jumpSpeed=maxJumpSpeed;
  flickerModel?.damagecolor();
      
    }
  
  }else
  {
    jumpSpeed=DefaultJumpSpeed;
  }  if (groundCast.Custom(EndAnimDistanse) && velocity.y < 0) {
         JumpEndAnim();
         keikei.delaycall(JumpEnd,jumpOverDelayTime);
                }
    

        if (keiinput.Instance.jump && groundCast.isGrounded && !isJumping&&!Stop) {
           JumpPrepair();
        }
         if ((keiinput.Instance.jumpup||(MaxAutoJump&&jumpSpeed==maxJumpSpeed))&&Charge && groundCast.isGrounded && !isJumping&&!Stop) {
            JumpNow();
           
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
        agent.enabled = false;
    }
}