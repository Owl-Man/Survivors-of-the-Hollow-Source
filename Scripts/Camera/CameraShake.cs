using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float duration, magnitude, noize;

    public void StrongShake()
    {
        StopAllCoroutines();
        StartCoroutine(StrongShaking());
    }
    
    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(Shaking());
    }
    
    private IEnumerator StrongShaking()
    {
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(Shaking());
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    private IEnumerator Shaking()
    {
        //Инициализируем счётчиков прошедшего времени
        float elapsed = 0f;
        //Сохраняем стартовую локальную позицию
        Vector3 startPosition = transform.localPosition;
        //Генерируем две точки на "текстуре" шума Перлина
        Vector2 noizeStartPoint0 = Random.insideUnitCircle * noize;
        Vector2 noizeStartPoint1 = Random.insideUnitCircle * noize;

        //Выполняем код до тех пор пока не иссякнет время
        while (elapsed < duration)
        {
            //Генерируем две очередные координаты на текстуре Перлина в зависимости от прошедшего времени
            Vector2 currentNoizePoint0 = Vector2.Lerp(noizeStartPoint0, Vector2.zero, elapsed / duration);
            Vector2 currentNoizePoint1 = Vector2.Lerp(noizeStartPoint1, Vector2.zero, elapsed / duration);
            //Создаём новую дельту для камеры и умножаем её на длину дабы учесть желаемый разброс
            Vector2 cameraPostionDelta = new Vector2(Mathf.PerlinNoise(currentNoizePoint0.x, currentNoizePoint0.y),
                Mathf.PerlinNoise(currentNoizePoint1.x, currentNoizePoint1.y));
            cameraPostionDelta *= magnitude;

            //Перемещаем камеру в нувую координату
            transform.localPosition = startPosition + (Vector3)cameraPostionDelta;

            //Увеличиваем счётчик прошедшего времени
            elapsed += Time.deltaTime;
            //Приостанавливаем выполнение корутины, в следующем кадре она продолжит выполнение с данной точки
            yield return null;
        }

        //По завершении вибрации, возвращаем камеру в исходную позицию
        transform.localPosition = startPosition;
    }
}