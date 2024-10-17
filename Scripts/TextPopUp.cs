using System.Collections;
using NTC.Global.Pool;
using TMPro;
using UnityEngine;

public class TextPopUp : MonoBehaviour
{
    [SerializeField] private TMP_Text popUp;

    public static TextPopUp Instance;

    private void Awake() => Instance = this;

    public void CallPopUp(Vector2 position, string text, Color color)
    {
        TMP_Text newPopUp = NightPool.Spawn(popUp.gameObject, position, Quaternion.identity).GetComponent<TMP_Text>();
        
        newPopUp.transform.parent = transform;

        newPopUp.transform.position = new
            Vector2(newPopUp.transform.position.x + Random.Range(-0.5f, 0.5f),
                newPopUp.transform.position.y + Random.Range(-0.5f, 0.5f));
        
        newPopUp.text = text;
        newPopUp.color = color;

        StartCoroutine(PopUpAnimOf(newPopUp));
    }
    
    private IEnumerator PopUpAnimOf(TMP_Text obj)
    {
        obj.alpha = 0;

        obj.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y - 1);

        while (obj.alpha < 1)
        {
            obj.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y + 0.1f);
            obj.alpha += 0.1f;

            if (obj.alpha < 0.4) yield return null;
            else yield return new WaitForSeconds(0.015f);
        }

        yield return new WaitForSeconds(0.2f);

        while (obj.alpha > 0)
        {
            obj.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y + 0.1f);
            obj.alpha -= 0.1f;
            
            if (obj.alpha < 0.4) yield return null;
            else yield return new WaitForSeconds(0.015f);
        }

        obj.alpha = 1;
        
        NightPool.Despawn(obj);
    }
}