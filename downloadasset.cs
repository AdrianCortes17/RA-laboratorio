using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Vuforia;
using UnityEngine.Networking;

public class downloadasset : MonoBehaviour
{
    public TMPro.TextMeshProUGUI texto;
    public static string numerador;
    public static int checador;

    public void assetnumber(string num){
        if (num == "99"){
            numerador = null;
        }else{
        numerador = num;
        }
    }

    public void assetchecador(int num){
        checador = num;
    }

    // Start is called before the first frame update
    public void Update()
    {
         if(numerador != null && checador != 1)
        {
        StartCoroutine(downloadassetbundle(numerador));  
        }
    }

    private IEnumerator downloadassetbundle(string num)
    {
        GameObject go = null;
        AssetBundle bundle = null;
        if (num == numerador){
            string url = "http://jueputaadrian.vastserve.com/assets/" + num;
            Debug.Log(url);
            using(UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url)){
            yield return www.SendWebRequest();
            if( www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("no");
            }
            else
            {
                bundle = DownloadHandlerAssetBundle.GetContent(www);
                go = bundle.LoadAsset(bundle.GetAllAssetNames()[0]) as GameObject;
                bundle.Unload(false);
            }
            www.Dispose();
        }
        InstantiateGameObject(go);

        }
        
    }

    private void InstantiateGameObject(GameObject go)
    {
        if(go != null)
        {
            GameObject instanceGo = Instantiate(go);
            switch(numerador){
                case "1": instanceGo.transform.position = new Vector3(0,0,500);
                break;
                case "2": instanceGo.transform.position = new Vector3(0,0,500);
                break;
                case "3": instanceGo.transform.position = new Vector3(0,0,50);
                break;
                default: instanceGo.transform.position = new Vector3(0,0,150);
                break;
            }
            texto.text = "funciona";
            instanceGo.tag = "clone";
            checador = 1;
        }
        else 
        {
            Debug.Log("no bundle");
        }
    }
}
