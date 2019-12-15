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

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            string name = RandomString(10);
            AddViewer(name);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            DeleteViewersAll();
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            HideNamesAll();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            ShowNamesAll();
        }

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
            //print("policia detectada");
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

        //print(count + " objetos deletados");

        currentViewersNames.Clear();
        currentViewersObjects.Clear();

    }

    public void DeleteAt(int index)
    {
        if(currentViewersObjects[index] != null)
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

}
