using System;
using System.Collections;
using System.Text;
using OVRSimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class APIWrapper : MonoBehaviour
{
    
    private const string URL = "https://on-request-example-hc4jyd4vva-uc.a.run.app/transactions/evaluate";
    private string jsonBody = "";

    public bool requestComplete = false;
    public string geePeeTee = "";
    public int actualValue = 0;
    
    [System.Serializable]
    public class Response {
        public string reply;
        public string spent_amount;
    }
    
    public void RequestAnswer(int guess, string category)
    {
        jsonBody = "{\"user_id\":\"user_1\",\"expected_amount\":" + guess +
                          ",\"filter\":{\"tx_type\":\""+ category +"\"}}";
        
        print(jsonBody);

        StartCoroutine(nameof(MakeAPIRequest));
    }
    
    private IEnumerator MakeAPIRequest()
    {
        using UnityWebRequest webRequest = new UnityWebRequest(URL, "POST");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        byte[] rawTextData = Encoding.UTF8.GetBytes(jsonBody);

        webRequest.uploadHandler = new UploadHandlerRaw(rawTextData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.Success:
                var res = webRequest.downloadHandler.text;
                Response jsonRes = JsonUtility.FromJson<Response>(res);
                print(res);
                print(jsonRes.reply);
                // TODO: something with the result
                geePeeTee = jsonRes.reply;
                actualValue = Int32.Parse(jsonRes.spent_amount);
                requestComplete = true;
                break;
            case UnityWebRequest.Result.InProgress:
                break;
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.ProtocolError:
            case UnityWebRequest.Result.DataProcessingError:
                print(webRequest.error);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

