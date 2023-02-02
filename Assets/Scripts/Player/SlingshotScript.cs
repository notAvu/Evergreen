using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotScript : MonoBehaviour
{
    [HideInInspector]
    public bool touchingGrass;

    private void OnTriggerStay2D(Collider2D collision)
    {
        touchingGrass = collision.CompareTag("Ground");
    }
}
