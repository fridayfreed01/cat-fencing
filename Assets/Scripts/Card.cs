using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
	public bool hasBeenPlayed;
	public int handIndex;

	GameManager gm;

	public GameObject effect;
	public GameObject hollowCircle;

	private void Start()
	{
		gm = FindObjectOfType<GameManager>();
	}
	private void OnMouseDown()
	{
		if (!hasBeenPlayed)
		{
			//Instantiate(hollowCircle, transform.position, Quaternion.identity);
			transform.position = gm.playerActiveCard.position;
			hasBeenPlayed = true;
			gm.availableCardSlots[handIndex] = true;

			
		}
	}

	//void MoveToDiscardPile()
	//{
	//	Instantiate(effect, transform.position, Quaternion.identity);
	//	gm.discardPile.Add(this);
	//	gameObject.SetActive(false);
	//}



}
