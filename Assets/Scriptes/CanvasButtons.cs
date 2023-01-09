using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public Sprite musicOn, musicOff;//переменные в которые установим картинки
    private void Start()
    {
        if(PlayerPrefs.GetString("music") == "No" && gameObject.name=="Music")//если при выходе из приложения музыка была выключена, в начале этой игры выключаем музыку && если объект (кнопка) = музыка (чтобы срабатывало тока на этой кнопке, а не на всех подряд)
        GetComponent<Image>().sprite = musicOff;
    }
   public void RestartGame()//перезапуск игры
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();//проигрывание музыки
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//при перезагрузке игры выводим сцену с индексом 0 или SceneManager.GetActiveScene().buildIndex
    }
    public void LoadInstagram() //загружаем инстаграм
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
        Application.OpenURL("https://instagram.com/breakinbender?igshid=Mzc0YWU1OWY="); //осуществляется переход по ссылке
    }
    public void LoadShop()//загрузка магазина
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Shop");//при перезагрузке игры выводим сцену с индексом 0 или SceneManager.GetActiveScene().buildIndex
    }
    public void CloseShop()//выход из магазина (крестик)
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Main");//при перезагрузке игры выводим сцену с индексом 0 или SceneManager.GetActiveScene().buildIndex
    }
    public void CloseMain()//перезапуск игры
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();//проигрывание музыки
        Application.Quit();//выход из приложения
    }
    public void MusicWork()//при нажатии на иконку музыки будет осуществляться вкючение/выключение музыки и смена иконки на противоположную
    {
        if (PlayerPrefs.GetString("music") == "No")
        {//через пользовательские настройки проверяем работает ли музыка
            GetComponent<AudioSource>().Play();
            PlayerPrefs.SetString("music", "Yes");//музыка не работает, надо включить, включение
            GetComponent<Image>().sprite=musicOn;//смена иконки на противоположную
        }
        else//музыка работает, надо выключить
        {
            PlayerPrefs.SetString("music", "No");//выключение музыки
            GetComponent<Image>().sprite=musicOff;//смена иконки на противоположную
        }
    }
}
