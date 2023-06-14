using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public float starGroup1Parallax = 0.8f;
    public float starGroup2Parallax = 0.73f;
    public float cloudGroupParallax = 0.95f;
    
    // public GameObject cloudGroup;
    public Transform starGroup1;
    private Vector3 _starGroup1StartPos;
    private Vector2 _sizeOfStarGroup1;
    
    public Transform starGroup2;
    private Vector3 _starGroup2StartPos;
    private Vector2 _sizeOfStarGroup2;

    public Transform cloudGroup;
    private Vector3 _cloudGroupStartPos;
    private Vector2 _sizeOfCloudGroup;


    void Awake()
    {
        _starGroup1StartPos = starGroup1.transform.position;
        _starGroup2StartPos = starGroup2.transform.position;
        
        _sizeOfStarGroup1 =  GetComponentInChildren<SpriteRenderer>().bounds.size;
        _sizeOfStarGroup2 =  GetComponentInChildren<SpriteRenderer>().bounds.size;
    }
    
    private void Update()
    {
        Vector3 Position = Camera.main.transform.position;
        Vector2 Temp = new Vector2(Position.x * (1 - starGroup1Parallax), Position.y * (1 - starGroup1Parallax));
        Vector2 Distance = new Vector2(Position.x, Position.y) * starGroup1Parallax;

        Vector3 NewPosition = _starGroup1StartPos + (Vector3)Distance;

        starGroup1.position = NewPosition;
        
        //StarGroup 2
        Vector2 Temp2 = new Vector2(Position.x * (1 - starGroup2Parallax), Position.y * (1 - starGroup2Parallax));
        Vector2 Distance2 = new Vector2(Position.x, Position.y) * starGroup2Parallax;
        
        Vector3 NewPosition2 = _starGroup2StartPos + (Vector3)Distance2;
        
        starGroup2.position = NewPosition2;
        
        //CloudGroup
        Vector2 Temp3 = new Vector2(Position.x * (1 - cloudGroupParallax), Position.y * (1 - cloudGroupParallax));
        Vector2 Distance3 = new Vector2(Position.x, Position.y) * cloudGroupParallax;
        
        Vector3 NewPosition3 = _cloudGroupStartPos + (Vector3)Distance3;
        
        cloudGroup.position = NewPosition3;
    
        // if (Temp.magnitude > ((Vector2)_starGroup1StartPos + (_sizeOfStarGroup1 / 2)).magnitude)
        // {
        //     _starGroup1StartPos += (Vector3)_sizeOfStarGroup1;
        // }
        // if (Temp.magnitude > ((Vector2)_starGroup1StartPos - (_sizeOfStarGroup1 / 2)).magnitude)
        // {
        //     _starGroup1StartPos -= (Vector3)_sizeOfStarGroup1;
        // }
    }
}
