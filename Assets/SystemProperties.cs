using UnityEngine;
using System.Collections;

public class SystemProperties
{
    private static SystemProperties mInstance = new SystemProperties();
    private AndroidJavaClass jc;

    private SystemProperties()
    {
#if UNITY_EDITOR
#else
        jc = new AndroidJavaClass("android.os.SystemProperties");
#endif
    }

    private string getProperty(string key, string def)
    {
#if UNITY_EDITOR
        return def;
#else
        return jc.CallStatic<string> ("get", key, def);
#endif
    }
    private void setProperty(string key,string val)
    {
#if UNITY_EDITOR
        return;
#else
        jc.CallStatic("set", key, val);
#endif
    }
    public static string get(string key, string def)
    {
        return mInstance.getProperty(key, def);
    }
    public static void set(string key,string value)
    {
        mInstance.setProperty(key, value);
    }
}

