using UnityEngine;

public class IsEnabled : MonoBehaviour
{
    public int needToUnlock;//счёт, необходимый для открытия материала
    public Material blackMaterial;//закрытый(чёрный) материал
    private void Start()
    {
        if (PlayerPrefs.GetInt("best score") < needToUnlock)//если рекорд меньше, чем счёт, необходимый для открытия материала 
            GetComponent<MeshRenderer>().material=blackMaterial;//компонент MeshRenderer отвечает за материал
    }
}
