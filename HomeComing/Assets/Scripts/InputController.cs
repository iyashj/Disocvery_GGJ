using Gamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public bool bDisableController;
    public LevelManager levelManager;
    public CameraController cameraController;

    [SerializeField]
    KeyCode keycodeToIndicateNorth;
    [SerializeField]
    KeyCode keycodeToIndicateSouth;
    [SerializeField]
    KeyCode keycodeToIndicateWest;
    [SerializeField]
    KeyCode keycodeToIndicateEast;

    private void Update()
    {

        if (!GAMECONSTANTS.ROUND_WON)
        {
            if (cameraController.bTweening)
            {
                return;
            }
            else
            {
                if (Input.GetKeyDown(keycodeToIndicateNorth))
                {
                    levelManager.DirectTo(Gamespace.EDirection.North);
                    //cameraController.PanCamera(EPanningDirection.UP);
                }

                else if (Input.GetKeyDown(keycodeToIndicateSouth))
                {
                    levelManager.DirectTo(Gamespace.EDirection.South);
                    //cameraController.PanCamera(EPanningDirection.DOWN);
                }

                else if (Input.GetKeyDown(keycodeToIndicateWest))
                {
                    levelManager.DirectTo(Gamespace.EDirection.West);
                    //cameraController.PanCamera(EPanningDirection.LEFT
                    //    );
                }

                else if (Input.GetKeyDown(keycodeToIndicateEast))
                {
                    levelManager.DirectTo(Gamespace.EDirection.East);
                    //cameraController.PanCamera(EPanningDirection.RIGHT);
                }
            }
        }

        
        
    }


}
