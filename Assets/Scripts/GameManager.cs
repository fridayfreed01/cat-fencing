using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

namespace Assets.Scripts
{
	public class GameManager : MonoBehaviour
	{

		public List<Card> deck;

		public Transform[] cardSlots;
		public bool[] availableCardSlots;

		public List<Card> discardPile;

		public Transform playerActiveCard;
		public Transform enemyActiveCard;

		private void Start()
		{
            for (int i = 0; i < cardSlots.Length; i++)
			{
				DrawCard();
			}
		}

		public void DrawCard()
		{
            if (deck.Count >= 1)
            {
                Card randomCard = deck[Random.Range(0, deck.Count)];
				Debug.Log(randomCard.cardName);
                for (int j = 0; j < availableCardSlots.Length; j++)
                {
					if (availableCardSlots[j] == true)
                    {
                        randomCard.gameObject.SetActive(true);
                        randomCard.handIndex = j;
                        randomCard.transform.position = cardSlots[j].position;
                        randomCard.hasBeenPlayed = false;
                        deck.Remove(randomCard);
                        availableCardSlots[j] = false;
                        return;
                    }
                }
            }
        }

		public void Shuffle()
		{
			if (discardPile.Count >= 1)
			{
				foreach (Card card in discardPile)
				{
					deck.Add(card);
				}
				discardPile.Clear();
			}
		}


	}
}