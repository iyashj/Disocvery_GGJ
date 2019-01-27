using Gamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid_InputController : MonoBehaviour
{
    [SerializeField]
    grid_LevelManager gridLevelManager;

    [SerializeField]
    KeyCode keycodeToIndicateUP;
    [SerializeField]
    KeyCode keycodeToIndicateDOWN;
    [SerializeField]
    KeyCode keycodeToIndicateLEFT;
    [SerializeField]
    KeyCode keycodeToIndicateRIGHT;

    private void Update()
    {
        if (!GAMECONSTANTS.ROUND_WON && !gridLevelManager.btweening)
        {
            if (Input.GetKeyDown(keycodeToIndicateUP))
            {
                gridLevelManager.MoveTowards(Gamespace.EDirection.North);
                //cameraController.PanCamera(EPanningDirection.UP);
            }

            else if (Input.GetKeyDown(keycodeToIndicateDOWN))
            {
                gridLevelManager.MoveTowards(Gamespace.EDirection.South);
                //cameraController.PanCamera(EPanningDirection.DOWN);
            }

            else if (Input.GetKeyDown(keycodeToIndicateLEFT))
            {
                gridLevelManager.MoveTowards(Gamespace.EDirection.West);
                //cameraController.PanCamera(EPanningDirection.LEFT
                //    );
            }

            else if (Input.GetKeyDown(keycodeToIndicateRIGHT))
            {
                gridLevelManager.MoveTowards(Gamespace.EDirection.East);
                //cameraController.PanCamera(EPanningDirection.RIGHT);
            }
        }

        
    }

}
