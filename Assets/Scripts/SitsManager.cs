using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitsManager : MonoBehaviour
{

    public int horizontalSits;
    public int verticalSits;
    public Transform prefab_Sit1;
    public Transform prefab_Sit2;

    public Vector2 startingPos;
    Vector2 currentSpawnPos;

    public float xOffset;

    public Vector3 fileiraOffset;
    public Vector3 newFileiraOffset;

    public List<Transform> bonecos;

    void Start()
    {


        for (int x = 0; x < verticalSits; x++) // para cada fileira vertical
        {
            string curGOName = "Fileira " + (x + 1);
            GameObject fileira = new GameObject(curGOName);

            currentSpawnPos = startingPos;
            for (int y = 0; y < horizontalSits; y++) // spawna uma fileira horizontal
            {
                Transform goObject = Instantiate(prefab_Sit1, currentSpawnPos, Quaternion.identity, fileira.transform);
                bonecos.Add(goObject);
                currentSpawnPos += new Vector2(xOffset, 0f);
                print("Pos: " + currentSpawnPos);
            }

            newFileiraOffset = (fileiraOffset*(x));
            fileira.transform.position = newFileiraOffset;
            print(newFileiraOffset);
        }

    }
}
