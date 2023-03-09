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

    public int lungeDmg = 2;
    public int pounceDmg = 3;
    public int sneakDmg = 4;
    public int parryDmg = 2;
    public int feintDmg = 3;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BasicCardType playerChoice;
    public BasicCardType enemyChoice;

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
        OnCardPlay();
    }

    IEnumerator CardInteraction(BasicCardType playerCard, BasicCardType enemyCard)
    {
        Debug.Log("Card interaction starting");
        //Compare the cards
        int damage = 0;
        //Damage calculation
        
        if(playerCard == BasicCardType.LUNGE)
        {
            damage = lungeDmg;
        } 
        else if(playerCard == BasicCardType.POUNCE)
        {
            damage = pounceDmg;
        }
        else if(playerCard == BasicCardType.SNEAK)
        {
            damage = sneakDmg;
        }
        else if(playerCard == BasicCardType.FEINT)
        {
            damage = feintDmg;
        }
        else if(playerCard == BasicCardType.PARRY)
        {
            damage = feintDmg;
        }
        enemyUnit.TakeDamage(damage);
        enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit);
        //Check if player or enemy is dead

        //Change state
  
        //change later
        yield return new WaitForSeconds(2f);
    }

    public void OnCardPlay()
    {
        Debug.Log("Card is being played");
        if(state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(CardInteraction(playerChoice, enemyChoice));
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
