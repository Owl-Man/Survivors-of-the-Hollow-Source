using System.Collections;
using TMPro;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    [SerializeField] private TMP_Text goldCountText, raspberylCountText, sapphireCountText;
    [SerializeField] private GameObject gold, raspberyl, sapphire;
    public int GoldCount { get; private set; }
    public int RaspberylCount { get; private set; }
    public int SapphireCount { get; private set; }

    public static ResourcesManager Instance;

    private void Awake() => Instance = this;

    public void PreviewGoldChanging(int preview)
    {
        goldCountText.text += $" -<color=red>{preview}</color>";
        StartCoroutine(ScaleAnimationOf(gold));
    }

    public void PreviewRaspberylChanging(int preview)
    {
        raspberylCountText.text += $" -<color=red>{preview}</color>";
        StartCoroutine(ScaleAnimationOf(raspberyl));
    }

    public void PreviewSapphireChanging(int preview)
    {
        sapphireCountText.text += $" -<color=red>{preview}</color>";
        StartCoroutine(ScaleAnimationOf(sapphire));
    }

    public void ChangeGoldValue(int count)
    {
        GoldCount += count;
        UpdateValues();
        StartCoroutine(ScaleAnimationOf(gold));
    }

    public void ChangeRaspberylValue(int count)
    {
        RaspberylCount += count;
        UpdateValues();
        StartCoroutine(ScaleAnimationOf(raspberyl));
    }

    public void ChangeSapphireValue(int count)
    {
        SapphireCount += count;
        UpdateValues();
        StartCoroutine(ScaleAnimationOf(sapphire));
    }

    private IEnumerator ScaleAnimationOf(GameObject animObj)
    {
        if (animObj.transform.localScale.x != 1f) yield break;
        
        while (animObj.transform.localScale.x < 1.5f)
        {
            animObj.transform.localScale = new Vector2(animObj.transform.localScale.x + 0.02f,
                animObj.transform.localScale.y + 0.02f);
            yield return null;
        }
        
        while (animObj.transform.localScale.x > 1f)
        {
            animObj.transform.localScale = new Vector2(animObj.transform.localScale.x - 0.02f,
                animObj.transform.localScale.y - 0.02f);
            yield return null;
        }
    }

    public void UpdateValues() 
    {
        goldCountText.text = GoldCount.ToString();
        raspberylCountText.text = RaspberylCount.ToString();
        sapphireCountText.text = SapphireCount.ToString();
    }
}
