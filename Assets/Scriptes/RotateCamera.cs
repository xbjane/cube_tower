using UnityEngine;

public class RotateCamera : MonoBehaviour //�������� ������� Rotator, ������� �������� � ���� ������ � ����, ��������� ������������ �������� ������ ����� ��� �����, ������ ������ ������� �������� (Platform)
{
    public float speed = 5f; //�������� �������� �������, ������ ����� ������ � Unity(public)
    private Transform _rotator; //���������� ��������� �� ��������� Transform
    // Start is called before the first frame update
   private void Start() //������� ����������� ���� ������
    {
        _rotator = GetComponent<Transform>();  //���������� � ���������� ��������� Transform
    }

    // Update is called once per frame (����� ��� � �������)
    private void Update() 
    {
        _rotator.Rotate(0, speed*Time.deltaTime/*��������� ����������� �� �������, ����� �������� �������� ����������*/, 0);  //�������� ������� ������ �� y(���) 
    }
}
