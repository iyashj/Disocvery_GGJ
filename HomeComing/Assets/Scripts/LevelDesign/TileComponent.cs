using Gamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TileComponent : MonoBehaviour
{
    public LevelDesigner levelDesigner;

    // intrinsic index of tile
    public int tileIndex;

    public TileComponent northTileComponent;
    public TileComponent eastTileComponent;
    public TileComponent westTileComponent;
    public TileComponent southTileComponent;

    public Image northBlocker;
    public Image southBlocker;
    public Image westBlocker;
    public Image eastBlocker;

    public Image specialPoint;

    private void Start()
    {
        northBlocker.gameObject.AddComponent<Button>();
        UnityEngine.UI.Button btnNorth = northBlocker.GetComponent<UnityEngine.UI.Button>();
        btnNorth.onClick.AddListener(() =>

        {
            ToggleActivation(northBlocker);
            ToggleActivation(northTileComponent.southBlocker);
        }
        
        );

        southBlocker.gameObject.AddComponent<Button>();
        UnityEngine.UI.Button btnSouth = southBlocker.GetComponent<UnityEngine.UI.Button>();
        btnSouth.onClick.AddListener(() =>

        {
            ToggleActivation(southBlocker);
            ToggleActivation(southTileComponent.northBlocker);
        }

        );

        westBlocker.gameObject.AddComponent<Button>();
        UnityEngine.UI.Button btnWest = westBlocker.GetComponent<UnityEngine.UI.Button>();
        btnWest.onClick.AddListener(() => {

            ToggleActivation(westBlocker);
            ToggleActivation(westTileComponent.eastBlocker);
        }

        );

        eastBlocker.gameObject.AddComponent<Button>();
        UnityEngine.UI.Button btnEast = eastBlocker.GetComponent<UnityEngine.UI.Button>();
        btnEast.onClick.AddListener(() => {

            ToggleActivation(eastBlocker);
            ToggleActivation(eastTileComponent.westBlocker);
        }

        );

        specialPoint.gameObject.AddComponent<Button>();
        UnityEngine.UI.Button btnSpecialPt = specialPoint.GetComponent<UnityEngine.UI.Button>();
        btnSpecialPt.onClick.AddListener(() => 
        {
            Sprite _specialSprite = levelDesigner.PlaceSpecialSprite(tileIndex);
            
            if (_specialSprite == null)
            {
                // do nothing
            }
            else
            {
                specialPoint.sprite = _specialSprite;
            }
        });
    }



    void ToggleActivation(Image _toggleImageColor)
    {
        if(_toggleImageColor.color.a == 0)
        {
            _toggleImageColor.color = new Color(_toggleImageColor.color.r, _toggleImageColor.color.g, _toggleImageColor.color.b, 1);
        }
        else
        {
            _toggleImageColor.color = new Color(_toggleImageColor.color.r, _toggleImageColor.color.g, _toggleImageColor.color.b, 0);
        }
    }

    public RConnectionData GetConnectorData()
    {
        int _northIndex = 0;
        int _southIndex = 0;
        int _eastIndex = 0;
        int _westIndex = 0;

        if (northBlocker.IsActive())
        {
            _northIndex = northTileComponent.tileIndex;
        }

        if (southBlocker.IsActive())
        {
            _southIndex = southTileComponent.tileIndex;
        }

        if (eastBlocker.IsActive())
        {
            _eastIndex = eastTileComponent.tileIndex;
        }

        if (westBlocker.IsActive())
        {
            _westIndex = westTileComponent.tileIndex;
        }

        return new RConnectionData(_northIndex, _southIndex, _eastIndex, _westIndex);
    }
}
