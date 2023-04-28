using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;

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

        public TextMeshProUGUI battleDialogue;
        public GameObject dialogueBox;

        public Animation playerAttackAnim;
        public Animation playerHurtAnim;
        public Animation playerSurrenderAnim;

        public Animation enemyAttackAnim;
        public Animation enemyHurtAnim;
        public Animation enemySurrenderAnim;

        BasicCardType[] randomCards = new BasicCardType[5];

        // Start is called before the first frame update
        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            state = BattleState.START;
            StartCoroutine(SetupBattle());
            if (playerUnit.name == "Clover(Clone)")
            {
                playerUnit.name = "Clover";
            }
            switch (enemyUnit.name)
            {
                case "Peanut(Clone)":
                    enemyUnit.name = "Peanut";
                    break;
                case "Fluffy(Clone)":
                    enemyUnit.name = "Fluffy";
                    break;
                case "Moses(Clone)":
                    enemyUnit.name = "Moses";
                    break;
                case "Ollie(Clone)":
                    enemyUnit.name = "Ollie";
                    break;
                case "Snowball(Clone)":
                    enemyUnit.name = "Snowball";
                    break;
            }
            randomCards[0] = BasicCardType.LUNGE;
            randomCards[1] = BasicCardType.POUNCE;
            randomCards[2] = BasicCardType.SNEAK;
            randomCards[3] = BasicCardType.PARRY;
            randomCards[4] = BasicCardType.FEINT;
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
            SetDialogueText();
            yield return new WaitForSeconds(2f);

            //change to ENEMYTURN after setting values
            state = BattleState.BATTLE;
            StartCoroutine(Compare());
        }

        //This function is not IEnumerator so the player can spend time on their turn
        void PlayerTurn()
        {
            Debug.Log("Player turn starting");
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
            SetDialogueText();
        }

        IEnumerator Compare()
        {
            Debug.Log("Cards are comparing");
            dialogueBox.SetActive(false);
            EnemyTurn();
            
            yield return new WaitForSeconds(2f);
            
            state = BattleState.CLEANUP;
            StartCoroutine(Cleanup());
        }

        IEnumerator Cleanup()
        {
            CardInteraction(playerChoice, enemyChoice);
            
            yield return new WaitForSeconds(2f);
            Debug.Log("Cleanup");
            dialogueBox.SetActive(false);
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
                StopAllCoroutines();
                //win the battle, move to next level
            }
            else if (state == BattleState.LOSE)
            {
                SceneManager.LoadScene("Lose");
                StopAllCoroutines();
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
                    dialogueBox.SetActive(true);
                    battleDialogue.text = "The attacks clash!";
                    return;
                } 
                //Lunge vs Pounce
                else if (enemyCard == BasicCardType.POUNCE)
                {
                    damage = 3;
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = playerUnit.unitName + " takes " + damage + " damage!";
                } 
                //Lunge vs Sneak
                else if (enemyCard == BasicCardType.SNEAK)
                {
                    damage = 2;
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = enemyUnit.unitName + " takes " + damage + " damage!";
                } 
                //Lunge vs Feint
                else if (enemyCard == BasicCardType.FEINT)
                {
                    damage = 1;
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = enemyUnit.unitName + " takes " + damage + " damage!";
                } 
                //Lunge vs Parry
                else if (enemyCard == BasicCardType.PARRY)
                {
                    damage = 2;
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = enemyUnit.unitName + " takes " + damage + " damage!";
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
                    dialogueBox.SetActive(true);
                    battleDialogue.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
                //Pounce vs Pounce
                else if (enemyCard == BasicCardType.POUNCE)
                {
                    Debug.Log("The attacks clash!");
                    dialogueBox.SetActive(true);
                    battleDialogue.text = "The attacks clash!";
                    return;
                }
                //Pounce vs Sneak
                else if (enemyCard == BasicCardType.SNEAK)
                {
                    damage = 1;
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
                //Pounce vs Feint
                else if (enemyCard == BasicCardType.FEINT)
                {
                    damage = 2;
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
                //Pounce vs Parry
                else if (enemyCard == BasicCardType.PARRY)
                {
                    damage = 1;
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = playerUnit.unitName + " takes " + damage + " damage!";
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
                    dialogueBox.SetActive(true);
                    battleDialogue.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
                //Sneak vs Pounce
                else if (enemyCard == BasicCardType.POUNCE)
                {
                    damage = 1;
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
                //Sneak vs Sneak
                else if (enemyCard == BasicCardType.SNEAK)
                {
                    Debug.Log("The attacks clash!");
                    dialogueBox.SetActive(true);
                    battleDialogue.text = "The attacks clash!";
                    return;
                }
                //Sneak vs Feint
                else if (enemyCard == BasicCardType.FEINT)
                {
                    damage = 1;
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
                //Sneak vs Parry
                else if (enemyCard == BasicCardType.PARRY)
                {
                    damage = 4;
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = enemyUnit.unitName + " takes " + damage + " damage!";
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
                    dialogueBox.SetActive(true);
                    battleDialogue.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
                //Feint vs Pounce
                else if (enemyCard == BasicCardType.POUNCE)
                {
                    damage = 2;
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
                //Feint vs Sneak
                else if (enemyCard == BasicCardType.SNEAK)
                {
                    damage = 1;
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
                //Feint vs Feint
                else if (enemyCard == BasicCardType.FEINT)
                {
                    Debug.Log("The attacks clash!");
                    dialogueBox.SetActive(true);
                    battleDialogue.text = "The attacks clash!";
                    return;
                }
                //Feint vs Parry
                else if (enemyCard == BasicCardType.PARRY)
                {
                    damage = 3;
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = enemyUnit.unitName + " takes " + damage + " damage!";
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
                    dialogueBox.SetActive(true);
                    battleDialogue.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
                //Parry vs Pounce
                else if (enemyCard == BasicCardType.POUNCE)
                {
                    damage = 1;
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
                //Parry vs Sneak
                else if (enemyCard == BasicCardType.SNEAK)
                {
                    damage = 4;
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
                //Parry vs Feint
                else if (enemyCard == BasicCardType.FEINT)
                {
                    damage = 3;
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    battleDialogue.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
                //Parry vs Parry
                else if (enemyCard == BasicCardType.PARRY)
                {
                    Debug.Log("The attacks clash!");
                    dialogueBox.SetActive(true);
                    battleDialogue.text = "The attacks clash!";
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
        int randomNum = Random.Range(0, 4);
        public BasicCardType GetEnemyChoice()
        {
            
            BasicCardType choice = BasicCardType.NONE;
            switch (enemyUnit.gameObject.name)
            {
                case "Peanut":
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
                case "Fluffy":
                    if (turns < 2)
                    {
                        choice = BasicCardType.POUNCE;
                        turns++;
                    }
                    else
                    {
                        choice = BasicCardType.LUNGE;
                        turns++;
                        if(turns == 3)
                        {
                            turns = 0;
                        }
                    }
                    break;
                case "Moses":
                    if (turns < 1)
                    {
                        choice = BasicCardType.SNEAK;
                        turns++;
                    }
                    else if (turns < 3)
                    {
                        choice = BasicCardType.FEINT;
                        turns++;
                    }
                    else if (turns < 5)
                    {
                        choice = BasicCardType.PARRY;
                        turns = 0;
                    }
                    break;
                case "Snowball":
                    if (turns % 2 == 0)
                    {
                        choice = BasicCardType.FEINT;
                        turns++;
                    }
                    else if (turns == 1)
                    {
                        choice = randomCards[randomNum];
                        turns++;
                    }
                    else if (turns == 3)
                    {
                        choice = BasicCardType.LUNGE;
                        turns++;
                    }
                    if(turns == 5)
                    {
                        choice = BasicCardType.LUNGE;
                        turns = 0;
                    }
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

        public void SetDialogueText()
        {
            dialogueBox.SetActive(true);
            if (state == BattleState.BATTLE)
            {
                switch (enemyChoice)
                {
                    case BasicCardType.LUNGE:
                        battleDialogue.text = enemyUnit.name + " tries to attack with a " + enemyChoice + "!";
                        break;
                    case BasicCardType.PARRY:
                        battleDialogue.text = enemyUnit.name + " provokes an attack with a " + enemyChoice + "!";
                        break;
                    case BasicCardType.POUNCE:
                        battleDialogue.text = enemyUnit.name + " attempts to bound with a " + enemyChoice + "!";
                        break;
                    case BasicCardType.SNEAK:
                        battleDialogue.text = enemyUnit.name + " tries to evade with a " + enemyChoice + "!";
                        break;
                    case BasicCardType.FEINT:
                        battleDialogue.text = enemyUnit.name + " attempts to divert an attack with a " + enemyChoice + "!";
                        break;
                }
            }
            else
            {
                switch (playerChoice)
                {
                    case BasicCardType.LUNGE:
                        battleDialogue.text = playerUnit.name + " tries to attack with a " + playerChoice + "!";
                        break;
                    case BasicCardType.PARRY:
                        battleDialogue.text = playerUnit.name + " provokes an attack with a " + playerChoice + "!";
                        break;
                    case BasicCardType.POUNCE:
                        battleDialogue.text = playerUnit.name + " attempts to bound with a " + playerChoice + "!";
                        break;
                    case BasicCardType.SNEAK:
                        battleDialogue.text = playerUnit.name + " tries to evade with a " + playerChoice + "!";
                        break;
                    case BasicCardType.FEINT:
                        battleDialogue.text = playerUnit.name + " attempts to divert an attack with a " + playerChoice + "!";
                        break;
                }
            }
            
            
        }
    }
}