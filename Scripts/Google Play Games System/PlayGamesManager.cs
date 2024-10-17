using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

namespace Google_Play_Games_System
{
    public class PlayGamesManager : MonoBehaviour
    {
        public static PlayGamesManager Instance;

        public bool isAuth;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        private void Start() => SignIn();

        private void SignIn()
        {
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }

        internal void ProcessAuthentication(SignInStatus status) 
        {
            if (status == SignInStatus.Success)
            {
                isAuth = true;
                // Continue with Play Games Services
            }
            else
            {
                isAuth = false;
                // Disable your integration with Play Games Services or show a login button
                // to ask users to sign-in. Clicking it should call
                // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
            }
        }
    }

    public static class GPGIDs
    {
        public const string extra_fuel_achievement = "CgkI5KL70dIGEAIQAQ";
        public const string superior_material_achievement = "CgkI5KL70dIGEAIQAg";
        public const string efficient_processing_achievement = "CgkI5KL70dIGEAIQAw";
        public const string cumulative_missile_achievement = "CgkI5KL70dIGEAIQBA";
        public const string drill_amplifier_achievement = "CgkI5KL70dIGEAIQBQ";
        public const string artifact_package_achievement = "CgkI5KL70dIGEAIQBg";
    }
}