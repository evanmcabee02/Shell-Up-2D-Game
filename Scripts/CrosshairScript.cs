using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
    private Vector3 mousePosition;
    private float speed = 1.0f;

    void Start()
    {
        //Set Cursor to not be visible
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        mousePosition = (Input.mousePosition);
        mousePosition.z = speed;
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        //Debug.Log("X Position" + Camera.main.ScreenToWorldPoint(mousePosition));
    }
    public Vector3 GetMousePosition()
    {
        return mousePosition;
    }
}
