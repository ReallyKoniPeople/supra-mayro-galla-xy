using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LiveCounter : MonoBehaviour
{
    public TMP_Text livesString;
    // Start is called before the first frame update
    void Start()
    {
        livesString.text = PlayerStats.Lives.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
