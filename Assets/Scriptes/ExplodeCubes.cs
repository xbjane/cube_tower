using UnityEngine;

public class ExplodeCubes : MonoBehaviour //����������� ������ ����� ��� ������� �����
{
    public GameObject restartButton, explosion;//������ ��� ������ ������ ��������� ����� ����; ������� ������, ����������� �� ������ ������
    bool _collisionSet;
    private void OnCollisionEnter(Collision collision){//������� ������������� ��� ��������������� � �������� (Ground)
        if (collision.gameObject.tag == "Cube"&&!_collisionSet) 
        {//�������� �������� �� ������ � ������� ��������� ���������������, ����� && �������� ���������� �� ������� �������, ����� �����-�� ��� ������ ����� ������ Cube
            for (int i = collision.transform.childCount - 1; i >= 0; i--)
            { //���������� ��� ������� �� ���������� ���������� �� �������(�� 3 �� 0, � �������)
                Transform child = collision.transform.GetChild(i);//������ ��� ������ ������ ������������ ����� ���
                child.gameObject.AddComponent<Rigidbody>();//���������� ������� ������� ��������� Rigidbody, ������� �������� �� ������ ������� �����
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(70f/*������� ����*/, Vector3.up/*����������� y*/, 5f/*������ ��������*/);//�������� ����������� ��������� � ������ ������� �������� ����
                child.SetParent(null);//������ ������ �� ����� ����������� �� ��������(All Cubes)
            }
            restartButton.SetActive(true);//������ ������� ������ �����������
            Camera.main.transform.localPosition -= new Vector3(0,0,3f);//����� ������ � ������ ��������� �� ������(0,0,3)
            Camera.main.gameObject.AddComponent<CameraShake>();//�������� ������ ������: ����� ������ ��� ����� ���������
            GameObject newExplosion=Instantiate(explosion, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z)/*����� ���������� ������ �������*/, Quaternion.identity) as GameObject;//����� ��������� � ������ ���������� �����
            Destroy(newExplosion, 2.5f);//������� ������, ����� �� �� ������� ������
            if (PlayerPrefs.GetString("music") != "No")//��������� ����� ������
                GetComponent<AudioSource>().Play();
            Destroy(collision.gameObject);//���������� ������-��������(All Cubes)
            _collisionSet = true;
        }
    }
}
