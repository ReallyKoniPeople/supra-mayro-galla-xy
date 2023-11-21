using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public int targetFrameRate = 60;
    public bool disableCursor = false;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        if(disableCursor){
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
