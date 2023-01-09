using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class GameController : MonoBehaviour
{
    private CubePosition nowCube = new CubePosition(0, 1, 0);//создаём объект на основе структуры через конструктор
    public float cubeChangePlaceSpeed = 0.5f;//переменная показывает значение как быстро кубик, где можно поставить новый кубик может менять свою позицию
    public Transform cubeToPlace;//переменная, содержащая ссылку на объект Cube to Place 
    public GameObject[] cubesToCreate;//массив (разных видов) кубов (всех) которые можно создать 
    public GameObject allCubes,vfx; //ссылка на allCubes, которую надо помещать в All Cubes;созданный куб;эффект выставления куба
    public Text scoreTxt;//переменная для вывода результатов счёта и рекорда на экран
    private Rigidbody allCubesRB;//переменная, созданная для обновления Rigidbody и возможного падения башни 
    private bool isLose, firstCube;//переменная для единичного срабатывания пройгрыша
    public GameObject[] canvasStartPage;//массив объектов
   
    private float camMoveToYPosition, camMoveSpeed=2f;//переменная для передвижения камеры по у
    public Color[] bGColors;//массив, содержащий цвета
    private Color toCameraColor;//для создания плавности перехода цвета заднего фона  
    private List<Vector3> AllCubesPositions = new List<Vector3> { //динамический список содержит все координаты, в которых уже находятся кубы
    new Vector3(0,0,0),
    new Vector3(-1,0,0),
    new Vector3(1,0,0),
    new Vector3(0,1,0),
    new Vector3(0,0,1),
    new Vector3(0,0,-1),
    new Vector3(1,0,1),
    new Vector3(-1,0,1),
    new Vector3(-1,0,-1),
    new Vector3(1,0,-1),
};
    private List<GameObject> possibleCubesToCreate= new List<GameObject>();//список видов кубов которые можно создать
    private int prevCountMaxHorizontal;//перменная prevCountMaxHorizontal введена для единичного срабатывания отдаления камеры
    private Transform mainCam;//переменная, обозначающая камеру
    private Coroutine showCubePlace; //объект класса куратина, необходимый для остановки созданной в начале куратины
    private void Start() //начало
    {
        if (PlayerPrefs.GetInt("best score") < 20)//добавляем виды кубов для создания при разных рекордах
            AddPossibleCubes(1);
        else if (PlayerPrefs.GetInt("best score") < 40)
        {
            AddPossibleCubes(2);
        }
        else if (PlayerPrefs.GetInt("best score") < 65)
        {
            AddPossibleCubes(3);
        }
        else if (PlayerPrefs.GetInt("best score") < 90)
        {
            AddPossibleCubes(4);
        }
        else if (PlayerPrefs.GetInt("best score") < 120)
        {
            AddPossibleCubes(5);
        }
        else if (PlayerPrefs.GetInt("best score") < 150)
        {
            AddPossibleCubes(6);
        }
        else if (PlayerPrefs.GetInt("best score") < 200)
        {
            AddPossibleCubes(7);
        }
        else AddPossibleCubes(8);
        scoreTxt.text = "<size=28>score:</size> 0\n<size=25>best:</size> " + PlayerPrefs.GetInt("best score"); //выводим запись о рекорде и нулевом счёте на экране
        toCameraColor = Camera.main.backgroundColor;//в начале игры устанавливаем значение цвета фона который имеется в начале игры
        mainCam = Camera.main.transform; //вводим переменную, обозначающую камеру
        camMoveToYPosition = 2.9f + nowCube.y - 1f;
        allCubesRB = allCubes.GetComponent<Rigidbody>();
        StartCoroutine(ShowCubePlace());
    }
    private void Update() //обновление с целью узнать нажал ли пользователь на клавишу
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && cubeToPlace!= null && allCubes != null && !EventSystem.current.IsPointerOverGameObject())
        {//нажал ли пользователь на левую клавишу мыши || количество пальцев на экране >0 ) && объект свободная позиция ещё существует && объект все кубы ещё существует && при нажатии на элемент пользовательского интерфейса происходит выход из функции, реакции на клик не будет
            if (!firstCube) //скрытие объектов UI при начале игры (построении второго куба)
            {
                firstCube = true;//чтобы действие произошло не больше одного раза
                foreach (GameObject obj in canvasStartPage)//уничтажаем каждый объект
                    Destroy(obj);
            }
            GameObject createCube = null;
            if (possibleCubesToCreate.Count == 1)//если добавлен только один вид куба
                createCube = possibleCubesToCreate[0];//то создаваемый куб не будет выбираться а использоваться по факту
            else
                createCube = possibleCubesToCreate[UnityEngine.Random.Range(0, possibleCubesToCreate.Count)];//иначе выбирается случайным образом
                GameObject newCube = Instantiate( //создаётся новый игровой объект с помощью функции Instantiate
                  createCube,//создаётся куб (префаб)
                  cubeToPlace.position,//на свободной позиции 
                  Quaternion.identity //не меняем вращение объекта
                  ) as GameObject;
            newCube.transform.SetParent(allCubes.transform); //устанавливаем родителя All Cubes для созданного куба 
            nowCube.setVector(cubeToPlace.position);//текущим кубом будет являться последний поставленный куб
            AllCubesPositions.Add(nowCube.getVector()); //добавляем данную позицию в занятую

            if (PlayerPrefs.GetString("music") != "No")//включаем звук установки куба
                GetComponent<AudioSource>().Play();

           GameObject newVfx = Instantiate(vfx, cubeToPlace.position, Quaternion.identity) as GameObject;//создаём эффект установки куба
            Destroy(newVfx, 1.5f);//удаляем эффект, чтобы он не засорял память
            allCubesRB.isKinematic = true;//переключение галочки
            allCubesRB.isKinematic = false;//переключение галочки

            SpawnPositions();//на всякий случай вызываем метод чтобы свободные позиции уж точно правильно были просчитаны
            MoveCameraChangeBG();//после того, как поставили кубик проверяем максимальный кубик и передвигаем камеру
        }
        if (!isLose && allCubesRB.velocity.magnitude > 0.1f)
        {//проверяем состояние башни для создания пройгрыша(magnitude - скорость качания)
            Destroy(cubeToPlace.gameObject);//разрушаем объект свободная позиция для куба
            isLose = true; //для единичного срабатывания пройгрыша
           // StopCoroutine(showCubePlace);//останавливаем куратину через обЪект, не прописываем, чтобы не было проблемы нулевой куратины
        }

        mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition, new Vector3(mainCam.localPosition.x, camMoveToYPosition, mainCam.localPosition.z), camMoveSpeed*Time.deltaTime);//для создания плавного передвижения камеры по у
        if (Camera.main.backgroundColor != toCameraColor)
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);//плавно устанавливаем цвет заднего фона 
    }
    IEnumerator ShowCubePlace()
    { //описываем куратину, отвечающую за показ мест, возможных для вставки нового куба
        while (cubeToPlace!=null)
        {//бесконечный цикл(остановится, только если мы пройграем)
            SpawnPositions();
            yield return new WaitForSeconds(cubeChangePlaceSpeed);//смена показа происходит раз в значение cubeChangePlaceSpeed
        }
    }
    private void SpawnPositions() //метод, меняющий места
    {
        List<Vector3> positions = new List<Vector3>();//динамический список (динамический массив данных), тип данных - Vector3
                                                      //в список добавляются все возможные варианты, где можно в данный момент расположить куб
        if (IsPositionEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z)) && nowCube.x + 1 != cubeToPlace.position.x)//функция проверки свободно ли место, в начале игры в векторе находятся координаты первого куба со сдвигом на 1 по х 
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));//добавляем в список свободных мест проверенное свобоодное место
        if (IsPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z)) && nowCube.x - 1 != cubeToPlace.position.x)//функция проверки свободно ли место, в начале игры в векторе находятся координаты первого куба со сдвигом на 1 по х 
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z)) && nowCube.y + 1 != cubeToPlace.position.y)//функция проверки свободно ли место, в начале игры в векторе находятся координаты первого куба со сдвигом на 1 по х 
            positions.Add(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z)) && nowCube.y - 1 != cubeToPlace.position.y)//функция проверки свободно ли место, в начале игры в векторе находятся координаты первого куба со сдвигом на 1 по х 
            positions.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1)) && nowCube.z + 1 != cubeToPlace.position.z)//функция проверки свободно ли место, в начале игры в векторе находятся координаты первого куба со сдвигом на 1 по х 
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1)) && nowCube.z - 1 != cubeToPlace.position.z)//функция проверки свободно ли место, в начале игры в векторе находятся координаты первого куба со сдвигом на 1 по х 
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));
        if (positions.Count > 1)
            cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)]; //случайная выборка элемента из списка свободных для выделения
        else if (positions.Count == 0)
            isLose = true;//если пользователь построил башню в тупик, то он пройграл
        else//если существует только одна позиция, в которой можно расположить куб
            cubeToPlace.position = positions[0];
    }
    private bool IsPositionEmpty(Vector3 targetPosition)
    {
        if (targetPosition.y == 0)
            return false;
        foreach (Vector3 pos in AllCubesPositions)
        {
            if (pos.x == targetPosition.x && pos.y == targetPosition.y && pos.z == targetPosition.z)
                return false;
        }
        return true;//если ничего не сработало, значит позиция свободна
    }
   private void MoveCameraChangeBG() //сдвиг камеры при увеличении башни && изменение заднего фона
    {
        int maxX = 0, maxY = 0, maxZ = 0, maxHor;//максимальные значения по трём координатам
foreach(Vector3 pos in AllCubesPositions) //перебираем каждый вектор на котором уже стоит куб
        {
            if (Mathf.Abs(Convert.ToInt32(pos.x)) > maxX)
                maxX = Mathf.Abs(Convert.ToInt32(pos.x)); //находим максимаальные значения по 3 координатам, среди позиций, где естть кубы
            if (Convert.ToInt32(pos.y) > maxY)
                maxY = Convert.ToInt32(pos.y);
            if (Mathf.Abs(Convert.ToInt32(pos.z)) > maxZ)
                maxZ = Mathf.Abs(Convert.ToInt32(pos.z));
        }
        maxY--;
        if (PlayerPrefs.GetInt("best score") < maxY)//если нынешнее значение высоты выше, чем рекорд, устанавливаем новый рекорд(через пользовательские настройки, т к они сохраняются)
            PlayerPrefs.SetInt("best score", maxY);
        scoreTxt.text="<size=28>score:</size> " + maxY + "\n<size=25>best:</size> " + PlayerPrefs.GetInt("best score"); //выводим записи о результатах на экране

        camMoveToYPosition = 2.9f + nowCube.y - 1f;//устанавливаем новое значение камеры при каждом построении куба
        //отодвигаем камеру, чтобы иметь обзор по x и z
        maxHor = maxX > maxZ ? maxX : maxZ;//определяем у кого больше максимальное значение
        if (maxHor % 3 == 0&&prevCountMaxHorizontal!=maxHor)
        {
            mainCam.localPosition -= new Vector3(0, 0, 2f); //сдвиг при каждом увеличении на 3 куба
            prevCountMaxHorizontal = maxHor; 
        }
        if (maxY >= 5)//меняем цвет заднего фона, условия в обратном порядке, чтобы выполнялось только одно условие с увеличение высоты, а не все подряд
            toCameraColor = bGColors[0];
        else if (maxY >= 10)
            toCameraColor = bGColors[1];
        else if (maxY >= 15)
            toCameraColor = bGColors[2];
        else if (maxY >= 20)
            toCameraColor = bGColors[3];
        else if (maxY >= 25)
            toCameraColor = bGColors[4];
        else if (maxY >= 30)
            toCameraColor = bGColors[5];
        else if (maxY >= 35) 
            toCameraColor = bGColors[6];
    }
    private void AddPossibleCubes(int till)
    {
        for(int i=0;i<till;i++)
            possibleCubesToCreate.Add(cubesToCreate[i]);
    }
}
struct CubePosition
{//структура отвечает за хранение координат какого-либо объекта
    public int x, y, z;//переменные-координаты
    public CubePosition(int x, int y, int z)//конструктор с параметрами-координатами
    {
        this.x = x;//поля структуры принимают значения из конструктора
        this.y = y;
        this.z = z;
    }
    public Vector3 getVector()
    {
        return new Vector3(x, y, z); //метод возвращает вектор из 3-ёх координат
    }
    public void setVector(Vector3 position)//метод ничего не возвращает а устанавливает значения координат через значения вектора
    {
        x = Convert.ToInt32(position.x); //конвертируем из float в int
        y = Convert.ToInt32(position.y);
        z = Convert.ToInt32(position.z);
    }
}