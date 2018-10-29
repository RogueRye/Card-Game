using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public Slot slotPrefab;

    [Range(3, 10)]
    public int slotsPerRow = 5;
    public float paddingPerRow;

    public List<GameObject> rows = new List<GameObject>(); 

    public static Slot[,] fieldA;
    public static Slot[,] fieldB;



    public void FindRows()
    {
        rows.Clear();
        foreach(Transform child in transform.GetChild(0))
        {
            if (child.CompareTag("Row"))
            {
                rows.Add(child.gameObject);
            }
        }



    }
    public void CreateRows()
    {

        FindRows();
        foreach(GameObject row in rows)
        {
            for (int i = 0; i < slotsPerRow; i++)
            {
                Slot temp = Instantiate(slotPrefab, row.transform);
                temp.transform.position = new Vector3(
                    row.transform.localPosition.x + ((temp.transform.localScale.x / 2)) + (paddingPerRow * i),
                    row.transform.localPosition.y,
                    row.transform.localPosition.z               
                    ); 
            }
        }


    }

    public void CreateFields()
    {
        if (fieldA == null)
        {
            fieldA = new Slot[rows.Count / 2, slotsPerRow];
        }
        if (fieldB == null)
        {
            fieldB = new Slot[rows.Count / 2, slotsPerRow];
        }
        for (int i = 0; i < rows.Count / 2; i++)
        {
            for (int k = 0; k < rows[i].transform.childCount; k++)
            {
                fieldA[i, k] = rows[i].transform.GetChild(k).GetComponent<Slot>();
                Debug.Log(fieldA[i, k] + " is in field A");
            }
        }
        for (int i = rows.Count / 2; i < rows.Count; i++)
        {
            for (int k = 0; k < rows[i].transform.childCount; k++)
            {
                fieldB[(i - (rows.Count / 2)), k] = rows[i].transform.GetChild(k).GetComponent<Slot>();
                Debug.Log(fieldB[(i - (rows.Count / 2)), k] + " is in field B");
            }
        }
    }

    public void DeleteRows()
    {
        
        foreach (GameObject row in rows)
        {
            for (int i = 0; i < row.transform.childCount; i++)
            {
                DestroyImmediate(row.transform.GetChild(i).gameObject);
            }
        }


    }

}
