using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public enum BattleState { START, PLAYER, BATTLE, CLEANUP, WIN, LOSE }
    public enum BasicCardType { NONE, LUNGE, POUNCE, SNEAK, PARRY, FEINT }
    public enum Enemy { Peanut, Fluffy }

    public class BattleSystem : StateMachine
    {
        public BattleState state;
        public GameManager gameManager;
        public GameObject playerPf;
        public GameObject enemyPf;

        public Card playerCard;
        public Card enemyCard; 

        public Transform playerSpawn;
        public Transform enemySpawn;

        public Unit playerUnit;
        public Unit enemyUnit;

        public int lungeDmg = 2;
        public int pounceDmg = 3;
        public int sneakDmg = 4;
        public int parryDmg = 2;
        public int feintDmg = 3;

        public BattleHUD playerHUD;
        public BattleHUD enemyHUD;

        public BasicCardType playerChoice;
        public BasicCardType enemyChoice;

        public Card[] enemyCards;

        public TextMeshProUGUI text;


        // Start is called before the first frame update
        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            state = BattleState.START;
            StartCoroutine(SetupBattle());
        }

        private void Update()
        {
            StateText();
        }
        IEnumerator SetupBattle()
        {
            GameObject playerGO = Instantiate(playerPf, playerSpawn);
            playerUnit = playerGO.GetComponent<Unit>();
            playerHUD.SetHUD(playerUnit);

            GameObject enemyGO = Instantiate(enemyPf, enemySpawn);
            enemyUnit = enemyGO.GetComponent<Unit>();
            enemyHUD.SetHUD(enemyUnit);

            yield return new WaitForSeconds(2f);
            state = BattleState.PLAYER;
            PlayerTurn();
        }
        
        IEnumerator PlayCard()
        {
            //get values to compare to enemy's card on ENEMYTURN
            //use SetChoice to find out what card player chose on click
            yield return new WaitForSeconds(0);

            //change to ENEMYTURN after setting values
            state = BattleState.BATTLE;
            StartCoroutine(Compare());
        }

        //This function is not IEnumerator so the player can spend time on their turn
        void PlayerTurn()
        {
            Debug.Log("Player turn starting");
            //make sure that the game state changes when a card is played
            //maybe a script in card?
        }

        public void OnPlayCard()
        {
            if (state != BattleState.PLAYER)
            {
                return;
            }
            StartCoroutine(PlayCard());
        }

        public void EnemyTurn()
        {
            Debug.Log("Enemy turn starting");
            enemyChoice = GetEnemyChoice();
            switch (enemyChoice)
            {
                case (BasicCardType.LUNGE):
                    enemyCards[0].transform.position = gameManager.enemyActiveCard.position;
                    enemyCards[0].gameObject.SetActive(true);
                    break;
                case (BasicCardType.POUNCE):
                    enemyCards[1].transform.position = gameManager.enemyActiveCard.position;
                    enemyCards[1].gameObject.SetActive(true);
                    break;
                case (BasicCardType.PARRY):
                    enemyCards[2].transform.position = gameManager.enemyActiveCard.position;
                    enemyCards[2].gameObject.SetActive(true);
                    break;
                case (BasicCardType.SNEAK):
                    enemyCards[3].transform.position = gameManager.enemyActiveCard.position;
                    enemyCards[3].gameObject.SetActive(true);
                    break;
                case (BasicCardType.FEINT):
                    enemyCards[4].transform.position = gameManager.enemyActiveCard.position;
                    enemyCards[4].gameObject.SetActive(true);
                    break;
                case (BasicCardType.NONE):
                    break;
            }
        }

        IEnumerator Compare()
        {
            Debug.Log("Cards are comparing");
            EnemyTurn();
            CardInteraction(playerChoice, enemyChoice);

            yield return new WaitForSeconds(0);
            state = BattleState.CLEANUP;
            StartCoroutine(Cleanup());
        }

        IEnumerator Cleanup()
        {
            yield return new WaitForSeconds(3f);
            Debug.Log("Cleanup");
            for(int i = 0; i < enemyCards.Length; i++)
            {
                enemyCards[i].gameObject.SetActive(false);
            }
            playerCard.gameObject.SetActive(false);
            if (playerUnit.currentHP <= 0)
            {
                state = BattleState.LOSE;
                EndBattle();
            }
            else if (enemyUnit.currentHP <= 0)
            {
                state = BattleState.WIN;
                EndBattle();
            }
            else
            {
                state = BattleState.PLAYER;
                PlayerTurn();
            }
            for (int i = 0; i < 5; i++)
            {
                if (gameManager.availableCardSlots[i])
                {
                    gameManager.DrawCard();
                }
            }
        }
        void EndBattle()
        {
            if(state == BattleState.WIN)
            {
                SceneManager.LoadScene("Win");
                //win the battle, move to next level
            }
            else if (state == BattleState.LOSE)
            {
                SceneManager.LoadScene("Lose");
                //lose the battle, move to menu or restart battle?
            }
        }
        public void CardInteraction(BasicCardType playerCard, BasicCardType enemyCard)
        {
            Debug.Log("Card interaction starting");
            int damage = 0;
            //Compare the cards
            //Player LUNGE
            if (playerCard == BasicCardType.LUNGE)
            {
                //Lunge vs Lunge
                if (enemyCard == BasicCardType.LUNGE)
                {
                    Debug.Log("The attacks clash!");
                    return;
                } 
                //Lunge vs Pounce
                else if (enemyCard == BasicCardType.POUNCE)
                {
                    damage = 3;
                    playerUnit.TakeDamage(damage);
                } 
                //Lunge vs Sneak
                else if (enemyCard == BasicCardType.SNEAK)
                {
                    damage = 2;
                    enemyUnit.TakeDamage(damage);
                } 
                //Lunge vs Feint
                else if (enemyCard == BasicCardType.FEINT)
                {
                    damage = 1;
                    enemyUnit.TakeDamage(damage);
                } 
                //Lunge vs Parry
                else if (enemyCard == BasicCardType.PARRY)
                {
                    damage = 2;
                    playerUnit.TakeDamage(damage);
                }
            }
            //Player POUNCE
            if (playerCard == BasicCardType.POUNCE)
            {
                //Pounce vs Lunge
                if (enemyCard == BasicCardType.LUNGE)
                {
                    damage = 3;
                    enemyUnit.TakeDamage(damage);
                }
                //Pounce vs Pounce
                else if (enemyCard == BasicCardType.POUNCE)
                {
                    Debug.Log("The attacks clash!");
                    return;
                }
                //Pounce vs Sneak
                else if (enemyCard == BasicCardType.SNEAK)
                {
                    damage = 1;
                    enemyUnit.TakeDamage(damage);
                }
                //Pounce vs Feint
                else if (enemyCard == BasicCardType.FEINT)
                {
                    damage = 2;
                    playerUnit.TakeDamage(damage);
                }
                //Pounce vs Parry
                else if (enemyCard == BasicCardType.PARRY)
                {
                    damage = 1;
                    playerUnit.TakeDamage(damage);
                }
            }
            //Player SNEAK
            if (playerCard == BasicCardType.SNEAK)
            {
                //Sneak vs Lunge
                if (enemyCard == BasicCardType.LUNGE)
                {
                    damage = 2;
                    playerUnit.TakeDamage(damage);
                }
                //Sneak vs Pounce
                else if (enemyCard == BasicCardType.POUNCE)
                {
                    damage = 1;
                    playerUnit.TakeDamage(damage);
                }
                //Sneak vs Sneak
                else if (enemyCard == BasicCardType.SNEAK)
                {
                    Debug.Log("The attacks clash!");
                    return;
                }
                //Sneak vs Feint
                else if (enemyCard == BasicCardType.FEINT)
                {
                    damage = 1;
                    enemyUnit.TakeDamage(damage);
                }
                //Sneak vs Parry
                else if (enemyCard == BasicCardType.PARRY)
                {
                    damage = 4;
                    enemyUnit.TakeDamage(damage);
                }
            }
            //Player FEINT
            if (playerCard == BasicCardType.FEINT)
            {
                //Feint vs Lunge
                if (enemyCard == BasicCardType.LUNGE)
                {
                    damage = 1;
                    playerUnit.TakeDamage(damage);
                }
                //Feint vs Pounce
                else if (enemyCard == BasicCardType.POUNCE)
                {
                    damage = 2;
                    enemyUnit.TakeDamage(damage);
                }
                //Feint vs Sneak
                else if (enemyCard == BasicCardType.SNEAK)
                {
                    damage = 1;
                    playerUnit.TakeDamage(damage);
                }
                //Feint vs Feint
                else if (enemyCard == BasicCardType.FEINT)
                {
                    Debug.Log("The attacks clash!");
                    return;
                }
                //Feint vs Parry
                else if (enemyCard == BasicCardType.PARRY)
                {
                    damage = 3;
                    enemyUnit.TakeDamage(damage);
                }
            }
            //Player PARRY
            if (playerCard == BasicCardType.PARRY)
            {
                //Parry vs Lunge
                if (enemyCard == BasicCardType.LUNGE)
                {
                    damage = 2;
                    enemyUnit.TakeDamage(damage);
                }
                //Parry vs Pounce
                else if (enemyCard == BasicCardType.POUNCE)
                {
                    damage = 1;
                    enemyUnit.TakeDamage(damage);
                }
                //Parry vs Sneak
                else if (enemyCard == BasicCardType.SNEAK)
                {
                    damage = 4;
                    playerUnit.TakeDamage(damage);
                }
                //Parry vs Feint
                else if (enemyCard == BasicCardType.FEINT)
                {
                    damage = 3;
                    playerUnit.TakeDamage(damage);
                }
                //Parry vs Parry
                else if (enemyCard == BasicCardType.PARRY)
                {
                    Debug.Log("The attacks clash!");
                    return;
                }
            }
            playerHUD.SetHP(playerUnit.currentHP, playerUnit);
            enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit);
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

        //turns keeps count of how many turns have passed to change behavior
        int turns = 0;
        public BasicCardType GetEnemyChoice()
        {
            BasicCardType choice = BasicCardType.NONE;
            switch (enemyUnit.gameObject.name)
            {
                case "Peanut(Clone)":
                    if (turns < 3)
                    {
                        choice = BasicCardType.LUNGE;
                        turns++;
                    }
                    else
                    {
                        choice = BasicCardType.PARRY;
                        turns = 0;
                    }
                    break;
                case "Fluffy(Clone)":
                    choice = BasicCardType.NONE;
                    break;
                case null:
                    choice = BasicCardType.NONE;
                    break;
            }
            return choice;
        }
        public void StateText()
        {
            if (state == BattleState.START)
            {
                text.text = "Start!";
            }
            if (state == BattleState.PLAYER)
            {
                text.text = "Player Turn";
            }
            if (state == BattleState.BATTLE || state == BattleState.CLEANUP)
            {
                text.text = "Clash!";
            }
        }
    }
}