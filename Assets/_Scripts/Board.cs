using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public Slot slotPrefab;

    [Range(3, 10)]
    public int slotsPerRow = 5;
    public float paddingPerRow;

    public List<GameObject> rows = new List<GameObject>(); 

    


    public void FindRows()
    {
        rows.Clear();
        foreach(Transform child in gameObject.transform)
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

    public void DeleteRows()
    {
        
        foreach (GameObject row in rows)
        {
            foreach (Transform child in row.transform)
            {                
                DestroyImmediate(child.gameObject);
            }
        }


    }

}
