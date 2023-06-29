using System;
using UnityEngine;
using UnityEngine.Networking;

public class Requests : MonoBehaviour
{
    public static void Post(string route,byte[] bite,string acessToken="",Action<string> onSuccess=null, Action<string> onFailure=null, Action onConnectionError=null)
    {
        UnityWebRequest request = new UnityWebRequest(AllConstants.url+route, UnityWebRequest.kHttpVerbPOST)
        {
            uploadHandler = new UploadHandlerRaw(bite),
            downloadHandler = new DownloadHandlerBuffer(),
        };
        if (acessToken != "")
        {
            request.SetRequestHeader(AllConstants.authorizationHeaderVariableName, acessToken);
        }
        UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
        asyncOperation.completed += operation =>
        {
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                onConnectionError?.Invoke();
            }
            else if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                onFailure?.Invoke(request.downloadHandler.text);
            }
            request.Dispose();
        };
    }
    public static void PUT(string route, byte[] bite, string acessToken = "", Action<string> onSuccess = null, Action<string> onFailure = null, Action onConnectionError = null)
    {
        UnityWebRequest request = new UnityWebRequest(AllConstants.url + route, UnityWebRequest.kHttpVerbPUT)
        {
            uploadHandler = new UploadHandlerRaw(bite),
            downloadHandler = new DownloadHandlerBuffer(),
        };
        if (acessToken != "")
        {
            request.SetRequestHeader(AllConstants.authorizationHeaderVariableName, acessToken);
        }
        UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
        asyncOperation.completed += operation =>
        {
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                onConnectionError?.Invoke();
            }
            else if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                onFailure?.Invoke(request.downloadHandler.text);
            }
            request.Dispose();
        };
    }
    public static void Delete(string route, byte[] bite, string acessToken = "", Action<string> onSuccess = null, Action<string> onFailure = null, Action onConnectionError = null)
    {
        UnityWebRequest request = new UnityWebRequest(AllConstants.url + route, UnityWebRequest.kHttpVerbDELETE)
        {
            uploadHandler = new UploadHandlerRaw(bite),
            downloadHandler = new DownloadHandlerBuffer(),
        };
        if (acessToken != "")
        {
            request.SetRequestHeader(AllConstants.authorizationHeaderVariableName, acessToken);
        }
        UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
        asyncOperation.completed += operation =>
        {
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                onConnectionError?.Invoke();
            }
            else if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                onFailure?.Invoke(request.downloadHandler.text);
            }
            request.Dispose();
        };
    }
}
