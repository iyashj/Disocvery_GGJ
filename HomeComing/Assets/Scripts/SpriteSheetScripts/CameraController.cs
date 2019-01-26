using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    KeyCode UpKey;
    [SerializeField]
    KeyCode DownKey;
    [SerializeField]
    KeyCode LeftKey;
    [SerializeField]
    KeyCode RightKey;

    [SerializeField]
    float panningXDistance;
    [SerializeField]
    float panningYDistance;

    public Sprite baseSprite;

    public int cellCount;
    public Camera orthoCamera;
    public Vector2 spriteAspectRatio;

    public void CalculateCameraOrthoFrameSize()
    {
        Vector2 spriteSizeRect = new Vector2(baseSprite.rect.width, baseSprite.rect.height);
        
    }

    [SerializeField]

    public bool bTweening;
    [SerializeField]
    private EPanningDirection panningDirection;
    [SerializeField]
    private float panningDuration;
    private void Start()
    {
        //panningXDistance = orthoCamera.pixelWidth;
        //panningYDistance = orthoCamera.pixelHeight;




    }

   private void CallTween(EPanningDirection _panningDirection)
    {
       
    }

    //private void Update()
    //{
    //    if (bTweening)
    //    {
    //        return;
    //    }
    //    else
    //    {
    //        if (Input.GetKeyDown(UpKey))
    //        {
    //            PanCamera(EPanningDirection.UP);
    //        }
    //        else if (Input.GetKeyDown(DownKey))
    //        {
    //            PanCamera(EPanningDirection.DOWN);

    //        }
    //        else if (Input.GetKeyDown(LeftKey))
    //        {
    //            PanCamera(EPanningDirection.LEFT);

    //        }
    //        else if (Input.GetKeyDown(RightKey))
    //        {
    //            PanCamera(EPanningDirection.RIGHT);
    //        }
    //    }
    //}

    void tweenCompleted()
    {
        bTweening = false;
    }
    

    public void PanCamera(EPanningDirection _panningDirection)
    {
        

        if (!bTweening)
        {
            Hashtable hashtable = new Hashtable();

            Vector3 targetPos = orthoCamera.gameObject.transform.position;


            switch (_panningDirection)
            {
                case EPanningDirection.NONE:
                    break;
                case EPanningDirection.UP:
                    targetPos = new Vector3(targetPos.x, targetPos.y + panningYDistance, targetPos.z);
                    break;
                case EPanningDirection.DOWN:
                    targetPos = new Vector3(targetPos.x, targetPos.y - panningYDistance, targetPos.z);
                    break;
                case EPanningDirection.LEFT:
                    targetPos = new Vector3(targetPos.x - panningXDistance, targetPos.y, targetPos.z);
                    break;
                case EPanningDirection.RIGHT:
                    targetPos = new Vector3(targetPos.x + panningXDistance, targetPos.y, targetPos.z);
                    break;
                case EPanningDirection.MAX:
                    break;
                default:
                    break;
            }

            hashtable.Add("position", targetPos);
            hashtable.Add("islocal", false);
            hashtable.Add("time", panningDuration);
            hashtable.Add("oncomplete", "tweenCompleted");
            hashtable.Add("oncompletetarget", orthoCamera.gameObject);
            hashtable.Add("easetype", iTween.EaseType.linear);
            iTween.MoveTo(orthoCamera.gameObject, hashtable);
            bTweening = true;
            return;
        }
    }


   

}
public enum EPanningDirection
{
    NONE,
    UP,
    DOWN,
    LEFT,
    RIGHT,
    MAX,
}