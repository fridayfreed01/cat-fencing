using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOSE }
public enum BasicCardType { NONE, LUNGE, POUNCE, SNEAK, PARRY, FEINT }
public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public GameObject playerPf;
    public GameObject enemyPf;

    public Transform playerSpawn;
    public Transform enemySpawn;

    Unit playerUnit;
    Unit enemyUnit;

    public Card playerCard;
    public Card enemyCard;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    private BasicCardType playerChoice = BasicCardType.NONE;
    private BasicCardType enemyChoice = BasicCardType.NONE;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    //set up needed information/transforms for the game
    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPf, playerSpawn);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPf, enemySpawn);
        enemyUnit = enemyGO.GetComponent<Unit>();

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        
    }

    IEnumerator CardInteraction(Card playerCard, Card enemyCard)
    {
        //Compare the cards

        //Damage calculation

        //Check if player or enemy is dead

        //Change state
  
        yield return new WaitForSeconds(2f);
    }

    public void OnCardPlay()
    {
        if(state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(CardInteraction(playerCard, enemyCard));
    }

    public void SetChoice(BasicCardType choice)
    {
        switch (choice)
        {
            case BasicCardType.LUNGE:
                playerChoice = BasicCardType.LUNGE;
                break;

            case BasicCardType.POUNCE:
                playerChoice = BasicCardType.POUNCE;

                break;

            case BasicCardType.SNEAK:
                playerChoice = BasicCardType.SNEAK;
                break;

            case BasicCardType.PARRY:
                playerChoice = BasicCardType.PARRY;
                break;

            case BasicCardType.FEINT:
                playerChoice = BasicCardType.FEINT;
                break;
        }
    }

}
