using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public float maxScaleTransition;

    public float transitionSpeed;

    public GameObject transitionObject;
    public GameObject levelTransitionObject;


    public bool levelSelected;

    public GameObject mainlevelPanel;

    [SerializeField] GameObject pixiesMain;

    public GameObject pixie;
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;


        for(int i=0; i < mainlevelPanel.transform.childCount; i++)
        {
            if(PlayerPrefs.GetInt("reachedlevel", 0) >= i)
            {
                mainlevelPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
            }
            else
            {
                mainlevelPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }
    }

    private void FixedUpdate()
    {
        transitionObject.GetComponent<RectTransform>().sizeDelta =
            new Vector2(Mathf.Clamp(transitionObject.GetComponent<RectTransform>().sizeDelta.x + (transitionSpeed * Time.fixedDeltaTime * (transitionObject.GetComponent<Image>().sprite.bounds.size.x / 100)), 0, maxScaleTransition * transitionObject.GetComponent<Image>().sprite.bounds.size.x),
            Mathf.Clamp(transitionObject.GetComponent<RectTransform>().sizeDelta.y + (transitionSpeed * Time.fixedDeltaTime * (transitionObject.GetComponent<Image>().sprite.bounds.size.y/100)), 0, maxScaleTransition * transitionObject.GetComponent<Image>().sprite.bounds.size.y));
        if(levelSelected)
        {
            levelTransitionObject.GetComponent<RectTransform>().sizeDelta =
            new Vector2(Mathf.Clamp(levelTransitionObject.GetComponent<RectTransform>().sizeDelta.x + (Mathf.Abs(transitionSpeed) * Time.fixedDeltaTime * (levelTransitionObject.GetComponent<Image>().sprite.bounds.size.x / 100)), 0, maxScaleTransition * levelTransitionObject.GetComponent<Image>().sprite.bounds.size.x),
            Mathf.Clamp(levelTransitionObject.GetComponent<RectTransform>().sizeDelta.y + (Mathf.Abs(transitionSpeed) * Time.fixedDeltaTime * (levelTransitionObject.GetComponent<Image>().sprite.bounds.size.y / 100)), 0, maxScaleTransition * levelTransitionObject.GetComponent<Image>().sprite.bounds.size.y));
            if(levelTransitionObject.GetComponent<RectTransform>().sizeDelta.x + 1 >= maxScaleTransition * levelTransitionObject.GetComponent<Image>().sprite.bounds.size.x)
            {
                LoadScene(1);
            }
        }
    }

    public void LoadScene(int whichScene)
    {
        SceneManager.LoadScene(whichScene);
    }

    public void OpenUrl(string URL)
    {
        Application.OpenURL(URL);

    }

    public void Transition()
    {
        if(!levelSelected)
        {
            transitionSpeed = -transitionSpeed;
        }
    }
    public void OpenLevel(int level)
    {
        if(!levelSelected)
        {
            PlayerPrefs.SetInt("currentlevel", level);
            levelSelected = true;
        }
    }

    public void AddPixies()
    {
        for(int i = 0; i<10; i++)
        {
            Instantiate(pixie, new Vector3(Random.Range(-10, 10), Random.Range(-4f, 4f), 0), Quaternion.identity,pixiesMain.transform);
        }
    }

    public void ClearPixies()
    {
        for(int i = 0; i<pixiesMain.transform.childCount; i++)
        {
            Destroy(pixiesMain.transform.GetChild(i).gameObject);
        }
    }
}
