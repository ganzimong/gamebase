using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceSystem
{
    public static string LanguageCode()
    {
        return null;
    }

    public static string CountryCode()
    {
        return null;
    }

    public static string UniqueDeviceId()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }

    public static string DeviceModel()
    {
        return SystemInfo.deviceModel;
    }

    public static string OSVersion()
    {
        return SystemInfo.operatingSystem;
    }

    public static string Platform()
    {
#if UNITY_EDITOR
        return "EDITOR";
#elif UNITY_ANDROID
        return "ANDROID";
#elif UNITY_IOS
        return "iOS";
#else
        return null;
#endif
    }
}
