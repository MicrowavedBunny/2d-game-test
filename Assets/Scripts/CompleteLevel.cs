using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteLevel : MonoBehaviour
{
    public Animator end;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        end.SetBool("hasFinished", true);
    }
}
