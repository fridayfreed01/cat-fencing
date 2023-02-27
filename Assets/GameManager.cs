using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //for later, will need deck
    public List<Card> deck = new List<Card>();
    public Transform[] slots;
    public bool[] openSlots;

    public void DrawCard()
    {
        if (deck.Count >= 1)
        {
            Card randomCard = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < openSlots.Length; i++)
            {
                if (openSlots[i])
                {
                    randomCard.gameObject.SetActive(true);
                    randomCard.transform.position = slots[i].position;
                    openSlots[i] = false;
                    deck.Remove(randomCard);
                    return;
                }
            }
        }
    }
    public void Start()
    {
       
    }

    private void Update()
    {
        
    }
}
