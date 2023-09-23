using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class character
{  
    public ChatCharactor ChatCharactor{set{ChatCharactorSet(value);_ChatCharactor=value;
 }get{return _ChatCharactor;}
    }

    ChatCharactor _ChatCharactor;
   [HideInInspector] public string layerName;
  [HideInInspector] public string name;
  [HideInInspector] public int power = 3;
  [HideInInspector] public Sprite Icon;
  [HideInInspector] public int hp;
  [HideInInspector] public int maxhp;
   [Header("説明")]
  [HideInInspector] public string Explain;
  [HideInInspector] public int defence;
    public GameObject bone;
    public GameObject mesh;
    public Avatar avatar;
    public GameObject righthand,lefthand,rightfoot,leftfoot,weapon;
    public transformdata weaponstransform;
   
    public float HPRate(){
       return (float) hp/maxhp;
    }

    public void ChatCharactorSet(ChatCharactor ChatCharactor){
     layerName=ChatCharactor.name; 
     name=ChatCharactor.name; 
power=ChatCharactor.Power;
Icon=ChatCharactor.icon;
hp=ChatCharactor.CurrentHP;
maxhp=ChatCharactor.MaxHP;
Explain=ChatCharactor.Explain;
defence=ChatCharactor.Defence;

    }  
    public void Set(charactorchange charactorchange)
    {  
        foreach (var Elementss in charactorchange.Elements)
        {
            Elementss.bone.SetActive(false);
            Elementss.mesh.SetActive(false);
        }
     
        bone.SetActive(true);
        mesh.SetActive(true);
    
        charactorchange.anim.avatar = avatar;
        charactorchange.anim.enabled=true;
      //アバター変更の後に変更する必要がある
       for (int i = 0; i < charactorchange.Elements.Count; i++)
        {
               charactorchange.anim.SetLayerWeight(i,0);
        }
        charactorchange.anim.SetLayerWeight(charactorchange.anim.GetLayerIndex(layerName),1);
 if (charactorchange.weapons!=null)
        {
              charactorchange.weapons.transform.parent = righthand.transform;
        }
        //武器の装着
        if (weaponstransform != null && charactorchange.weapons != null)
        {
            transformenter(charactorchange.weapons.transform, weaponstransform);
        }
        
    }
public void transformenter(Transform trans,transformdata transformdata){

trans.localPosition=transformdata.pos;
trans.localRotation=transformdata.rotation;

}
}

public interface GetBodyPart
{
    GameObject GetRightHand();
    GameObject GetLeftHand();
    GameObject GetLeftFoot();
    GameObject GetRightFoot();  GameObject GetWeapon();
}

public class charactorchange : SelectBehabior<character> ,GetBodyPart
{
    public Dropdown DropDown;
    public GameObject weapons;
    public Animator anim;
    [Button("Reload","リロード")]
    public int q;
    public Text text;
    string temptext;
    int hairetucheck;
    public List<string> m_DropOptions;
    public float CharactorApeareDelay;
    public GameObject ChangeSpawn;
    public DoTweenSeri DoTweenSeri;
    public GameObject GetRightHand()=> CurrentElement.righthand;
    public GameObject GetLeftHand()=>CurrentElement.lefthand;
    public GameObject GetLeftFoot()=> CurrentElement.leftfoot;
    public GameObject GetRightFoot()=>CurrentElement.rightfoot;
    public GameObject GetWeapon()=>CurrentElement.weapon;
    void OnDisable()
    {
        this.enabled = true;
    }

    void Awake()
    {
        gameObject.Stop();
    }
 void OnEnable() {
    Reload();
}
public void Reload(){
    if (CurrentElement==null)
    {
       keikei.delaycall(Reload,1);
        return;
    }
     CurrentElement.Set(this);
}
    void Start()
    {
   
		  if (GetComponent<hpcore>())
        { 
        hpcore= GetComponent<hpcore>();
        }
        
        //DropDownの要素にリストを追加
        foreach (character item in Elements)
        {   
               m_DropOptions.Add(item.name);
               item.mesh.SetActive(false);
        }
        
        if (DropDown)
        {
            DropDown.ClearOptions();
            DropDown.AddOptions(m_DropOptions);
        }
    
    }



    public void characterhide()
    {
        foreach (var Elementss in Elements)
        {
            Elementss.mesh.SetActive(false);
        }
    } 
    Sequence ChangeTween;
    
    public override void ChangeCallBack()
    { 
         if (ChangeSpawn!=null)
        {
            Instantiate(ChangeSpawn,transform.position,Quaternion.identity);
        } 
        gameObject.Stop();
        if (ChangeTween!=null)
        {
                     ChangeTween.Complete();
  
        }
         
       ChangeTween= DoTweenSeri.Play(transform);
        keikei.delaycall(()=>{
        gameObject.Restart();
        CurrentElement.Set(this);
 
        if (hpcore!=null)
        {
        hpcore.HP=CurrentElement.ChatCharactor.CurrentHP;
        hpcore.maxHP=CurrentElement.ChatCharactor.MaxHP;
        hpcore.defence=CurrentElement.ChatCharactor.Defence;
        }
        if (GetComponent<AnimSpeedChangeState>()!=null)
        {
            GetComponent<AnimSpeedChangeState>().runSpeed=CurrentElement.ChatCharactor.RunSpeed;
            GetComponent<AnimSpeedChangeState>().idleSpeed=CurrentElement.ChatCharactor.IdleSpeed;
            GetComponent<AnimSpeedChangeState>().walkSpeed=CurrentElement.ChatCharactor.WalkSpeed;
        }
        if (GetComponent<attackcore>()!=null)
        { 
          GetComponent<attackcore>().basedamagevalue=CurrentElement.ChatCharactor.Power;
          GetComponent<attackcore>().baseforcepower=CurrentElement.ChatCharactor.knockBack;    
        }  if (GetComponent<Jump>()!=null)
        { GetComponent<Jump>().DefaultJumpSpeed=CurrentElement.ChatCharactor.JumpPower;
        }

        },CharactorApeareDelay);
 //チェンジ中に周りの動きを止める
   
     try
{
      var navchaises=GameObjectExtension.GetComponentsInActiveScene<navchaise>(false);
         if (navchaises!=null)
         {
              
        foreach (var item in  navchaises)
        {
            
        item.Stop=true;
        keikei.delaycall(()=>item.Stop=false,1.5f);
        }
         }
}
catch (System.Exception)
{
    
    throw;
}
    }

hpcore hpcore;
   // Update is called once per frame
    void LateUpdate()
    {      
         if(text != null)
        {
            text.text = CurrentElement.name;
        }
       
        if (hpcore!=null&&CurrentElement!=null)
        {    
          CurrentElement.hp=hpcore.HP;
          CurrentElement.maxhp=hpcore.maxHP;
        }
        if (DropDown != null)
        {
            if (temptext != DropDown.captionText.text)
            {
                hairetucheck = 0;
                temptext = DropDown.captionText.text;

                foreach (var item in Elements)
                {
                    hairetucheck++;
                    if (item.name == DropDown.captionText.text)
                    {
                        active = hairetucheck;
                    }
                }
            }
        }
    }
}
