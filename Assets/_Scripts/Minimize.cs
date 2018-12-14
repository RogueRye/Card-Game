using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Minimize : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    public RectTransform target;

    Button m_btn;
    Vector2 startSize;
    
    bool isChanging = false;
    private void Start()
    {
        m_btn = GetComponent<Button>();
        m_btn.onClick.AddListener(() => Action());

        startSize.x = target.rect.width;
        startSize.y = target.rect.height;

        
    }

    public void Action()
    {
        
        if (!isChanging)
        {

            if (target.rect.height <= 25f)
            {
                Grow();
            }
            else
            {
                Shrink();
            }
        }
    }

     void Shrink()
    {
        var target = Vector2.one;
        StartCoroutine(AlterSize(target));
    }

     void Grow()
    {
        StartCoroutine(AlterSize(startSize));
    }

    IEnumerator AlterSize(Vector2 goal)
    {
        
        isChanging = true;
        while (Vector2.Distance(target.rect.size, goal) >= .3f)
        {            
            target.sizeDelta = Vector2.Lerp(target.rect.size, goal, Time.deltaTime * 20f);
            yield return null;
        }

        target.sizeDelta = goal;
        isChanging = false;

    }


    public virtual void OnDrag(PointerEventData eventData)
    {

        RectTransform m_DraggingPlane = GetComponentInParent<Canvas>().gameObject.transform as RectTransform;


        var rt = target;
        Vector3 globalMousePos;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
          
                rt.position = globalMousePos;
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {

    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
       
    }

}
