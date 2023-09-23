using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace Shogi
{
    public enum Team
    {
        Red,
        Blue,
        None
    }

    [System.Serializable]
    public class KomaInfo
    {  
        public Vector2 Adress;
        public Koma Koma;
        public Team Team;
        public bool King;
        public bool isNari;

        [HideInInspector]
        public GameObject obj;
        Renderer render;

        public KomaInfo(
            Koma Komas,
            Vector2 Adresss,
            Team Teams,
            bool Kings = false
        )
        {
            King = Kings;
            Koma = Komas;
            Team = Teams;
            Adress = Adresss;
           
        }

        public KomaInfo Clone()
        {
            return MemberwiseClone() as KomaInfo;
        }

        public GameObject Create(GameObject KomaModel)
        {
            obj = keikei.Instantiate(
                KomaModel,
                new Vector3(Adress.x, 0, Adress.y),
                Quaternion.identity
            );
            if (Team == Team.Red)
            {
                obj.transform.rotation=Quaternion.Euler(0, 180, 0);
        
            }
            if (Team == Team.None)
            {
                ChangeColor(new Color(0,0,0,0));
            }
            render = obj.GetComponent<Renderer>();
            render.material.mainTexture = Koma.NormalIcon;
            return obj;
        }

        public void Nari()
        {
            isNari = true;
            render.material.mainTexture = Koma.NariIcon;
        }

        public void ChangeAdress(Vector2 Adresss)
        {
            Adress = Adresss;
            obj.transform.position=new Vector3(Adress.x, 0, Adress.y);
        }
        
        public void EnableAlphaClipping(Material mat)
{
    // 1. シェーダーをURP Litに変更する
    mat.shader = Shader.Find("Universal Render Pipeline/Lit");

    // 2. アルファクリッピングを有効にする
    mat.EnableKeyword("_ALPHATEST_ON");

    // その他の設定やパラメータの変更が必要な場合、ここに追加します
}

        public void ChangeColor(Color col)
        {
            if (render == null)
            {
                render = obj.GetComponent<Renderer>();
            }
               // アルファクリッピングを有効にする
    EnableAlphaClipping(render.material);


            render.material.color = col;
        }
    }

    [CreateAssetMenu(fileName = "board", menuName = "Shogi/Board", order = 0)]
    public class BoardData : ScriptableObject
    {
        public List<KomaInfo> KomaInfos;
        public int BoardSize;
    }
}
