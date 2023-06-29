using System;
using UnityEngine;
public class APICalls
{
    public static void GuestLogin(string playerName,Action<string> onSuccess=null, Action<string> onFailure=null,Action onConnectionError=null)
    {
        PlayerForGuestLogin playerForGuestLogin = new PlayerForGuestLogin(playerName,SystemInfo.deviceUniqueIdentifier,1);
        string jsonData = JsonUtility.ToJson(playerForGuestLogin);
        byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);
        Requests.Post(AllConstants.guestLoginRoute , bite , onSuccess : onSuccess,onFailure : onFailure,onConnectionError : onConnectionError);
    }
    public static void Login(string email, Action<string> onSuccess = null, Action<string> onFailure = null, Action onConnectionError = null)
    {
        EmailIDForEmailUpdateAndLogin emailIDForEmailUpdateAndLogin = new EmailIDForEmailUpdateAndLogin(email); 
        string jsonData = JsonUtility.ToJson(emailIDForEmailUpdateAndLogin);
        byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);
        Requests.Post(AllConstants.loginRoute, bite, onSuccess: onSuccess, onFailure: onFailure, onConnectionError: onConnectionError);
    }
    public static void EmailUpdate(string emailID,string accessToken,Action<string> onSuccess=null, Action<string> onFailure=null, Action onConnectionError=null)
    {
        EmailIDForEmailUpdateAndLogin emailIDForEmailUpdate = new EmailIDForEmailUpdateAndLogin(emailID);
        string jsonData = JsonUtility.ToJson(emailIDForEmailUpdate);
        byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);
        Requests.PUT(AllConstants.emailUpdateRoute,bite,accessToken, onSuccess : onSuccess,onFailure : onFailure,onConnectionError : onConnectionError);
    }
    public static void EquipCar(string cardId,string acessToken, Action<string> onSuccess, Action<string> onFailure = null, Action onConnectionError = null)
    {
        CarIdForEquiCarAndBuyCar carIdForEquiCar = new CarIdForEquiCarAndBuyCar(cardId);
        string jsonData = JsonUtility.ToJson(carIdForEquiCar);
        byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);
        Requests.PUT(AllConstants.equipCarRoute, bite, acessToken, onSuccess: onSuccess, onFailure: onFailure, onConnectionError: onConnectionError);
    }
    public static void BuyCar(string carId,string acessToken ,Action<string> onSuccess = null, Action<string> onFailure = null, Action onConnectionError = null)
    {
        CarIdForEquiCarAndBuyCar carIdForEquiCarAndBuyCar = new CarIdForEquiCarAndBuyCar(carId);
        string jsonData = JsonUtility.ToJson(carIdForEquiCarAndBuyCar);
        byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);
        Requests.Post(AllConstants.buyCarRoute, bite,acessToken, onSuccess: onSuccess, onFailure: onFailure, onConnectionError: onConnectionError);
    }
    public static void SellCar(string cardId,string acessToken,Action<string> onSuccess = null, Action<string> onFailure = null, Action onConnectionError = null)
    {
        CarIdForEquiCarAndBuyCar carIdForEquiCarAndBuyCar = new CarIdForEquiCarAndBuyCar(cardId);
        string jsonData = JsonUtility.ToJson(carIdForEquiCarAndBuyCar);
        byte[] bite = System.Text.Encoding.UTF8.GetBytes(jsonData);
        Requests.Delete(AllConstants.sellCarRoute, bite, acessToken, onSuccess: onSuccess, onFailure: onFailure, onConnectionError: onConnectionError);
    }
}
public class PlayerForGuestLogin
{
    public string playerName;
    public string deviceId;
    public int os;
    public PlayerForGuestLogin(string playerName,string deviceId,int os)
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