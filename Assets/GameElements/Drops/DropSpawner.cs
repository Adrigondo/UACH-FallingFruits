using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stariluz.GameLoop;
using UnityEngine;
using Random = UnityEngine.Random;

public class DropSpawner : MonoBehaviour, IGameElement
{
    public DropController[] ediblesPrefabs;
    public DropController[] nastysPrefabs;
    public float spawnInterval = 2f;
    public float screenLimits = 1f;
    private Coroutine currentCoroutine;
    private float totalEdiblesProbability = 1f;
    private float totalNastysProbability = 1f;
    private List<DropController> currentDrops = new();
    // Start is called before the first frame update
    public void StartGamePlay()
    {
        currentCoroutine = StartCoroutine(SpawnObject());
        totalEdiblesProbability = CalcTotalDropsProbability(ediblesPrefabs);
        totalNastysProbability = CalcTotalDropsProbability(nastysPrefabs);
    }
    public void Stop()
    {
        StopCoroutine(currentCoroutine);
    }
    private IEnumerator SpawnObject()
    {
        while (true)
        {
            // Calculate a random X position within the defined limits
            float randomX = Random.Range(-screenLimits, screenLimits);
            DropController dropToSpawn = ChooseRandomDrop();

            // Instantiate the prefab at the calculated position
            RegisterDrop(Instantiate(dropToSpawn, new Vector3(randomX, gameObject.transform.position.y, 0), Quaternion.identity));


            // Wait for the specified interval before spawning the next object
            yield return new WaitForSeconds(spawnInterval);
            spawnInterval -= 0.1f;
            spawnInterval = Math.Max(1f, spawnInterval);
        }
    }

    DropController ChooseRandomDrop()
    {
        DropController chosenPrefab = null;
        string randomDropType = Random.Range(0f, 1f) > 0.4 ? "edible" : "nasty";
        switch (randomDropType)
        {
            case "edible":
                chosenPrefab = GetRandomDrop(ediblesPrefabs, totalEdiblesProbability);
                break;
            case "nasty":
                chosenPrefab = GetRandomDrop(nastysPrefabs, totalNastysProbability);
                break;
        }
        return chosenPrefab;
    }

    DropController GetRandomDrop(DropController[] drops, float totalProbability)
    {
        float currentRange = 0f;
        DropController randomDrop = drops[^1];
        float randomProbability = Random.Range(0f, totalProbability);
        for (int i = 1; i < drops.Length; i++)
        {
            if (randomProbability < currentRange)
            {
                randomDrop = drops[i];
                break;
            }
            else
            {
                currentRange += drops[i].probability;
            }
        }
        return randomDrop;
    }

    float CalcTotalDropsProbability(DropController[] drops)
    {
        float probability = 0f;
        foreach (var drop in drops)
        {
            probability += drop.probability;
        }
        return probability;
    }
    public void RegisterDrop(DropController drop)
    {
        currentDrops.Append(drop);
    }
    public void UnregisterDrop(DropController drop)
    {
        drop.GetComponentInParent<DropController>().Destroy();
        currentDrops.Remove(drop);
    }
    public void Pause()
    {
        currentDrops.ForEach((drop)=>{
            drop.Pause();
        });
    }

    public void Resume()
    {
        currentDrops.ForEach((drop)=>{
            drop.Resume();
        });
    }

    public void StopGamePlay()
    {
        throw new NotImplementedException();
    }
}
