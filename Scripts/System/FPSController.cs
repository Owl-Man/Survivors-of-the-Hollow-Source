using UnityEngine;

namespace System
{
    public class FPSController : MonoBehaviour
    {
        [Range(1, 1000)][SerializeField] private int FPS = 60;
        [SerializeField] private bool isVSyncActive;

        public static FPSController Instance;

        private void Awake()
        {
            Instance = this;
            //FPS = PlayerPrefs.GetInt("FPS", (int)Screen.currentResolution.refreshRateRatio.value);
        }

        private void Start()
        {
            QualitySettings.vSyncCount = !isVSyncActive ? 0 : 1;
            Application.targetFrameRate = FPS;
        }

        public void EnableVSync() => QualitySettings.vSyncCount = 1;

        public void DisableVSync() => QualitySettings.vSyncCount = 0;

        public void SetFPS(int fps)
        {
            if (fps > 0 && fps <= 1000) FPS = fps;
            else throw new ArgumentException();
            
            Application.targetFrameRate = FPS;
            PlayerPrefs.SetInt("FPS", FPS);
        }

        public int GetFPS() => FPS;
    }
}