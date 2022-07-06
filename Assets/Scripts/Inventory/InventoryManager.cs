using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private IngredientBlueprint testBlueprint;
    public static InventoryManager Instance { get; private set; }

    private Inventory inventory;
    private UIInventoryMenu inventoryMenu;



    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one InventoryManager in the scene. Destroying the newest one");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        inventory = GetComponentInChildren<Inventory>();
        inventoryMenu = GetComponentInChildren<UIInventoryMenu>();


        DontDestroyOnLoad(gameObject);

    }
    private void Start()
    {
        inventoryMenu.Initialize(inventory);
    }


    public void PickUpItem(Ingredient ingredient)
    {
        inventory.AddItem(ingredient);
    }

    [ContextMenu("Add item from test blueprint")]
    public void AddItemFromTestGenerator()
    {
        inventory.AddItem(new Ingredient(testBlueprint, 1));
    }

}
