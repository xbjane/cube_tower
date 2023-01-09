using UnityEngine;

public class ExplodeCubes : MonoBehaviour //организация взрыва кубов при падении башни
{
    public GameObject restartButton, explosion;//объект для вывода кнопки повторной пробы игры; игровой эффект, ссылающийся на эффект взрыва
    bool _collisionSet;
    private void OnCollisionEnter(Collision collision){//функция срабатывающая при соприкосновении с объектом (Ground)
        if (collision.gameObject.tag == "Cube"&&!_collisionSet) 
        {//проверка является ли объект с которым произошло соприкосновение, кубом && проверка вызывается ли функция впервые, вдруг какой-то ещё объект будет назван Cube
            for (int i = collision.transform.childCount - 1; i >= 0; i--)
            { //перебираем все объекты от последнего созданного до первого(от 3 до 0, к примеру)
                Transform child = collision.transform.GetChild(i);//каждый раз создаём объект обозначающий новый куб
                child.gameObject.AddComponent<Rigidbody>();//присаиваем каждому объекту компонент Rigidbody, который отвечает за физику падения кубов
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(70f/*номинал силы*/, Vector3.up/*направление y*/, 5f/*радиус действия*/);//получаем добавленный компонент и придаём падению взрывную силу
                child.SetParent(null);//делаем каждый из кубов независимым от родителя(All Cubes)
            }
            restartButton.SetActive(true);//делаем видимой кнопку перезапуска
            Camera.main.transform.localPosition -= new Vector3(0,0,3f);//сдвиг камеры в момент пройгрыша на вектор(0,0,3)
            Camera.main.gameObject.AddComponent<CameraShake>();//включаем другой скрипт: трясём камеру уже после отдаления
            GameObject newExplosion=Instantiate(explosion, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z)/*когда произойдет первый контакт*/, Quaternion.identity) as GameObject;//после отдаления и тряски происходит взрыв
            Destroy(newExplosion, 2.5f);//удаляем эффект, чтобы он не засорял память
            if (PlayerPrefs.GetString("music") != "No")//включение звука взрыва
                GetComponent<AudioSource>().Play();
            Destroy(collision.gameObject);//уничтожаем объект-родитель(All Cubes)
            _collisionSet = true;
        }
    }
}
