using System;
using UnityEngine;

public class APICalls
{
    public static void GuestLogin(string playerName, Action<string> onSuccess = null, Action<string> onFailure = null, Action onConnectionError = null)
    {
        // Create a new PlayerForGuestLogin instance
        PlayerForGuestLogin playerForGuestLogin = new PlayerForGuestLogin(playerName, SystemInfo.deviceUniqueIdentifier, 1);

        // Convert the instance to JSON
        string jsonData = JsonUtility.ToJson(playerForGuestLogin);

        // Convert the JSON string to bytes
        byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Make a POST request to the guest login route
        Requests.Post(AllConstants.guestLoginRoute, bite, onSuccess: onSuccess, onFailure: onFailure, onConnectionError: onConnectionError);
    }

    public static void Login(string email, Action<string> onSuccess = null, Action<string> onFailure = null, Action onConnectionError = null)
    {
        // Create a new EmailIDForEmailUpdateAndLogin instance
        EmailIDForEmailUpdateAndLogin emailIDForEmailUpdateAndLogin = new EmailIDForEmailUpdateAndLogin(email);

        // Convert the instance to JSON
        string jsonData = JsonUtility.ToJson(emailIDForEmailUpdateAndLogin);

        // Convert the JSON string to bytes
        byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Make a POST request to the login route
        Requests.Post(AllConstants.loginRoute, bite, onSuccess: onSuccess, onFailure: onFailure, onConnectionError: onConnectionError);
    }

    public static void EmailUpdate(string emailID, string accessToken, Action<string> onSuccess = null, Action<string> onFailure = null, Action onConnectionError = null)
    {
        // Create a new EmailIDForEmailUpdateAndLogin instance
        EmailIDForEmailUpdateAndLogin emailIDForEmailUpdate = new EmailIDForEmailUpdateAndLogin(emailID);

        // Convert the instance to JSON
        string jsonData = JsonUtility.ToJson(emailIDForEmailUpdate);

        // Convert the JSON string to bytes
        byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Make a PUT request to the email update route
        Requests.PUT(AllConstants.emailUpdateRoute, bite, accessToken, onSuccess: onSuccess, onFailure: onFailure, onConnectionError: onConnectionError);
    }

    public static void EquipCar(string cardId, string accessToken, Action<string> onSuccess, Action<string> onFailure = null, Action onConnectionError = null)
    {
        // Create a new CarIdForEquiCarAndBuyCar instance
        CarIdForEquiCarAndBuyCar carIdForEquiCar = new CarIdForEquiCarAndBuyCar(cardId);

        // Convert the instance to JSON
        string jsonData = JsonUtility.ToJson(carIdForEquiCar);

        // Convert the JSON string to bytes
        byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Make a PUT request to the equip car route
        Requests.PUT(AllConstants.equipCarRoute, bite, accessToken, onSuccess: onSuccess, onFailure: onFailure, onConnectionError: onConnectionError);
    }

    public static void BuyCar(string carId, string accessToken, Action<string> onSuccess = null, Action<string> onFailure = null, Action onConnectionError = null)
    {
        // Create a new CarIdForEquiCarAndBuyCar instance
        CarIdForEquiCarAndBuyCar carIdForEquiCarAndBuyCar = new CarIdForEquiCarAndBuyCar(carId);

        // Convert the instance to JSON
        string jsonData = JsonUtility.ToJson(carIdForEquiCarAndBuyCar);

        // Convert the JSON string to bytes
        byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Make a POST request to the buy car route
        Requests.Post(AllConstants.buyCarRoute, bite, accessToken, onSuccess: onSuccess, onFailure: onFailure, onConnectionError: onConnectionError);
    }

    public static void SellCar(string cardId, string accessToken, Action<string> onSuccess = null, Action<string> onFailure = null, Action onConnectionError = null)
    {
        // Create a new CarIdForEquiCarAndBuyCar instance
        CarIdForEquiCarAndBuyCar carIdForEquiCarAndBuyCar = new CarIdForEquiCarAndBuyCar(cardId);

        // Convert the instance to JSON
        string jsonData = JsonUtility.ToJson(carIdForEquiCarAndBuyCar);

        // Convert the JSON string to bytes
        byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Make a DELETE request to the sell car route
        Requests.Delete(AllConstants.sellCarRoute, bite, accessToken, onSuccess: onSuccess, onFailure: onFailure, onConnectionError: onConnectionError);
    }
}

public class PlayerForGuestLogin
{
    public string playerName;
    public string deviceId;
    public int os;

    public PlayerForGuestLogin(string playerName, string deviceId, int os)
    {
        this.playerName = playerName;
        this.deviceId = deviceId;
        this.os = os;
    }
}

public class EmailIDForEmailUpdateAndLogin
{
    public string email;

    public EmailIDForEmailUpdateAndLogin(string email)
    {
        this.email = email;
    }
}

public class CarIdForEquiCarAndBuyCar
{
    public string carId;

    public CarIdForEquiCarAndBuyCar(string carId)
    {
        this.carId = carId;
    }
}
