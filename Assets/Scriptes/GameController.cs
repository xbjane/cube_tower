using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class GameController : MonoBehaviour
{
    private CubePosition nowCube = new CubePosition(0, 1, 0);//������ ������ �� ������ ��������� ����� �����������
    public float cubeChangePlaceSpeed = 0.5f;//���������� ���������� �������� ��� ������ �����, ��� ����� ��������� ����� ����� ����� ������ ���� �������
    public Transform cubeToPlace;//����������, ���������� ������ �� ������ Cube to Place 
    public GameObject[] cubesToCreate;//������ (������ �����) ����� (����) ������� ����� ������� 
    public GameObject allCubes,vfx; //������ �� allCubes, ������� ���� �������� � All Cubes;��������� ���;������ ����������� ����
    public Text scoreTxt;//���������� ��� ������ ����������� ����� � ������� �� �����
    private Rigidbody allCubesRB;//����������, ��������� ��� ���������� Rigidbody � ���������� ������� ����� 
    private bool isLose, firstCube;//���������� ��� ���������� ������������ ���������
    public GameObject[] canvasStartPage;//������ ��������
   
    private float camMoveToYPosition, camMoveSpeed=2f;//���������� ��� ������������ ������ �� �
    public Color[] bGColors;//������, ���������� �����
    private Color toCameraColor;//��� �������� ��������� �������� ����� ������� ����  
    private List<Vector3> AllCubesPositions = new List<Vector3> { //������������ ������ �������� ��� ����������, � ������� ��� ��������� ����
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
    private List<GameObject> possibleCubesToCreate= new List<GameObject>();//������ ����� ����� ������� ����� �������
    private int prevCountMaxHorizontal;//��������� prevCountMaxHorizontal ������� ��� ���������� ������������ ��������� ������
    private Transform mainCam;//����������, ������������ ������
    private Coroutine showCubePlace; //������ ������ ��������, ����������� ��� ��������� ��������� � ������ ��������
    private void Start() //������
    {
        if (PlayerPrefs.GetInt("best score") < 20)//��������� ���� ����� ��� �������� ��� ������ ��������
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
        scoreTxt.text = "<size=28>score:</size> 0\n<size=25>best:</size> " + PlayerPrefs.GetInt("best score"); //������� ������ � ������� � ������� ����� �� ������
        toCameraColor = Camera.main.backgroundColor;//� ������ ���� ������������� �������� ����� ���� ������� ������� � ������ ����
        mainCam = Camera.main.transform; //������ ����������, ������������ ������
        camMoveToYPosition = 2.9f + nowCube.y - 1f;
        allCubesRB = allCubes.GetComponent<Rigidbody>();
        StartCoroutine(ShowCubePlace());
    }
    private void Update() //���������� � ����� ������ ����� �� ������������ �� �������
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && cubeToPlace!= null && allCubes != null && !EventSystem.current.IsPointerOverGameObject())
        {//����� �� ������������ �� ����� ������� ���� || ���������� ������� �� ������ >0 ) && ������ ��������� ������� ��� ���������� && ������ ��� ���� ��� ���������� && ��� ������� �� ������� ����������������� ���������� ���������� ����� �� �������, ������� �� ���� �� �����
            if (!firstCube) //������� �������� UI ��� ������ ���� (���������� ������� ����)
            {
                firstCube = true;//����� �������� ��������� �� ������ ������ ����
                foreach (GameObject obj in canvasStartPage)//���������� ������ ������
                    Destroy(obj);
            }
            GameObject createCube = null;
            if (possibleCubesToCreate.Count == 1)//���� �������� ������ ���� ��� ����
                createCube = possibleCubesToCreate[0];//�� ����������� ��� �� ����� ���������� � �������������� �� �����
            else
                createCube = possibleCubesToCreate[UnityEngine.Random.Range(0, possibleCubesToCreate.Count)];//����� ���������� ��������� �������
                GameObject newCube = Instantiate( //�������� ����� ������� ������ � ������� ������� Instantiate
                  createCube,//�������� ��� (������)
                  cubeToPlace.position,//�� ��������� ������� 
                  Quaternion.identity //�� ������ �������� �������
                  ) as GameObject;
            newCube.transform.SetParent(allCubes.transform); //������������� �������� All Cubes ��� ���������� ���� 
            nowCube.setVector(cubeToPlace.position);//������� ����� ����� �������� ��������� ������������ ���
            AllCubesPositions.Add(nowCube.getVector()); //��������� ������ ������� � �������

            if (PlayerPrefs.GetString("music") != "No")//�������� ���� ��������� ����
                GetComponent<AudioSource>().Play();

           GameObject newVfx = Instantiate(vfx, cubeToPlace.position, Quaternion.identity) as GameObject;//������ ������ ��������� ����
            Destroy(newVfx, 1.5f);//������� ������, ����� �� �� ������� ������
            allCubesRB.isKinematic = true;//������������ �������
            allCubesRB.isKinematic = false;//������������ �������

            SpawnPositions();//�� ������ ������ �������� ����� ����� ��������� ������� �� ����� ��������� ���� ����������
            MoveCameraChangeBG();//����� ����, ��� ��������� ����� ��������� ������������ ����� � ����������� ������
        }
        if (!isLose && allCubesRB.velocity.magnitude > 0.1f)
        {//��������� ��������� ����� ��� �������� ���������(magnitude - �������� �������)
            Destroy(cubeToPlace.gameObject);//��������� ������ ��������� ������� ��� ����
            isLose = true; //��� ���������� ������������ ���������
           // StopCoroutine(showCubePlace);//������������� �������� ����� ������, �� �����������, ����� �� ���� �������� ������� ��������
        }

        mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition, new Vector3(mainCam.localPosition.x, camMoveToYPosition, mainCam.localPosition.z), camMoveSpeed*Time.deltaTime);//��� �������� �������� ������������ ������ �� �
        if (Camera.main.backgroundColor != toCameraColor)
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);//������ ������������� ���� ������� ���� 
    }
    IEnumerator ShowCubePlace()
    { //��������� ��������, ���������� �� ����� ����, ��������� ��� ������� ������ ����
        while (cubeToPlace!=null)
        {//����������� ����(�����������, ������ ���� �� ���������)
            SpawnPositions();
            yield return new WaitForSeconds(cubeChangePlaceSpeed);//����� ������ ���������� ��� � �������� cubeChangePlaceSpeed
        }
    }
    private void SpawnPositions() //�����, �������� �����
    {
        List<Vector3> positions = new List<Vector3>();//������������ ������ (������������ ������ ������), ��� ������ - Vector3
                                                      //� ������ ����������� ��� ��������� ��������, ��� ����� � ������ ������ ����������� ���
        if (IsPositionEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z)) && nowCube.x + 1 != cubeToPlace.position.x)//������� �������� �������� �� �����, � ������ ���� � ������� ��������� ���������� ������� ���� �� ������� �� 1 �� � 
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));//��������� � ������ ��������� ���� ����������� ���������� �����
        if (IsPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z)) && nowCube.x - 1 != cubeToPlace.position.x)//������� �������� �������� �� �����, � ������ ���� � ������� ��������� ���������� ������� ���� �� ������� �� 1 �� � 
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z)) && nowCube.y + 1 != cubeToPlace.position.y)//������� �������� �������� �� �����, � ������ ���� � ������� ��������� ���������� ������� ���� �� ������� �� 1 �� � 
            positions.Add(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z)) && nowCube.y - 1 != cubeToPlace.position.y)//������� �������� �������� �� �����, � ������ ���� � ������� ��������� ���������� ������� ���� �� ������� �� 1 �� � 
            positions.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1)) && nowCube.z + 1 != cubeToPlace.position.z)//������� �������� �������� �� �����, � ������ ���� � ������� ��������� ���������� ������� ���� �� ������� �� 1 �� � 
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1)) && nowCube.z - 1 != cubeToPlace.position.z)//������� �������� �������� �� �����, � ������ ���� � ������� ��������� ���������� ������� ���� �� ������� �� 1 �� � 
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));
        if (positions.Count > 1)
            cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)]; //��������� ������� �������� �� ������ ��������� ��� ���������
        else if (positions.Count == 0)
            isLose = true;//���� ������������ �������� ����� � �����, �� �� ��������
        else//���� ���������� ������ ���� �������, � ������� ����� ����������� ���
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
        return true;//���� ������ �� ���������, ������ ������� ��������
    }
   private void MoveCameraChangeBG() //����� ������ ��� ���������� ����� && ��������� ������� ����
    {
        int maxX = 0, maxY = 0, maxZ = 0, maxHor;//������������ �������� �� ��� �����������
foreach(Vector3 pos in AllCubesPositions) //���������� ������ ������ �� ������� ��� ����� ���
        {
            if (Mathf.Abs(Convert.ToInt32(pos.x)) > maxX)
                maxX = Mathf.Abs(Convert.ToInt32(pos.x)); //������� ������������� �������� �� 3 �����������, ����� �������, ��� ����� ����
            if (Convert.ToInt32(pos.y) > maxY)
                maxY = Convert.ToInt32(pos.y);
            if (Mathf.Abs(Convert.ToInt32(pos.z)) > maxZ)
                maxZ = Mathf.Abs(Convert.ToInt32(pos.z));
        }
        maxY--;
        if (PlayerPrefs.GetInt("best score") < maxY)//���� �������� �������� ������ ����, ��� ������, ������������� ����� ������(����� ���������������� ���������, � � ��� �����������)
            PlayerPrefs.SetInt("best score", maxY);
        scoreTxt.text="<size=28>score:</size> " + maxY + "\n<size=25>best:</size> " + PlayerPrefs.GetInt("best score"); //������� ������ � ����������� �� ������

        camMoveToYPosition = 2.9f + nowCube.y - 1f;//������������� ����� �������� ������ ��� ������ ���������� ����
        //���������� ������, ����� ����� ����� �� x � z
        maxHor = maxX > maxZ ? maxX : maxZ;//���������� � ���� ������ ������������ ��������
        if (maxHor % 3 == 0&&prevCountMaxHorizontal!=maxHor)
        {
            mainCam.localPosition -= new Vector3(0, 0, 2f); //����� ��� ������ ���������� �� 3 ����
            prevCountMaxHorizontal = maxHor; 
        }
        if (maxY >= 5)//������ ���� ������� ����, ������� � �������� �������, ����� ����������� ������ ���� ������� � ���������� ������, � �� ��� ������
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
{//��������� �������� �� �������� ��������� ������-���� �������
    public int x, y, z;//����������-����������
    public CubePosition(int x, int y, int z)//����������� � �����������-������������
    {
        this.x = x;//���� ��������� ��������� �������� �� ������������
        this.y = y;
        this.z = z;
    }
    public Vector3 getVector()
    {
        return new Vector3(x, y, z); //����� ���������� ������ �� 3-�� ���������
    }
    public void setVector(Vector3 position)//����� ������ �� ���������� � ������������� �������� ��������� ����� �������� �������
    {
        x = Convert.ToInt32(position.x); //������������ �� float � int
        y = Convert.ToInt32(position.y);
        z = Convert.ToInt32(position.z);
    }
}