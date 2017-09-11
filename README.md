# ZXNotificationForAndroid
安卓推送2unity

根据:https://github.com/Agasper/unity-android-notifications修改

支持进程被杀掉以后后台推送,如果后台没有推送就是被一些安全软件拦截了.

点击推送如果没有拉起Active请检查一下AndroidLocalNotification中的__MAINACTIVITYCLASSNAME是否是你的游戏主Active名字

##然后记得在AndroidManifest中添加
<!--UnityActivity-->
<meta-data 
android:name="unityplayer.UnityActivity" 
android:value="true" /> 