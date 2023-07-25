using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float movingTime;
    public float borderDisappearTime;

    public GameObject mainImageObject;

    public PlayableImage[] playableImages;

    public List<int> images = new List<int>();

    public GameObject firstSelectedImage;
    public GameObject secondSelectedImage;

    int checkStatus;

    [HideInInspector] public bool checkStatusBool;

    [HideInInspector]public bool movingObject;

    public GameObject transitionObject;
    public float maxScaleTransition;
    public float transitionSpeed;

    public bool gameHasEndAndGoBack;

    public void Start()
    {
        images = Shuffle<int>(images);

        movingObject = false;

        checkStatusBool = false;

        gameHasEndAndGoBack = false;

        transitionObject.GetComponent<RectTransform>().sizeDelta = new Vector2(maxScaleTransition * transitionObject.GetComponent<Image>().sprite.bounds.size.x, maxScaleTransition * transitionObject.GetComponent<Image>().sprite.bounds.size.y);

        if(PlayerPrefs.GetInt("currentlevel", 0) < playableImages.Length)
        {
            for (int i = 0; i < mainImageObject.transform.childCount; i++)
            {
                mainImageObject.transform.GetChild(i).GetChild(0).GetComponent<Button>().image.sprite = playableImages[PlayerPrefs.GetInt("currentlevel", 0)].images[images[i]];
                mainImageObject.transform.GetChild(i).GetChild(0).GetComponent<imagebutton>().serieNumber = images[i];
                //mainImageObject.transform.GetChild(i).gameObject.name = images[i].ToString();
            }
        }
        else
        {
            int random = Random.Range(0, playableImages.Length);
            for (int i = 0; i < mainImageObject.transform.childCount; i++)
            {
                mainImageObject.transform.GetChild(i).GetChild(0).GetComponent<Button>().image.sprite = playableImages[random].images[images[i]];
                mainImageObject.transform.GetChild(i).GetChild(0).GetComponent<imagebutton>().serieNumber = images[i];
                //mainImageObject.transform.GetChild(i).gameObject.name = images[i].ToString();
            }
        }
    }

    [System.Serializable]
    public class PlayableImage
    {
        public Sprite[] images;
    }

    public List<T> Shuffle<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }

        return _list;
    }

    private void FixedUpdate()
    {
        if(!gameHasEndAndGoBack)
        {
            transitionObject.GetComponent<RectTransform>().sizeDelta =
            new Vector2(Mathf.Clamp(transitionObject.GetComponent<RectTransform>().sizeDelta.x - (transitionSpeed * Time.fixedDeltaTime * (transitionObject.GetComponent<Image>().sprite.bounds.size.x / 100)), 0, maxScaleTransition * transitionObject.GetComponent<Image>().sprite.bounds.size.x),
            Mathf.Clamp(transitionObject.GetComponent<RectTransform>().sizeDelta.y - (transitionSpeed * Time.fixedDeltaTime * (transitionObject.GetComponent<Image>().sprite.bounds.size.y / 100)), 0, maxScaleTransition * transitionObject.GetComponent<Image>().sprite.bounds.size.y));
        }else if(gameHasEndAndGoBack)
        {
            transitionObject.GetComponent<RectTransform>().sizeDelta =
            new Vector2(Mathf.Clamp(transitionObject.GetComponent<RectTransform>().sizeDelta.x + (transitionSpeed * Time.fixedDeltaTime * (transitionObject.GetComponent<Image>().sprite.bounds.size.x / 100)), 0, maxScaleTransition * transitionObject.GetComponent<Image>().sprite.bounds.size.x),
            Mathf.Clamp(transitionObject.GetComponent<RectTransform>().sizeDelta.y + (transitionSpeed * Time.fixedDeltaTime * (transitionObject.GetComponent<Image>().sprite.bounds.size.y / 100)), 0, maxScaleTransition * transitionObject.GetComponent<Image>().sprite.bounds.size.y));
            if (transitionObject.GetComponent<RectTransform>().sizeDelta.x + 1 >= maxScaleTransition * transitionObject.GetComponent<Image>().sprite.bounds.size.x)
            {
                SceneManager.LoadScene(0);
            }
        }
        
        for (int i = 0; i<mainImageObject.transform.childCount; i++)
        {
            if(int.Parse(mainImageObject.transform.GetChild(i).GetChild(0).name) == mainImageObject.transform.GetChild(i).GetChild(0).GetComponent<imagebutton>().serieNumber)
            {
                checkStatus++;
            }
            if (int.Parse(mainImageObject.transform.GetChild(i).GetChild(0).name) != mainImageObject.transform.GetChild(i).GetChild(0).GetComponent<imagebutton>().serieNumber)
            {
                checkStatus = 0;
            }
            if(checkStatus == mainImageObject.transform.childCount)
            {
                Debug.Log("done");
                checkStatusBool = true;
            }
        }
    }

    public void changeImages()
    {
        if(!checkStatusBool)
        {
            if (firstSelectedImage == null && !movingObject)
            {
                firstSelectedImage = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            }
            else if (firstSelectedImage != null && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != firstSelectedImage && !movingObject)
            {
                secondSelectedImage = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

                int firstSerie = int.Parse(firstSelectedImage.transform.name);
                int secondSerie = int.Parse(secondSelectedImage.transform.name);

                Vector3 firstPos = firstSelectedImage.transform.GetComponent<imagebutton>().mainPosition;
                Vector3 secondPos = secondSelectedImage.transform.GetComponent<imagebutton>().mainPosition;

                Debug.Log(firstPos);
                Debug.Log(secondPos);

                //firstSelectedImage.GetComponent<Button>().image.sprite = playableImages.images[secondSerie];
                firstSelectedImage.GetComponent<imagebutton>().mainPosition = secondPos;
                firstSelectedImage.gameObject.name = secondSerie.ToString();
                //secondSelectedImage.GetComponent<Button>().image.sprite = playableImages.images[firstSerie];
                secondSelectedImage.GetComponent<imagebutton>().mainPosition = firstPos;
                secondSelectedImage.gameObject.name = firstSerie.ToString();


                firstSelectedImage = null;
                secondSelectedImage = null;

                movingObject = true;
                Invoke("ChangingBool", movingTime);
            }
        }
    }

    public void ChangingBool()
    {
        movingObject = false;
    }

    public void BackToMainMenu()
    {
        gameHasEndAndGoBack = true;
    }
}