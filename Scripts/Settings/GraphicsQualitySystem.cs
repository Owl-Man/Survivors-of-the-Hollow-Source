using DataBase;
using UnityEngine;

public class GraphicsQualitySystem : MonoBehaviour
{
    [SerializeField] private GameObject postProcessing, rainPrefab, currentRain;

    public static GraphicsQualitySystem Instance;

    private void Awake() => Instance = this;

    private void Start() => UpdateGraphics();

    public void UpdateGraphics()
    {
        switch (DB.Access.GraphicsQuality)
        {
            case DB.HighGraphicsQuality:
                postProcessing.SetActive(true);
                break;
            case DB.MediumGraphicsQuality:
                postProcessing.SetActive(false);
                break;
            default:
                postProcessing.SetActive(false);
                break;
        }
    }

    public void UpdateRainState()
    {
        DisableRain();
        
        if (DB.Access.IsSPFXOn) EnableRain();
        else DisableRain();
    }

    private void EnableRain()
    {
        if (currentRain != null) Destroy(currentRain);
        
        currentRain = Instantiate(rainPrefab, rainPrefab.transform.position, rainPrefab.transform.rotation);

        currentRain.name = "RainPrefab2D";
    }
    
    private void DisableRain()
    {
        if (currentRain != null) Destroy(currentRain);
    }
}