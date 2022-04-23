using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileSelectButton : MonoBehaviour
{
    [SerializeField] ProfileSelectorMenu.Option id;

    public ProfileSelectorMenu.Option Id => id;
}
