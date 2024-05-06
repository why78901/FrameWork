public class AOTFairyStatics
{
#if UNITY_STANDALONE_WIN
    public static string addressRoot = "aa_win/";
#elif UNITY_ANDROID
    public static string addressRoot = "aa_android/";
#elif UNITY_IPHONE
    public static string addressRoot = "aa_ios/";
#endif
}