//#define UNITY_ANDROID

using System;
using UnityEngine;

/// <summary>
/// 推送测试
/// </summary>
public class NotificationTest : MonoBehaviour
{
#if UNITY_ANDROID

    private float sleepUntil = 0;

    private void OnGUI()
    {
        // Color is supported only in Android >= 5.0
        GUI.enabled = sleepUntil < Time.time;

        if (GUILayout.Button("5 SECONDS", GUILayout.Height(Screen.height * 0.2f)))
        {
            //AndroidLocalNotification.SendNotification(1, 5, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
            DateTime _dt = DateTime.Now;
            _dt = _dt.AddSeconds(5);
            ZXNotification.GetInstance().NotificationMessage(3, "测试标题", "测试中文", _dt, false);

            sleepUntil = Time.time + 5;
        }

        if (GUILayout.Button("5 SECONDS BIG ICON", GUILayout.Height(Screen.height * 0.2f)))
        {
            AndroidLocalNotification.SendNotification(2, 5, "Title", "Long message text with big icon", new Color32(0xff, 0x44, 0x44, 255), true, true, true, "app_icon");
            sleepUntil = Time.time + 5;
        }

        if (GUILayout.Button("EVERY 5 SECONDS", GUILayout.Height(Screen.height * 0.2f)))
        {
            AndroidLocalNotification.SendRepeatingNotification(1, 5, 5, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
            sleepUntil = Time.time + 99999;
        }

        if (GUILayout.Button("10 SECONDS EXACT", GUILayout.Height(Screen.height * 0.2f)))
        {
            AndroidLocalNotification.SendNotification(3, 10, "Title", "Long exact message text", new Color32(0xff, 0x44, 0x44, 255),
                                                      executeMode: AndroidLocalNotification.NotificationExecuteMode.ExactAndAllowWhileIdle);
            sleepUntil = Time.time + 10;
        }

        GUI.enabled = true;

        if (GUILayout.Button("STOP", GUILayout.Height(Screen.height * 0.2f)))
        {
            AndroidLocalNotification.CancelNotification(1);

            AndroidLocalNotification.CancelAllNotifications();

            sleepUntil = 0;
        }
    }

#endif
}