using System;
using UnityEngine;
using UnityEngine.Networking;

public class Requests : MonoBehaviour
{
    // Send a POST request
    public static void Post(string route, byte[] bite, string accessToken = "", Action<string> onSuccess = null, Action<string> onFailure = null, Action onConnectionError = null)
    {
        // Create a new UnityWebRequest with the specified route and HTTP method
        UnityWebRequest request = new UnityWebRequest(AllConstants.url + route, UnityWebRequest.kHttpVerbPOST)
        {
            uploadHandler = new UploadHandlerRaw(bite), // Set the upload handler with the given data
            downloadHandler = new DownloadHandlerBuffer(), // Set the download handler to store the response data
        };

        // Set the access token header if provided
        if (accessToken != "")
        {
            request.SetRequestHeader(AllConstants.authorizationHeaderVariableName, accessToken);
        }

        request.timeout = 8; // Set the request timeout

        // Send the request asynchronously and handle the completion
        UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
        asyncOperation.completed += operation =>
        {
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                // Handle connection error
                onConnectionError?.Invoke();
            }
            else if (request.result == UnityWebRequest.Result.Success)
            {
                // Handle successful response
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                // Handle failure response
                onFailure?.Invoke(request.downloadHandler.text);
            }

            request.Dispose(); // Dispose the request object
        };
    }

    // Send a PUT request
    public static void PUT(string route, byte[] bite, string accessToken = "", Action<string> onSuccess = null, Action<string> onFailure = null, Action onConnectionError = null)
    {
        // Create a new UnityWebRequest with the specified route and HTTP method
        UnityWebRequest request = new UnityWebRequest(AllConstants.url + route, UnityWebRequest.kHttpVerbPUT)
        {
            uploadHandler = new UploadHandlerRaw(bite), // Set the upload handler with the given data
            downloadHandler = new DownloadHandlerBuffer(), // Set the download handler to store the response data
        };

        // Set the access token header if provided
        if (accessToken != "")
        {
            request.SetRequestHeader(AllConstants.authorizationHeaderVariableName, accessToken);
        }

        request.timeout = 8; // Set the request timeout

        // Send the request asynchronously and handle the completion
        UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
        asyncOperation.completed += operation =>
        {
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                // Handle connection error
                onConnectionError?.Invoke();
            }
            else if (request.result == UnityWebRequest.Result.Success)
            {
                // Handle successful response
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                // Handle failure response
                onFailure?.Invoke(request.downloadHandler.text);
            }

            request.Dispose(); // Dispose the request object
        };
    }

    // Send a DELETE request
    public static void Delete(string route, byte[] bite, string accessToken = "", Action<string> onSuccess = null, Action<string> onFailure = null, Action onConnectionError = null)
    {
        // Create a new UnityWebRequest with the specified route and HTTP method
        UnityWebRequest request = new UnityWebRequest(AllConstants.url + route, UnityWebRequest.kHttpVerbDELETE)
        {
            uploadHandler = new UploadHandlerRaw(bite), // Set the upload handler with the given data
            downloadHandler = new DownloadHandlerBuffer(), // Set the download handler to store the response data
        };

        // Set the access token header if provided
        if (accessToken != "")
        {
            request.SetRequestHeader(AllConstants.authorizationHeaderVariableName, accessToken);
        }

        request.timeout = 8; // Set the request timeout

        // Send the request asynchronously and handle the completion
        UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
        asyncOperation.completed += operation =>
        {
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                // Handle connection error
                onConnectionError?.Invoke();
            }
            else if (request.result == UnityWebRequest.Result.Success)
            {
                // Handle successful response
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                // Handle failure response
                onFailure?.Invoke(request.downloadHandler.text);
            }

            request.Dispose(); // Dispose the request object
        };
    }
}
