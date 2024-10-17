using UnityEngine;
using Unity.Notifications.Android;


namespace AndroidIntegrationTools
{
    public class Notification : MonoBehaviour
    {
        private bool _ispaused;

        private int _timeForNotification;

        private void Start() => CreateNotificationChannel();

        private void OnApplicationPause (bool pauseStatus) 
        {
            _ispaused = pauseStatus;

            if (_ispaused) 
            {
                TrySendNotification();
            }
        }

        private void OnApplicationQuit () => TrySendNotification();

        public void TrySendNotification()
        {
            int isSending = Random.Range(1, 7); //Шанс отправки уведомления

            if (isSending == 1) 
            {
                SendNotification();
            }
        }

        public void CreateNotificationChannel() 
        {
            var channel = new AndroidNotificationChannel()
            {
                Id = "channel_id",
                Name = "Survivors of the Hollow",
                Importance = Importance.High,
                Description = "Generic notifications",
            };

            AndroidNotificationCenter.RegisterNotificationChannel(channel);
        }

        public void SendNotification() 
        {
            var notification = new AndroidNotification();
            notification.Title = "Hey survivor!";
            notification.Text = "Gotta get ready for the next wave";
            notification.LargeIcon = "icon_0";
            notification.SmallIcon = "icon_1";
            _timeForNotification = Random.Range(550, 2600);
            notification.FireTime = System.DateTime.Now.AddMinutes(_timeForNotification);

            AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }
    }
}