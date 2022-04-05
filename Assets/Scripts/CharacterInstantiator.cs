using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInstantiator : MonoBehaviour
{
    [SerializeField] Character _characterPrefab;

    private void Awake()
    {
    }


    public void SpawnCharacter(string id, Controller controller, int playerNumber)
    {
        Debug.Log($"Spawning: {id}");

        var character = Instantiate(_characterPrefab, Vector3.zero, Quaternion.identity);
        character.SetController(controller);
        character.SetPlayerNumber(playerNumber);
        character.gameObject.name += $" {playerNumber}"; 
    }
}
