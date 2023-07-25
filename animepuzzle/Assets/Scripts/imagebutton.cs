using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class imagebutton : MonoBehaviour
{
    public int serieNumber;

    public GameManager gameManager;

    public Vector3 position;
    public Vector3 mainPosition;

    public void Start()
    {
        mainPosition = transform.parent.GetComponent<RectTransform>().anchoredPosition;
    }


    public void Update()
    {
        transform.parent.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(transform.parent.GetComponent<RectTransform>().anchoredPosition, mainPosition, gameManager.movingTime);

        if(gameManager.checkStatusBool && !gameManager.movingObject)
        {
            if(PlayerPrefs.GetInt("currentlevel",0) + 1 > PlayerPrefs.GetInt("reachedlevel", 0))
            {
                PlayerPrefs.SetInt("reachedlevel", PlayerPrefs.GetInt("currentlevel", 0)+1);
            }
            Invoke("End", 0.4f);
        }
    }

    public void End()
    {
        transform.parent.GetChild(1).GetComponent<CanvasGroup>().alpha = Mathf.Lerp(transform.parent.GetChild(1).GetComponent<CanvasGroup>().alpha, 0, gameManager.borderDisappearTime);
        transform.parent.GetChild(0).GetComponent<Button>().interactable = false;
    }
}
