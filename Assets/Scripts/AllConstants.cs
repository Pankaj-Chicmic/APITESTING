public class AllConstants 
{
    public static string url = "https://00b4-112-196-113-2.ngrok-free.app";
    public static string authorizationHeaderVariableName = "Authorization";
    ////// GUEST LOGIN
    public static string guestLoginRoute = "/guest-login";
    public static string guestLoginDuplicateUserNameMessage = "duplicated key not allowed";
    public static string playerPrefUserNameVariableName= "UserName";
    public static string playerPrefEmailVariableName = "Email";
    public static string playerPrefAccessTokenVariableName = "AccessToken";
    //public static string
    ///// EMAIL UPDATE
    public static string emailUpdateRoute = "/update-email";
    public static string emailDuplicateMessage = "Email is already attached to another player";
    ////// Login
    public static string loginRoute = "/login";
    public static string loginRecordNotFoundMessage="record not found";
    //// Equip Car
    public static string equipCarRoute = "/equip-car";
    //// Buy Car
    public static string buyCarRoute = "/buy-car";
    //// Sell Car
    public static string sellCarRoute = "/sell-car";
}
