using System;
using UnityEngine;

public class AdvancedPlayerPrefs
{
    public enum Key
    {
        FirstBoot,
        LoginAccount,
        Sound,
        PushNotification

        //..
        //Add more Keys
    }

    private const string KeyPrefix = "#";

    public static void SetBool(Key key, bool value)
    {
        PlayerPrefs.SetInt(GetKeyName(key), Convert.ToInt32(value));
    }

    public static bool GetBool(Key key, bool defaultValue = false)
    {
        return Convert.ToBoolean(PlayerPrefs.GetInt(GetKeyName(key), Convert.ToInt32(defaultValue)));
    }

    public static void SetInt(Key key, int value)
    {
        PlayerPrefs.SetInt(GetKeyName(key), value);
    }

    public static int GetInt(Key key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(GetKeyName(key), defaultValue);
    }

    public static void SetFloat(Key key, float value)
    {
        PlayerPrefs.SetFloat(GetKeyName(key), value);
    }

    public static float GetFloat(Key key, float defaultValue = 0f)
    {
        return PlayerPrefs.GetFloat(GetKeyName(key), defaultValue);
    }

    public static void SetString(Key key, string value)
    {
        PlayerPrefs.SetString(GetKeyName(key), value);
    }

    public static string GetString(Key key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(GetKeyName(key), defaultValue);
    }

    public static void Delete(Key key)
    {
        PlayerPrefs.DeleteKey(GetKeyName(key));
    }

    public static void DeleteDefinedKeys()
    {
        foreach (Key key in Enum.GetValues(typeof(Key)))
        {
            PlayerPrefs.DeleteKey(GetKeyName(key));
        }

        PlayerPrefs.Save();
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    private static string GetKeyName(Key key)
    {
        return KeyPrefix + key.ToString().ToLower();
    }
}