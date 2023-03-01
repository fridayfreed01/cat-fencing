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
		battleSystem = GetComponent<BattleSystem>();
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
				break;

			case "Pounce":
				Debug.Log(cardName);
				selectedCard = BasicCardType.POUNCE;
				break;

			case "Sneak":
				Debug.Log(cardName);
				selectedCard = BasicCardType.SNEAK;
				break;

			case "Parry":
				Debug.Log(cardName);
				selectedCard = BasicCardType.PARRY;
				break;

			case "Feint":
				Debug.Log(cardName);
				selectedCard = BasicCardType.FEINT;
				break;
		}
		Debug.Log(selectedCard);
	}

}
