using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts
{
	public class Card : MonoBehaviour
	{
		//determines if specific card has been played already
		public bool hasBeenPlayed;
		public int handIndex;

		//if card is special, it does not advance the turn on play
		public bool isSpecial;
		public string cardName;
		public int damage;
		public bool isEnemyCard;
		GameManager gm;
		BattleSystem battleSystem;

		private void Start()
		{
			gm = FindObjectOfType<GameManager>();
			battleSystem = FindObjectOfType<BattleSystem>();
		}
		private void OnMouseDown()
		{
			if(battleSystem.state != BattleState.PLAYER || hasBeenPlayed)
			{
				return;
			}
			transform.position = gm.playerActiveCard.position;
			hasBeenPlayed = true;
			gm.availableCardSlots[handIndex] = true;
			battleSystem.playerCard = this;
            GetChoice();
			//trigger OnPlayCard() which will then change the state to enemyturn
            battleSystem.OnPlayCard();
		}
		
		public void EnemyPlay(BasicCardType card)
		{
			if(battleSystem.state != BattleState.BATTLE || !isEnemyCard)
			{
				return;
			}

			transform.position = gm.enemyActiveCard.position;
			gameObject.SetActive(true);
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
}