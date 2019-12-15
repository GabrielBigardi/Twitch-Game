using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Viewer : MonoBehaviour
{
    public Text nameText;

    public Transform bulletPrefab;
    public Transform target;

    public float bulletSpeed;
    public float bulletDelay;

    public int maxLife = 100;
    public int curLife;

    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Bullet")
        {
            //print("Atingido");
            TakeDamage(50);
            Destroy(col.gameObject);
        }
    }

    public void TakeDamage(int damageToTake)
    {
        curLife -= damageToTake;
        if(curLife <= 0)
        {
            int playerIndex = Manager.Instance.currentViewersNames.IndexOf(nameText.text);
            int playersCount = Manager.Instance.currentViewersNames.Count;

            //deleta da lista antes da checagem, caso contrario checagem não iniciará
            Manager.Instance.currentViewersNames.RemoveAt(playerIndex);
            Manager.Instance.currentViewersObjects.RemoveAt(playerIndex);

            //checagem se acabou batalha
            if ((playersCount - 1) <= 1)
            {
                BattleManager.Instance.EndBattle();
            }

            Destroy(gameObject);
        }
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void ShowName()
    {
        nameText.enabled = true;
    }

    public void HideName()
    {
        nameText.enabled = false;
    }

    public void Attack(Transform alvo)
    {
        StartCoroutine(Attack_CR(alvo));
    }

    public IEnumerator Attack_CR(Transform alvo)
    {
        target = alvo;
        while (true)
        {
            if (target == null)
                break;

            Vector2 distance = target.position - transform.position;
            Vector2 shootPos = distance.normalized;
            Vector3 instantiatePos = new Vector3(shootPos.x * 0.4f, shootPos.y * 0.4f, 0f);

            Transform bullet = Instantiate(bulletPrefab, transform.position + instantiatePos, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().AddForce(shootPos * bulletSpeed);
            yield return new WaitForSeconds(bulletDelay);
        }
        
    }
}
