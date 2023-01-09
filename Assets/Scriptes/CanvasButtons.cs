using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public Sprite musicOn, musicOff;//���������� � ������� ��������� ��������
    private void Start()
    {
        if(PlayerPrefs.GetString("music") == "No" && gameObject.name=="Music")//���� ��� ������ �� ���������� ������ ���� ���������, � ������ ���� ���� ��������� ������ && ���� ������ (������) = ������ (����� ����������� ���� �� ���� ������, � �� �� ���� ������)
        GetComponent<Image>().sprite = musicOff;
    }
   public void RestartGame()//���������� ����
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();//������������ ������
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//��� ������������ ���� ������� ����� � �������� 0 ��� SceneManager.GetActiveScene().buildIndex
    }
    public void LoadInstagram() //��������� ���������
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
        Application.OpenURL("https://instagram.com/breakinbender?igshid=Mzc0YWU1OWY="); //�������������� ������� �� ������
    }
    public void LoadShop()//�������� ��������
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Shop");//��� ������������ ���� ������� ����� � �������� 0 ��� SceneManager.GetActiveScene().buildIndex
    }
    public void CloseShop()//����� �� �������� (�������)
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Main");//��� ������������ ���� ������� ����� � �������� 0 ��� SceneManager.GetActiveScene().buildIndex
    }
    public void CloseMain()//���������� ����
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();//������������ ������
        Application.Quit();//����� �� ����������
    }
    public void MusicWork()//��� ������� �� ������ ������ ����� �������������� ��������/���������� ������ � ����� ������ �� ���������������
    {
        if (PlayerPrefs.GetString("music") == "No")
        {//����� ���������������� ��������� ��������� �������� �� ������
            GetComponent<AudioSource>().Play();
            PlayerPrefs.SetString("music", "Yes");//������ �� ��������, ���� ��������, ���������
            GetComponent<Image>().sprite=musicOn;//����� ������ �� ���������������
        }
        else//������ ��������, ���� ���������
        {
            PlayerPrefs.SetString("music", "No");//���������� ������
            GetComponent<Image>().sprite=musicOff;//����� ������ �� ���������������
        }
    }
}
