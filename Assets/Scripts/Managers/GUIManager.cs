using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * <summary>
 * 	Klasa obsługująca GUI
 * </summary>
 * <remarks>
 * 	Zarząda wyświetlanymi na ekranie informacjami o stanie gry
 * </remarks>
 */
public class GUIManager : MonoBehaviour 
{
    [SerializeField]
    private HorizontalLayoutGroup _heartsContainer;
    [SerializeField]
    private HorizontalLayoutGroup _itemsContainer;

    [SerializeField]
    private Image _heart1Full;
    [SerializeField]
    private Image _heart1Empty;
    [SerializeField]
    private Image _heart2Full;
    [SerializeField]
    private Image _heart2Empty;
    [SerializeField]
    private Text _coins;
    [SerializeField]
    private Image _itemPrefab;

	void Start () 
    {
        Messenger.AddListener<int,int>(Messages.PlayerHealthChanged, UpdateHealth);
        Messenger.AddListener<int>(Messages.PlayerCoinsChanged, UpdateCoins);
	}

    void UpdateHealth(int health, int maxHealth)
    {
        for(int i = 0; i < _heartsContainer.transform.childCount; i++)
        {
            Destroy(_heartsContainer.transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < maxHealth; i++)
        {
            Image toAdd = null;
            if(i < health)
            {
                if (i % 2 == 0)
                    toAdd = _heart1Full;
                else
                    toAdd = _heart2Full;
            }
            else
            {
                if (i % 2 == 0)
                    toAdd = _heart1Empty;
                else
                    toAdd = _heart2Empty;
            }

            var newObject = Instantiate(toAdd);
            newObject.transform.SetParent(_heartsContainer.transform);
            newObject.transform.localScale = Vector3.one;
        }
    }

    void UpdateCoins(int coins)
    {
        _coins.text = coins.ToString();
    }

    void UpdateItems(Sprite itemSprite)
    {
        var newItem = Instantiate(_itemPrefab);
        newItem.sprite = itemSprite;
        newItem.transform.SetParent(_itemsContainer.transform);
        newItem.transform.localScale = Vector3.one;
    }
}
