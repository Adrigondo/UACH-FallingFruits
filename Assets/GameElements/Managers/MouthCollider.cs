using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthCollider : MonoBehaviour
{
    Collider2D colider2D;
    public LivesManager livesManager;
    public ScoreManager scoreManager;
    public DropSpawner dropSpawner;
    // Start is called before the first frame update
    void Start()
    {
        colider2D = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.tag);
        if(collider.CompareTag("Edible"))
        {
            // TODO: Logic when an edible is ate.
            scoreManager.AddScore(100);
            dropSpawner.UnregisterDrop(collider.GetComponentInParent<DropLogic>());
        }
        else if(collider.CompareTag("Nasty"))
        {
            // TODO: Logic when a nasty is ate.
            scoreManager.RestoreStreak();
            livesManager.LoseLives(1);
            dropSpawner.UnregisterDrop(collider.GetComponentInParent<DropLogic>());
        }
    }
}
