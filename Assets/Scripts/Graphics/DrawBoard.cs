using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBoard : MonoBehaviour
{
    public GameObject squarePrefab;
    void Start()
    {
        GameObject squareHolder = new GameObject("SquareHolder");
        
        int row = 0;

        for (int i = 0; i < 64; i++)
        {
            (float x, float y) = PositionHelper.ArrayPositionToWorldPosition(i);
            GameObject square = Instantiate(squarePrefab, new Vector3(x, y, 0), Quaternion.identity);
            square.name = i.ToString();
            square.transform.SetParent(squareHolder.transform);
            square.transform.tag = "square";


            if (row % 2 == 0)
            {
                if (i % 2 == 0)
                {
                    GameObject.Find(i.ToString()).GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    GameObject.Find(i.ToString()).GetComponent<SpriteRenderer>().color = Color.black;
                }
            }
            else
            {
                if (i % 2 == 0)
                {
                    GameObject.Find(i.ToString()).GetComponent<SpriteRenderer>().color = Color.black;
                }
                else
                {
                    GameObject.Find(i.ToString()).GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
            

            if ((i + 1) % 8 == 0)
            {
                row++;
            }
        }
    }
}
