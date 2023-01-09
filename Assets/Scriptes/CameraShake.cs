using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform camTransform;
    private float shakeDur = 1f, shakeAmount=0.04f, decreaseFactor=1.5f; //время, которое камера будет трястись(радиус сферы);сила, с которой камера будет трястись; насколько быстро будет останавливаться тряска
    private Vector3 originPosition;
    private void Start()
    {
        camTransform = GetComponent<Transform>();//присваиваем значение компонента Transform
        originPosition = camTransform.localPosition;//localPosition - поскольку камера - дочерний элемент
    //присваиваем объекту начальное положение камеры
    }
    private void Update()
    {
        if (shakeDur > 0)//пока камера трясётся
        {//мы в каждый момент времени выбираем случайную позицию для камеры
            camTransform.localPosition = originPosition + Random.insideUnitSphere*shakeAmount; //выбираем случайную позицию (0.04 - радиус сферы, из которой выбирается поизиция)
            shakeDur -= Time.deltaTime * decreaseFactor;
        }
        else //если время тряски истекло
        {
            shakeDur = 0;
            camTransform.localPosition = originPosition;//возвращаем камеру на исходную позицию
        }
    }
}
