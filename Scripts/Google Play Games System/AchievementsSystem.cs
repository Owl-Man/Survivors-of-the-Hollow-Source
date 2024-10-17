using UnityEngine;

namespace Google_Play_Games_System
{
    public class AchievementsSystem : MonoBehaviour
    {
        public static AchievementsSystem Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        public void Unlock(string achievementID)
        {
            if (!PlayGamesManager.Instance.isAuth) return;
            
            Social.ReportProgress(achievementID, 100.00f, (bool success) =>
            {
                if (success) Debug.Log("achievement unlocked");
                else Debug.Log("fail achievement unlocking");
            });
        }

        public void ShowAchievementProgressUI()
        {
            Social.ShowAchievementsUI();
        }
    }
}