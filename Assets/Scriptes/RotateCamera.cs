using UnityEngine;

public class RotateCamera : MonoBehaviour //вращение объекта Rotator, который включаем в себя камеру и свет, поскольку организовать вращение вокруг своей оси проще, нежели вокруг другого предмета (Platform)
{
    public float speed = 5f; //скорость вращения объекта, котрую можно менять в Unity(public)
    private Transform _rotator; //переменная ссылается на компонент Transform
    // Start is called before the first frame update
   private void Start() //функция срабатывает прис старте
    {
        _rotator = GetComponent<Transform>();  //записываем в переменную компонент Transform
    }

    // Update is called once per frame (много раз в секунду)
    private void Update() 
    {
        _rotator.Rotate(0, speed*Time.deltaTime/*директива сглаживания по фреймам, иначе скорость вращения гигантская*/, 0);  //вращение объекта только по y(ось) 
    }
}
