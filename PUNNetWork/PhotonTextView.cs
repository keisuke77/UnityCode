using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonTextView : MonoBehaviour, IPunObservable
{
    private PhotonView photonView;

    public string Text;

 
    void Awake()
    {
        this.photonView = GetComponent<PhotonView>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // オーナーの場合
        if (stream.IsWriting)
        {
            stream.SendNext(this.Text);
        }
        // オーナー以外の場合
        else
        {
            this.Text = (string)stream.ReceiveNext();
        }
    }


}
