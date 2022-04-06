using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInstantiator : MonoBehaviour
{
    public static CharacterInstantiator Instance;
    //later, build out the character from a file
    [SerializeField] private Character characterPrefab;
    [SerializeField] private AimReticle aimReticlePrefab;

    private void Awake()
    {
        Instance = this;

        if (aimReticlePrefab == null || characterPrefab == null)
        {
            Debug.LogError("CharacterInstantiator Awake(): a prefab is unassigned!");
            return;
        }

    }


    public void SpawnCharacter(string id, Controller controller, int playerNumber, Vector3 spawnPoint)
    {
        Debug.Log($"Spawning: {id}");

        var character = Instantiate(characterPrefab, spawnPoint, Quaternion.identity);
        character.SetController(controller);
        character.SetPlayerNumber(playerNumber);
        character.gameObject.name = $"P{playerNumber} Character";


        var aimReticle = Instantiate(aimReticlePrefab);
        aimReticle.Initialize(controller, character.transform, playerNumber);
        aimReticle.gameObject.name = $"P{playerNumber} AimReticle";
    }
}
