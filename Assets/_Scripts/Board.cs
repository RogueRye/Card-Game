using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board instance;

    public RectTransform board;
    public Slot slotPrefab;

    [Range(3, 10)]
    public int slotsPerRow = 5;
    public float paddingPerRow;

    public List<GameObject> rows = new List<GameObject>();

    public static Slot[,] fieldA;
    public static Slot[,] fieldB;


    public void Awake()
    {
        if (instance == null)
            instance = this;
        CreateFields();

    }

    public void FindRows()
    {
        rows.Clear();
        foreach (Transform child in transform.GetChild(0))
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
        foreach (GameObject row in rows)
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
                //i = row number, k = spot in row
                var temp = rows[i].transform.GetChild(k).GetComponent<Slot>();
                fieldA[i, k] = temp;
                temp.Init(i, k);                
            }
        }
        for (int i = rows.Count / 2; i < rows.Count; i++)
        {
            for (int k = 0; k < rows[i].transform.childCount; k++)
            {
                var temp = rows[i].transform.GetChild(k).GetComponent<Slot>();
                fieldB[(i - (rows.Count / 2)), k] = temp;
                temp.Init((i - (rows.Count / 2)), k);
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
