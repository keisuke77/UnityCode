using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectchange : MonoBehaviour
{
    // Start is called before the first frame update
     public int active=0;
	
	
	[SerializeField]
    public GameObject[] objlist;
	public GameObject nowobj;
	
void Start()
{
	objhide();
}

public void change(){
objchanger();
active++;

}

public void changeint(int a){
	active=a;
	objchanger();
	
}
public GameObject Getobj(){

return objlist[active];
}
public void objhide(){

	for (var i = 0; i < objlist.Length; i++)
			{
			objlist[i].SetActive(false);
	
			}
			nowobj=null;
}
void objchanger(){

	objhide();


objlist[active].SetActive(true);

nowobj=objlist[active];
weapon();
}

public virtual void weapon(){}
	// Update is called once per frame
	void Update () {
		if (active>=objlist.Length)
		{
			active=0;
		}
		//　1キーを押したらカメラの切り替えをする
		
	}
}
 