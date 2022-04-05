using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] protected Sprite[] spriteVariants;

    protected SpriteRenderer spriteRenderer;
    

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteVariants.Length > 0)
            spriteRenderer.sprite = spriteVariants[UnityEngine.Random.Range(0, spriteVariants.Length)];
    }


}
