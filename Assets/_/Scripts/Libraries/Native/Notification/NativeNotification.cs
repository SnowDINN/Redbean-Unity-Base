﻿using System;
using Cysharp.Threading.Tasks;

#if UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;
#endif

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif

namespace Redbean.Native
{
	public class NativeNotification
	{
		private const string PERMISSION_ANDROID = "android.permission.POST_NOTIFICATIONS";
		private const string CHANNEL_ID = "REDBEAN_BoongGOD_PUSH_CHANNEL";
		
		public static async UniTask Setup()
		{
#if UNITY_ANDROID
			if (!Permission.HasUserAuthorizedPermission(PERMISSION_ANDROID))
				Permission.RequestUserPermission(PERMISSION_ANDROID);
			
			AndroidNotificationCenter.RegisterNotificationChannel(new AndroidNotificationChannel
			{
				Id = CHANNEL_ID,
				Name = CHANNEL_ID,
				Importance = Importance.Default,
				Description = "Generic notifications",
			});
#endif
			
#if UNITY_IOS
			using var request = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge, true);
			await UniTask.WaitUntil(() => request.IsFinished);
#endif

			await UniTask.CompletedTask;
		}
		
		public static void PushNotification(NotificationForm form)
		{
#if UNITY_ANDROID
			var notification = new AndroidNotification
			{
				Title = form.Title,
				Text = form.Body,
				FireTime = form.SendTime,
				ShouldAutoCancel = true
			};
			
			if (AndroidNotificationCenter.CheckScheduledNotificationStatus(form.Id).Equals(NotificationStatus.Scheduled))
				AndroidNotificationCenter.UpdateScheduledNotification(form.Id, notification, CHANNEL_ID);	
			else
				AndroidNotificationCenter.SendNotificationWithExplicitID(notification, CHANNEL_ID, form.Id);	
#endif
		
#if UNITY_IOS
			iOSNotificationCenter.ScheduleNotification(new iOSNotification($"{form.Id}")
			{
				Title = form.Title,
				Subtitle = form.SubTitle,
				Body = form.Body,
				CategoryIdentifier = CHANNEL_ID,
				ThreadIdentifier = "t1",
				Trigger = new iOSNotificationCalendarTrigger
				{ 
					Year = form.SendTime.Year,
					Month = form.SendTime.Month,
					Day = form.SendTime.Day,
					Hour = form.SendTime.Hour,
					Minute = form.SendTime.Minute,
					Second = form.SendTime.Second,
					Repeats = false,
					UtcTime = true,
				}
			});
#endif
		}
	}

	public class NotificationForm
	{
		public DateTime SendTime;
		public int Id;
		public string Title;
		public string SubTitle;
		public string Body;
	}
}