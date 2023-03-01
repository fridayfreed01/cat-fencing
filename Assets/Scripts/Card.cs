using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
	public bool hasBeenPlayed;
	public int handIndex;

	//if card is special, it does not advance the turn on play
	public bool isSpecial;
	public string cardName;

	GameManager gm;
	BattleSystem battleSystem;

	private void Start()
	{
		gm = FindObjectOfType<GameManager>();
		battleSystem = FindObjectOfType<BattleSystem>();
	}
	private void OnMouseDown()
	{
		if (!hasBeenPlayed)
		{
			transform.position = gm.playerActiveCard.position;
			hasBeenPlayed = true;
			gm.availableCardSlots[handIndex] = true;
			GetChoice();
		}
	}

	//void MoveToDiscardPile()
	//{
	//	Instantiate(effect, transform.position, Quaternion.identity);
	//	gm.discardPile.Add(this);
	//	gameObject.SetActive(false);
	//}

	public void GetChoice()
	{
		BasicCardType selectedCard = BasicCardType.NONE;
		
		switch (cardName)
		{
			case "Lunge":
				Debug.Log(cardName);
				selectedCard = BasicCardType.LUNGE;
				battleSystem.playerChoice = selectedCard;
				break;

			case "Pounce":
				Debug.Log(cardName);
				selectedCard = BasicCardType.POUNCE;
				battleSystem.playerChoice = selectedCard;
				break;

			case "Sneak":
				Debug.Log(cardName);
				selectedCard = BasicCardType.SNEAK;
				battleSystem.playerChoice = selectedCard;
				break;

			case "Parry":
				Debug.Log(cardName);
				selectedCard = BasicCardType.PARRY;
				battleSystem.playerChoice = selectedCard;
				break;

			case "Feint":
				Debug.Log(cardName);
				selectedCard = BasicCardType.FEINT;
				battleSystem.playerChoice = selectedCard;
				break;
		}
		Debug.Log(selectedCard);
	}

}
