using System;
using UnityEngine;

internal class AndroidLocalNotification
{
    /// <summary>
    /// Inexact uses `set` method
    /// Exact uses `setExact` method
    /// ExactAndAllowWhileIdle uses `setAndAllowWhileIdle` method
    /// Documentation: https://developer.android.com/intl/ru/reference/android/app/AlarmManager.html
    /// </summary>
    public enum NotificationExecuteMode
    {
        Inexact = 0,
        Exact = 1,
        ExactAndAllowWhileIdle = 2
    }

#if UNITY_ANDROID && !UNITY_EDITOR

    private const string __FULLCLASSNAME = "net.agasper.unitynotification.UnityNotificationManager";

    //private const string __MAINACTIVITYCLASSNAME = "com.unity3d.player.UnityPlayerNativeActivity";
    //private const string __MAINACTIVITYCLASSNAME = @"com.unity3d.player.UnityPlayer";
    //private const string __MAINACTIVITYCLASSNAME = "com.entermate.ahgy.UnityPlayerNativeActivity";

    

#endif
    /// <summary>
    /// 必须改成sdk主Active
    /// </summary>
    static private string __MAINACTIVITYCLASSNAME = null;
    //private const string __SMALLICON = "notify_icon_small";
    /// <summary>
    /// 推送显示的小图标
    /// </summary>
    private const string __SMALLICON = "icon_silhouette";

    /// <summary>
    /// 推送下拉出来显示的大图标
    /// </summary>
    private const string __BIGICON = "ic_launcher";

    /// <summary>
    /// 单次推送
    /// </summary>
    /// <param name="id">每一个不同的推送需要不同的id,如果相同就会出现几个推送文本什么都一样的错误</param>
    /// <param name="delay">推送延迟</param>
    /// <param name="title">标题</param>
    /// <param name="message">文本</param>
    public static void SendNotification(int id, TimeSpan delay, string title, string message)
    {
        SendNotification(id, (int)delay.TotalSeconds, title, message, Color.white);
    }

    /// <summary>
    /// 设置推送拉起来的Active
    /// </summary>
    /// <param name="_str"></param>
    public static void SetPushActiveName(string _str)
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        __MAINACTIVITYCLASSNAME = _str;

        //__MAINACTIVITYCLASSNAME = "com.unity3d.player.UnityPlayerNativeActivity"; 

#endif
    }

    /// <summary>
    /// 单次推送
    /// </summary>
    /// <param name="id">每一个不同的推送需要不同的id,如果相同就会出现几个推送文本什么都一样的错误</param>
    /// <param name="delay">推送延迟</param>
    /// <param name="title">标题</param>
    /// <param name="message">文本</param>
    /// <param name="bgColor">颜色</param>
    /// <param name="sound">声音</param>
    /// <param name="vibrate">震动</param>
    /// <param name="lights"></param>
    /// <param name="bigIcon">大图标</param>
    /// <param name="executeMode"></param>
    public static void SendNotification(int id, long delay, string title, string message, Color32 bgColor,
                                        bool sound = true, bool vibrate = true, bool lights = true,
                                        string bigIcon = __BIGICON,
                                        NotificationExecuteMode executeMode = NotificationExecuteMode.Inexact)
    {
        DebugLog.Log("Title:" + title);

#if UNITY_ANDROID && !UNITY_EDITOR

        if (__MAINACTIVITYCLASSNAME == null)
        {
            DebugLog.LogError("ActiveName Error");

            return;
        }

        AndroidJavaClass pluginClass = new AndroidJavaClass(__FULLCLASSNAME);

        if (pluginClass != null)
        {
            pluginClass.CallStatic("SetNotification", id, delay * 1000L, title, message, message, sound ? 1 : 0,
                                   vibrate ? 1 : 0, lights ? 1 : 0, bigIcon,
                                   __SMALLICON, bgColor.r * 65536 + bgColor.g * 256 + bgColor.b,
                                   (int)executeMode, __MAINACTIVITYCLASSNAME);
        }

#endif
    }

    /// <summary>
    /// 重复推送
    /// </summary>
    /// <param name="id">每一个不同的推送需要不同的id,如果相同就会出现几个推送文本什么都一样的错误</param>
    /// <param name="delay">推送延迟</param>
    /// <param name="timeout">下次推送开始时间</param>
    /// <param name="title">标题</param>
    /// <param name="message">文本</param>
    /// <param name="bgColor"></param>
    /// <param name="sound"></param>
    /// <param name="vibrate"></param>
    /// <param name="lights"></param>
    /// <param name="bigIcon">大图标.默认用的程序图标</param>
    public static void SendRepeatingNotification(int id, long delay, long timeout, string title,
                                                 string message, Color32 bgColor, bool sound = true,
                                                 bool vibrate = true, bool lights = true, string bigIcon = __BIGICON)
    {
        DebugLog.Log("SendRepeatingNotification");

#if UNITY_ANDROID && !UNITY_EDITOR
                
        if (__MAINACTIVITYCLASSNAME == null)
        {
            DebugLog.LogError("ActiveName Error");

            return;
        }

        AndroidJavaClass pluginClass = new AndroidJavaClass(__FULLCLASSNAME);

        if (pluginClass != null)
        {
            pluginClass.CallStatic("SetRepeatingNotification", id,
                                   delay * 1000L, title, message, message,
                                   timeout * 1000, sound ? 1 : 0, vibrate ? 1 : 0, lights ? 1 : 0,
                                   bigIcon, __SMALLICON, bgColor.r * 65536 + bgColor.g * 256 + bgColor.b,
                                   __MAINACTIVITYCLASSNAME);
        }

#endif
    }

    /// <summary>
    /// 清楚指定id推送
    /// </summary>
    /// <param name="id"></param>
    public static void CancelNotification(int id)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(__FULLCLASSNAME);

        if (pluginClass != null)
        {
            pluginClass.CallStatic("CancelNotification", id);
        }
#endif
    }

    public static void CancelAllNotifications()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(__FULLCLASSNAME);

        if (pluginClass != null)
        {
            pluginClass.CallStatic("CancelAll");
        }
#endif
    }
}