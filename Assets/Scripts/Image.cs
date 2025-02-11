using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    public Sprite changimage;
    private UnityEngine.UI.Image thisImage;

    void Start()
    {
        thisImage = GetComponent<UnityEngine.UI.Image>();
    }

    public void ChangeImage()
    {
        if (thisImage != null && changimage != null)
        {
            thisImage.sprite = changimage;
        }
    }
}
