using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScript : MonoBehaviour
{
    // Start is called before the first frame update
    private bool IsActive = true;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsActive)
        {
            IsActive = false;
            //Debug.Log("Game Over");
        }

    }



}
