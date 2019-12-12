using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    public Transform[] viewersPrefab;
    public Transform[] customPrefab;

    public List<string> currentViewersNames;
    public List<GameObject> currentViewersObjects;

    public List<string> viewersToRemoveString;
    public List<GameObject> viewersToRemoveGameObject;

    public Vector2 minSpawnPos;
    public Vector2 maxSpawnPos;

    bool isNamesHidden = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        // =========== pega a posição in-game máxima da tela ============
        //bottomLeftWorld = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        //bottomRightWorld = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane));
        //topLeftWorld = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane));
        //topRightWorld = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        // ================================================================



        StartCoroutine(CheckViewing_CR());
    }

    void Update()
    {
        //bombCooldown -= Time.deltaTime;
        //if (bombCooldown < 0f)
        //{
        //    bombCooldown = 0f;
        //}

        if (Input.GetKey(KeyCode.Space))
        {
            string name = RandomString(10);
            AddViewer(name);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            DeleteViewersAll();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CheckIfIsViewing();
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            HideNamesAll();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            ShowNamesAll();
        }

        //if (Input.GetKeyDown(KeyCode.RightShift))
        //{
        //    SpawnBomb(new Vector2(0,0), "a", "b");
        //}

    }

    public GameObject GetViewerGameObject(string viewerName)
    {
        if (currentViewersNames.Contains(viewerName))
        {
            int indexOfName = currentViewersNames.IndexOf(viewerName);
            return currentViewersObjects[indexOfName].gameObject;
        }

        return null;
    }

    public void CheckIfIsViewing()
    {
        //print("Checando se está vendo");

        for (int i = 0; i < currentViewersNames.Count; i++) // loop pelos viewers spawnados para checar se está vendo
        {
            if (TwitchChat.Instance.currentViewers.Contains(currentViewersNames[i]) || TwitchChat.Instance.currentMods.Contains(currentViewersNames[i]) || TwitchChat.Instance.currentStreamer.Contains(currentViewersNames[i]) || TwitchChat.Instance.currentVips.Contains(currentViewersNames[i]))
            {
                print("Está vendo");
            }
            else // se não estiver vendo
            {
                viewersToRemoveGameObject.Add(currentViewersObjects[i].gameObject);
                viewersToRemoveString.Add(currentViewersNames[i]);
            }
        }

        print("Removendo " + viewersToRemoveString.Count + " Viewer Strings");

        for (int i = 0; i < viewersToRemoveString.Count; i++)
        {
            for (int a = 0; a < currentViewersNames.Count; a++)
            {
                if (viewersToRemoveString[i] == currentViewersNames[a])
                {
                    currentViewersNames.Remove(currentViewersNames[a]);
                }
            }
        }

        print("Removendo " + viewersToRemoveGameObject.Count + " Viewer GameObjects");

        for (int i = 0; i < viewersToRemoveGameObject.Count; i++)
        {
            for (int a = 0; a < currentViewersObjects.Count; a++)
            {
                if (viewersToRemoveGameObject[i] == currentViewersObjects[a])
                {
                    Destroy(currentViewersObjects[a].gameObject);
                    currentViewersObjects.Remove(currentViewersObjects[a]);
                }
            }
        }

        print("Limpando listas de remoção");

        viewersToRemoveGameObject.Clear();
        viewersToRemoveString.Clear();
    }

    public void AddViewer(string name)
    {

        if (currentViewersNames.Contains(name))
        {
            return;
        }

        Transform go;

        //prefabs customizados para certas pessoas
        if (name == "streamlabs" || name == "moobot" || name == "nightbot")
        {
            print("policia detectada");
            go = Instantiate(viewersPrefab[UnityEngine.Random.Range(0, viewersPrefab.Length)], new Vector2(UnityEngine.Random.Range(minSpawnPos.x, maxSpawnPos.x), UnityEngine.Random.Range(minSpawnPos.y, maxSpawnPos.y)), Quaternion.identity);
        }
        else
        {
            go = Instantiate(viewersPrefab[UnityEngine.Random.Range(0, viewersPrefab.Length)], new Vector2(UnityEngine.Random.Range(minSpawnPos.x, maxSpawnPos.x), UnityEngine.Random.Range(minSpawnPos.y, maxSpawnPos.y)), Quaternion.identity);
        }

        go.GetComponent<Viewer>().SetName(name);

        currentViewersNames.Add(name);
        currentViewersObjects.Add(go.gameObject);


        if (isNamesHidden)
            go.GetComponent<Viewer>().HideName();
        else
            go.GetComponent<Viewer>().ShowName();

    }

    public void HideNamesAll()
    {
        for (int i = 0; i < currentViewersObjects.Count; i++)
        {
            currentViewersObjects[i].gameObject.GetComponent<Viewer>().HideName();
        }
        isNamesHidden = true;
    }

    public void ShowNamesAll()
    {
        for (int i = 0; i < currentViewersObjects.Count; i++)
        {
            currentViewersObjects[i].gameObject.GetComponent<Viewer>().ShowName();
        }
        isNamesHidden = false;
    }

    public void DeleteViewersAll()
    {
        int count = 0;
        int totalObjects = currentViewersNames.Count;
        for (int i = 0; i < totalObjects; i++)
        {
            Destroy(currentViewersObjects[i]);
            count++;
        }

        print(count + " objetos deletados");

        currentViewersNames.Clear();
        currentViewersObjects.Clear();

    }

    public void DeleteAt(int index)
    {
        Destroy(currentViewersObjects[index].gameObject);
        currentViewersNames.RemoveAt(index);
        currentViewersObjects.RemoveAt(index);
    }

    public string RandomString(int length)
    {
        System.Random random = new System.Random();

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    //public void SpawnBomb(Vector2 position, string quemFoi, string emQuem)
    //{
    //    if (bombCooldown == 0f)
    //    {
    //        if(quemFoi != emQuem)
    //        {
    //            Instantiate(bombPrefab, position, Quaternion.identity);
    //            bombCooldown = 10f;
    //        }
    //        else
    //        {
    //            print("vc mesmo");
    //        }
    //        //TwitchChat.Instance.SendChat(quemFoi + " lançou uma bomba em @" + emQuem + ", fuja para as montanhas !!!");
    //    }
    //    else
    //    {
    //        print("bomba em cooldown - " + bombCooldown.ToString("n0") + " restantes");
    //        //TwitchChat.Instance.SendChat("A bomba está em cooldown, você ainda tem que esperar "+ bombCooldown.ToString("n0") +" segundos para bombardear alguém !");
    //
    //    }
    //}

    //public void Bombar(Vector2 position)
    //{
    //    position = new Vector2(position.x, -5.3f);
    //    //position -= new Vector2(0f,-1f);
    //
    //    float radius = 5.0f;
    //    float power = 750.0f;
    //
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
    //    foreach (Collider2D hit in colliders)
    //    {
    //        Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
    //        //Lhama lhama = hit.GetComponent<Lhama>();
    //        Bomb bomba = hit.GetComponent<Bomb>();
    //
    //        //if (rb != null && lhama != null && bomba == null)
    //        //{
    //        //    lhama.moving = false;
    //        //    rb.AddExplosionForce(power, position, radius); // diminuir depois do radius = voa mais alto
    //        //}
    //    }
    //}

    public IEnumerator CheckViewing_CR()
    {
        yield return null;
        //while (true)
        //{
        //    // atualiza a lista de veiwers e checa se estão vendo
        //    TwitchChat.Instance.StartCoroutine(TwitchChat.Instance.HTTPRequest());
        //    yield return new WaitForSeconds(300f);
        //}


    }

}
