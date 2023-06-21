using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Vuforia;
using UnityEngine.Networking;

public class SimpleBarcoderScanner : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TextMeshProUGUI Nombre;
    public TMPro.TextMeshProUGUI Descripcion;
    public downloadasset AssetOnline; 
    BarcodeBehaviour mBarcodeBehaviour;
    public static string resultado;
    public static string[] resultadolista;
    public static string resultadoComparador;

    void Start()
    {
        mBarcodeBehaviour = GetComponent<BarcodeBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mBarcodeBehaviour != null && mBarcodeBehaviour.InstanceData != null)
        {
            if(resultado == null){
                StartCoroutine(GetRequest("https://multimedia-upqroo.000webhostapp.com/conectar.php?id=" + mBarcodeBehaviour.InstanceData.Text));
            }
        } else {
            GameObject barcode = GameObject.Find("Barcode(Clone)");
            GameObject[] clones;
 
            clones = GameObject.FindGameObjectsWithTag("clone");

            if (barcode == null && resultado != null){
                Nombre.text = null;
                Descripcion.text = null;
                resultado = null;
            }
            if (barcode == null){
                foreach(GameObject clone in clones)
                {
                    Destroy(clone);
                }
                AssetOnline.assetchecador(99);
            }
        }
    }


    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("no hay conexion");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    
                    resultado = webRequest.downloadHandler.text;

                    resultadolista = resultado.Split("# ");
                    Nombre.text = resultadolista[0];
                    Descripcion.text = resultadolista[1];
                    AssetOnline.assetnumber(resultadolista[2]);
                    break;
            }
        }

    }
}