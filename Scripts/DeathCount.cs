using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCount : MonoBehaviour
{
    public Text countText;
    int deaths;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShowDeaths();
    }

    private void ShowDeaths() 
    {
        countText.text = deaths.ToString();
    }

    public void addDeaths() 
    {
        deaths++;
    }
}
