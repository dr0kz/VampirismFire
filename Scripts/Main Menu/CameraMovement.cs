using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject title;
    public void MoveCameraTowardShop()
    {
        animator.SetTrigger("SHOP");
    }
    public void MoveCameraTowardMainMenu()
    {
        animator.SetTrigger("BACK");
    }
    public void ShowTitle()
    {
        title.SetActive(true);
    }
    public void HideTitle()
    {
        title.SetActive(false);
    }
    public void HideShop()
    {
        canvas.gameObject.SetActive(false);
    }
    public void ShowShop()
    {
        canvas.gameObject.SetActive(true);
    }
}
