using UnityEngine;

public class IsEnabled : MonoBehaviour
{
    public int needToUnlock;//����, ����������� ��� �������� ���������
    public Material blackMaterial;//��������(������) ��������
    private void Start()
    {
        if (PlayerPrefs.GetInt("best score") < needToUnlock)//���� ������ ������, ��� ����, ����������� ��� �������� ��������� 
            GetComponent<MeshRenderer>().material=blackMaterial;//��������� MeshRenderer �������� �� ��������
    }
}
