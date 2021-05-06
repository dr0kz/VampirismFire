using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private AudioClip buttonSound;
    private SoundManager SOUND_MANAGER;
    private void Start()
    {
        Application.targetFrameRate = 60;
        SOUND_MANAGER = SoundManager.SOUND_MANAGER;
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Shop()
    {
        camera.GetComponent<CameraMovement>().MoveCameraTowardShop();
        SOUND_MANAGER.PlaySound(buttonSound, 0.3f);
    }
    public void BackToMenu()
    {
        camera.GetComponent<CameraMovement>().MoveCameraTowardMainMenu();
        SOUND_MANAGER.PlaySound(buttonSound, 0.3f);
    }
    public void delete()
    {
        Application.Quit();
        //Debug.Log("DELETED");
        //SaveSystem.DeleteData();
    }
}
