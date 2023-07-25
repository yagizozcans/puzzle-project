using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectedImageData : MonoBehaviour
{
    public Vector3 selectedAnchoredPosition;

    public void Awake()
    {
        selectedAnchoredPosition = transform.GetComponent<RectTransform>().anchoredPosition;
        Debug.Log(selectedAnchoredPosition);
    }
}
