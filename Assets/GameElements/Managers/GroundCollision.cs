using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollision : MonoBehaviour
{

    public delegate void DropEvent(DropController collider2D);
    public DropEvent OnCollisionWithEdibleDrop;
    public DropEvent OnCollisionWithNastyDrop;
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Edible"))
        {
            OnCollisionWithEdibleDrop?.Invoke(collider.GetComponentInParent<DropController>());
        }
        else if (collider.CompareTag("Nasty"))
        {
            OnCollisionWithNastyDrop?.Invoke(collider.GetComponentInParent<DropController>());
        }
    }
}
