using UnityEngine;

public class BGM : MonoBehaviour {

    public string BGMName;
    private void Start() {
        soundManager.Instance.PlayBgmByName(name);
    }

    public void PlaySE(string name){
         soundManager.Instance.PlaySeByName(name);

    }
}