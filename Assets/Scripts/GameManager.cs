using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Assets.Scripts
{
	public class GameManager : MonoBehaviour
	{

		public List<Card> deck;
		public TextMeshProUGUI deckSizeText;

		public Transform[] cardSlots;
		public bool[] availableCardSlots;

		public List<Card> discardPile;
		public TextMeshProUGUI discardPileSizeText;

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
				for (int i = 0; i < availableCardSlots.Length; i++)
				{
					if (availableCardSlots[i] == true)
					{
						randomCard.gameObject.SetActive(true);
						randomCard.handIndex = i;
						randomCard.transform.position = cardSlots[i].position;
						randomCard.hasBeenPlayed = false;
						deck.Remove(randomCard);
						availableCardSlots[i] = false;
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

		private void Update()
		{

		}

	}
}