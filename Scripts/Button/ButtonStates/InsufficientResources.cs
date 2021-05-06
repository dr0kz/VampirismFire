using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class InsufficientResources : MonoBehaviour, IButtonState
{
    private CanvasManager CANVAS_MANAGER;
    private SoundManager SOUND_MANAGER;
    private MyButton objectPrefab;
    public InsufficientResources()
    {
        SOUND_MANAGER = SoundManager.SOUND_MANAGER;
        CANVAS_MANAGER = CanvasManager.CANVAS_MANAGER;
    }
    public bool Buy()
    {
        CANVAS_MANAGER.DisplayAlertMessageToUser("Insufficient resources!");
        if (!SOUND_MANAGER.GetButtonsAudioSource().isPlaying)
        {
            if (objectPrefab.GetResource().Equals("wood"))
            {
                SOUND_MANAGER.PlayButtonsSound(objectPrefab.GetMoreLumberRequiredSoundEffect(), 0.15f);
            }
            else
            {
                SOUND_MANAGER.PlayButtonsSound(objectPrefab.GetMoreGoldRequiredSoundEffect(), 0.15f);
            }
        }
        return false;
    }
    public void executeAction(MyButton objectPrefab)
    {
        this.objectPrefab = objectPrefab;
        objectPrefab.GetComponent<Image>().sprite = objectPrefab.GetInsufficientResourcesButtonLook();

    }
}
