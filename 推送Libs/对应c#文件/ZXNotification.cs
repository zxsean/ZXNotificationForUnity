//#define UNITY_IPHONE
//#define UNITY_ANDROID

using System;

/// <summary>
/// 推送
/// </summary>
internal class ZXNotification
{
    private const long __SECONDOFDAY = 24 * __SECONDOFHOUR;

    private const long __SECONDOFHOUR = 60 * __SECONDOFMINUTE;

    private const long __SECONDOFMINUTE = 60;

    internal delegate void Handler();

    /// <summary>
    /// 程序暂停时候调用
    /// </summary>
    internal Handler m_handerPause;

    /// <summary>
    /// 程序恢复时候调用
    /// </summary>
    internal Handler m_handerResume;

    /// <summary>
    /// 程序退出调用
    /// </summary>
    internal Handler m_handerQuit;

    static private ZXNotification m_instance = null;

    static internal ZXNotification GetInstance()
    {
        if (m_instance == null)
        {
            m_instance = new ZXNotification();

            // 初始化
            m_instance.Init();
        }

        return m_instance;
    }

    /// <summary>
    /// 本地推送
    /// </summary>
    /// <param name="_title">推送标题</param>
    /// <param name="_message">推送文本</param>
    /// <param name="_hour">几点推送24小时制</param>
    /// <param name="_isRepeatDay">是否重复</param>
    internal void NotificationMessage(int _id, string _title, string _message,
                                      int _hour, bool _isRepeatDay)
    {
        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
        System.DateTime newDate = new System.DateTime(year, month, day, _hour, 0, 0);

        NotificationMessage(_id, _title, _message, newDate, _isRepeatDay);
    }

    /// <summary>
    /// 本地推送 你可以传入一个固定的推送时间
    /// </summary>
    /// <param name="_message"></param>
    /// <param name="_date"></param>
    /// <param name="_isRepeatDay"></param>
    internal void NotificationMessage(int _id, string _title, string _message,
                                      DateTime _date, bool _isRepeatDay)
    {
        // 推送时间需要大于当前时间
        if (_date <= System.DateTime.Now)
        {
            // 往前翻一天
            _date = _date.AddDays(1);
        }

#if UNITY_IPHONE || NEW_EGSDK_IOS
        // 推送时间需要大于当前时间
        // if (_date > System.DateTime.Now)
        {
            UnityEngine.LocalNotification localNotification = new UnityEngine.LocalNotification();
            localNotification.fireDate = _date;
            localNotification.alertBody = _message;
            localNotification.applicationIconBadgeNumber = 1;
            localNotification.hasAction = true;
            localNotification.alertAction = _title;

            if (_isRepeatDay)
            {
                // 是否每天定期循环
                localNotification.repeatCalendar = CalendarIdentifier.ChineseCalendar;
                localNotification.repeatInterval = CalendarUnit.Day;
            }

            localNotification.soundName = UnityEngine.LocalNotification.defaultSoundName;
            NotificationServices.ScheduleLocalNotification(localNotification);
        }
#endif

#if UNITY_ANDROID || NEW_EGSDK_ANDROID
        // 前面已经前翻一天
        // if (_date > System.DateTime.Now)
        {
            long _delay;

            //if (_date.Hour >= 12)
            //{
            //    _delay = __SECONDOFDAY - ((_date.Hour - 12) * __SECONDOFHOUR + _date.Minute * __SECONDOFMINUTE + _date.Second);
            //}
            //else
            //{
            //    _delay = (12 - _date.Hour) * __SECONDOFHOUR - _date.Minute * __SECONDOFMINUTE - _date.Second;
            //}

            // 计算时间差 目标时间-当前时间获取时间差
            TimeSpan _ts = (TimeSpan)(_date - System.DateTime.Now);

            _delay = (long)_ts.TotalSeconds;

            DebugLog.Log("dateTime:" + _date.ToString());

            DebugLog.Log("delay:" + _delay);

            if (_isRepeatDay)
            {
                AndroidLocalNotification.SendRepeatingNotification(_id, _delay, __SECONDOFDAY, _title,
                                                                   _message,
                                                                   new Color32(0xff, 0x44, 0x44, 255),
                                                                   true, true, true, "app_icon");
            }
            else
            {
                AndroidLocalNotification.SendNotification(_id, _delay,
                                                          _title,
                                                          _message,
                                                          new Color32(0xff, 0x44, 0x44, 255),
                                                          true, true, true, "app_icon");
            }
        }
#endif
    }

    private void Init()
    {
#if UNITY_IPHONE

        NotificationServices.RegisterForRemoteNotificationTypes(RemoteNotificationType.Alert |
                                                                RemoteNotificationType.Badge |
                                                                RemoteNotificationType.Sound);
#endif

        //第一次进入游戏的时候清空，有可能用户自己把游戏冲后台杀死，这里强制清空
        CleanNotification();
    }

    /// <summary>
    /// 注册消息
    /// 记得找个MonoBehavior挂上去
    /// </summary>
    /// <param name="paused"></param>
    internal void OnApplicationPause(bool paused)
    {
        DebugLog.Log("OnApplicationPause");

        //程序进入后台时
        if (paused)
        {
            //// 10秒后发送
            //NotificationMessage(1, "这是测试后台推送", "这是notificationtest的推送正文信息", System.DateTime.Now.AddSeconds(10), false);

            //// 每天中午12点推送
            //NotificationMessage(2, "这是循环推送", "每天中午12点推送", 12, true);

            //// 这里注册
            //m_handerPause += () =>
            //{
            //    NotificationMessage(2, "这是循环推送", "每天中午12点推送", 12, true);
            //};

            // 执行
            if (m_handerPause != null)
            {
                m_handerPause();
            }
        }
        else
        {
            ////程序从后台进入前台时
            //CleanNotification();

            if (m_handerResume != null)
            {
                m_handerResume();
            }
        }
    }

    /// <summary>
    /// 退出时候调用
    /// 记得找个MonoBehavior挂上去
    /// </summary>
    internal void OnApplicationQuit()
    {
        DebugLog.Log("OnApplicationQuit");

        if (m_handerQuit != null)
        {
            m_handerQuit();
        }
    }

    /// <summary>
    /// 清空所有本地消息
    /// </summary>
    internal void CleanNotification()
    {
#if UNITY_IPHONE
        UnityEngine.LocalNotification l = new UnityEngine.LocalNotification();
        l.applicationIconBadgeNumber = -1;
        NotificationServices.PresentLocalNotificationNow(l);
        NotificationServices.CancelAllLocalNotifications();
        NotificationServices.ClearLocalNotifications();
#endif

#if UNITY_ANDROID
        // AndroidLocalNotification.CancelNotification(1);
        // AndroidLocalNotification.CancelNotification(2);
        AndroidLocalNotification.CancelAllNotifications();
#endif
    }
}