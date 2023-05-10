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
    public enum BattleState { START, PREBATTLE, PLAYER, BATTLE, CLEANUP, WIN, LOSE, EPILOGUE }
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

        //state text
        public TextMeshProUGUI text;

        //dialogue text
        public TextMeshProUGUI dialogueText;
        public GameObject dialogueBox;

        int randomNum;
        int randomNum2;

        public AudioClip attackClip;
        public AudioClip damageClip;
        public AudioClip neutralClip;

        public AudioSource source;

        BasicCardType[] randomCards = new BasicCardType[5];

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            Debug.Log(gameManager.deck.Count);
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

            randomNum = Random.Range(0, 4);
            randomNum2 = Random.Range(0, 1);

        }

        private void Update()
        {
            StateText();
        }
        IEnumerator SetupBattle()
        {
            gameManager.Shuffle();
            GameObject playerGO = Instantiate(playerPf, playerSpawn);
            playerUnit = playerGO.GetComponent<Unit>();
            playerHUD.SetHUD(playerUnit);

            GameObject enemyGO = Instantiate(enemyPf, enemySpawn);
            enemyUnit = enemyGO.GetComponent<Unit>();
            enemyHUD.SetHUD(enemyUnit);

            yield return new WaitForSeconds(1f);
            state = BattleState.PREBATTLE;
            StartCoroutine(PreBattle());
        }

        IEnumerator PreBattle()
        {
            dialogueBox.SetActive(true);
            SetDialogue();

            yield return new WaitForSeconds(5f);
            dialogueBox.SetActive(false);
            state = BattleState.PLAYER;
            PlayerTurn();
        }

        IEnumerator PlayCard()
        {
            //get values to compare to enemy's card on ENEMYTURN
            //use SetChoice to find out what card player chose on click
            SetBattleDialogue();
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
            SetBattleDialogue();
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
            for (int i = 0; i < enemyCards.Length; i++)
            {
                enemyCards[i].gameObject.SetActive(false);
            }
            playerCard.gameObject.SetActive(false);
            for (int i = 0; i < 5; i++)
            {
                if (gameManager.availableCardSlots[i])
                {
                    gameManager.DrawCard();
                }
            }
            if (playerUnit.currentHP <= 0)
            {
                state = BattleState.LOSE;
                playerUnit.PlayDeathAnim();
                yield return new WaitForSeconds(4f);
                EndBattle();
            }
            //else if (gameManager.deck.Count == 0)
            //{
            //    state = BattleState.LOSE;
            //    playerUnit.PlayDeathAnim();
            //    yield return new WaitForSeconds(4f);
            //    EndBattle();
            //}
            else if (enemyUnit.currentHP <= 0)
            {
                state = BattleState.WIN;
                enemyUnit.PlayDeathAnim();
                yield return new WaitForSeconds(4f);
                state = BattleState.EPILOGUE;
                StartCoroutine(Epilogue());
                //EndBattle();
            }
            else
            {
                state = BattleState.PLAYER;
                PlayerTurn();
            }
        }

        //call before end of battle to show text before switching scenes
        IEnumerator Epilogue()
        {
            dialogueBox.SetActive(true);
            SetDialogue();
            yield return new WaitForSeconds(5f);
            dialogueBox.SetActive(false);
            EndBattle();
        }
        void EndBattle()
        {
            if (state == BattleState.WIN || state == BattleState.EPILOGUE)
            {
                turns = 0;
                //the following code used for level select, added by Sage
                int indexToActivate = 0;
                switch (enemyUnit.gameObject.name)
                {
                    case "Peanut":
                        indexToActivate = 1;
                        break;
                    case "Ollie":
                        indexToActivate = 2;
                        break;
                    case "Moses":
                        indexToActivate = 3;
                        break;
                    case "Snowball":
                        indexToActivate = 4;
                        break;
                    case "Fluffy":
                        StopAllCoroutines();
                        SceneManager.LoadScene("Ending1");
                        break;
                }
                GameObject.FindWithTag("Progress").GetComponent<Progress>().values[indexToActivate] = 1;
                //end code done by Sage
                if (enemyUnit.gameObject.name != "Fluffy")
                {
                    SceneManager.LoadScene("Win");
                    //win the battle, move to next level
                }
            }
            else if (state == BattleState.LOSE)
            {
                turns = 0;
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
                    source.PlayOneShot(neutralClip);
                    dialogueBox.SetActive(true);
                    dialogueText.text = "The attacks clash!";
                    return;
                }
                //Lunge vs Pounce
                else if (enemyCard == BasicCardType.POUNCE)
                {
                    damage = 3;
                    enemyUnit.PlayAttackAnim();
                    source.PlayOneShot(damageClip);
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
                //Lunge vs Sneak
                else if (enemyCard == BasicCardType.SNEAK)
                {
                    damage = 2;
                    playerUnit.PlayAttackAnim();
                    source.PlayOneShot(attackClip);
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
                //Lunge vs Feint
                else if (enemyCard == BasicCardType.FEINT)
                {
                    damage = 1;
                    playerUnit.PlayAttackAnim();
                    source.PlayOneShot(attackClip);
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
                //Lunge vs Parry
                else if (enemyCard == BasicCardType.PARRY)
                {
                    damage = 2;
                    enemyUnit.PlayAttackAnim();
                    source.PlayOneShot(damageClip);
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
            }
            //Player POUNCE
            if (playerCard == BasicCardType.POUNCE)
            {
                //Pounce vs Lunge
                if (enemyCard == BasicCardType.LUNGE)
                {
                    damage = 3;
                    playerUnit.PlayAttackAnim();
                    source.PlayOneShot(attackClip);
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
                //Pounce vs Pounce
                else if (enemyCard == BasicCardType.POUNCE)
                {
                    Debug.Log("The attacks clash!");
                    source.PlayOneShot(neutralClip);
                    dialogueBox.SetActive(true);
                    dialogueText.text = "The attacks clash!";
                    return;
                }
                //Pounce vs Sneak
                else if (enemyCard == BasicCardType.SNEAK)
                {
                    damage = 1;
                    playerUnit.PlayAttackAnim();
                    source.PlayOneShot(attackClip);
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
                //Pounce vs Feint
                else if (enemyCard == BasicCardType.FEINT)
                {
                    damage = 2;
                    enemyUnit.PlayAttackAnim();
                    source.PlayOneShot(damageClip);
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
                //Pounce vs Parry
                else if (enemyCard == BasicCardType.PARRY)
                {
                    damage = 1;
                    enemyUnit.PlayAttackAnim();
                    source.PlayOneShot(damageClip);
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
            }
            //Player SNEAK
            if (playerCard == BasicCardType.SNEAK)
            {
                //Sneak vs Lunge
                if (enemyCard == BasicCardType.LUNGE)
                {
                    damage = 2;
                    enemyUnit.PlayAttackAnim();
                    source.PlayOneShot(damageClip);
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
                //Sneak vs Pounce
                else if (enemyCard == BasicCardType.POUNCE)
                {
                    damage = 1;
                    enemyUnit.PlayAttackAnim();
                    source.PlayOneShot(damageClip);
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
                //Sneak vs Sneak
                else if (enemyCard == BasicCardType.SNEAK)
                {
                    Debug.Log("The attacks clash!");
                    source.PlayOneShot(neutralClip);
                    dialogueBox.SetActive(true);
                    dialogueText.text = "The attacks clash!";
                    return;
                }
                //Sneak vs Feint
                else if (enemyCard == BasicCardType.FEINT)
                {
                    damage = 1;
                    playerUnit.PlayAttackAnim();
                    source.PlayOneShot(attackClip);
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
                //Sneak vs Parry
                else if (enemyCard == BasicCardType.PARRY)
                {
                    damage = 4;
                    playerUnit.PlayAttackAnim();
                    source.PlayOneShot(attackClip);
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
            }
            //Player FEINT
            if (playerCard == BasicCardType.FEINT)
            {
                //Feint vs Lunge
                if (enemyCard == BasicCardType.LUNGE)
                {
                    damage = 1;
                    enemyUnit.PlayAttackAnim();
                    source.PlayOneShot(damageClip);
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
                //Feint vs Pounce
                else if (enemyCard == BasicCardType.POUNCE)
                {
                    damage = 2;
                    playerUnit.PlayAttackAnim();
                    source.PlayOneShot(attackClip);
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
                //Feint vs Sneak
                else if (enemyCard == BasicCardType.SNEAK)
                {
                    damage = 1;
                    enemyUnit.PlayAttackAnim();
                    source.PlayOneShot(damageClip);
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
                //Feint vs Feint
                else if (enemyCard == BasicCardType.FEINT)
                {
                    Debug.Log("The attacks clash!");
                    source.PlayOneShot(neutralClip);
                    dialogueBox.SetActive(true);
                    dialogueText.text = "The attacks clash!";
                    return;
                }
                //Feint vs Parry
                else if (enemyCard == BasicCardType.PARRY)
                {
                    damage = 3;
                    playerUnit.PlayAttackAnim();
                    source.PlayOneShot(attackClip);
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
            }
            //Player PARRY
            if (playerCard == BasicCardType.PARRY)
            {
                //Parry vs Lunge
                if (enemyCard == BasicCardType.LUNGE)
                {
                    damage = 2;
                    playerUnit.PlayAttackAnim();
                    source.PlayOneShot(attackClip);
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
                //Parry vs Pounce
                else if (enemyCard == BasicCardType.POUNCE)
                {
                    damage = 1;
                    playerUnit.PlayAttackAnim();
                    source.PlayOneShot(attackClip);
                    enemyUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = enemyUnit.unitName + " takes " + damage + " damage!";
                }
                //Parry vs Sneak
                else if (enemyCard == BasicCardType.SNEAK)
                {
                    damage = 4;
                    enemyUnit.PlayAttackAnim();
                    source.PlayOneShot(damageClip);
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
                //Parry vs Feint
                else if (enemyCard == BasicCardType.FEINT)
                {
                    damage = 3;
                    enemyUnit.PlayAttackAnim();
                    source.PlayOneShot(damageClip);
                    playerUnit.TakeDamage(damage);
                    dialogueBox.SetActive(true);
                    dialogueText.text = playerUnit.unitName + " takes " + damage + " damage!";
                }
                //Parry vs Parry
                else if (enemyCard == BasicCardType.PARRY)
                {
                    Debug.Log("The attacks clash!");
                    source.PlayOneShot(neutralClip);
                    dialogueBox.SetActive(true);
                    dialogueText.text = "The attacks clash!";
                    return;
                }
            }
            playerHUD.SetHP(playerUnit);
            enemyHUD.SetHP(enemyUnit);
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
        public int turns = 0;

        public BasicCardType GetEnemyChoice()
        {

            BasicCardType choice = BasicCardType.NONE;
            switch (enemyUnit.gameObject.name)
            {
                case "Peanut":
                    // Lunge, Lunge, Lunge, Parry...
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
                    // Random (Lunge or Pounce), Feint, Random (L or Po), Sneak, Random (L or Po), Sneak...
                    if (turns == 1)
                    {
                        choice = BasicCardType.FEINT;
                        turns++;
                    }
                    else if (turns % 2 == 0)
                    {
                        choice = randomCards[randomNum2];
                        turns++;
                    }
                    else if (turns == 3)
                    {
                        choice = BasicCardType.SNEAK;
                        turns++;
                    }
                    else if (turns == 5)
                    {
                        choice = BasicCardType.SNEAK;
                        turns = 0;
                    }
                    break;
                case "Moses":
                    // Sneak, Feint, Feint, Pounce, Pounce, Pounce...
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
                    else if (turns <= 5)
                    {
                        choice = BasicCardType.POUNCE;
                        turns++;
                    }
                    else if (turns == 6)
                    {
                        choice = BasicCardType.POUNCE;
                        turns = 0;
                    }
                    break;
                case "Snowball":
                    // Feint, Random, Feint, Lunge, Feint, Lunge...
                    if (turns == 0)
                    {
                        choice = BasicCardType.FEINT;
                        turns++;
                    }
                    else if (turns == 1)
                    {
                        choice = randomCards[randomNum];
                        turns++;
                    }
                    else if (turns == 2)
                    {
                        choice = BasicCardType.FEINT;
                        turns++;
                    }
                    else if (turns == 3)
                    {
                        choice = BasicCardType.LUNGE;
                        turns++;
                    }
                    else if (turns == 4)
                    {
                        choice = BasicCardType.FEINT;
                        turns++;
                    }
                    else if (turns == 5)
                    {
                        choice = BasicCardType.LUNGE;
                        turns = 0;
                    }
                    break;
                case "Ollie":
                    // Lunge, Pounce, Lunge, Pounce...
                    if (turns % 2 == 1)
                    {
                        choice = BasicCardType.POUNCE;
                        turns++;
                    }
                    else if (turns % 2 == 0)
                    {
                        choice = BasicCardType.LUNGE;
                        turns++;
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
            if (state == BattleState.START || state == BattleState.PREBATTLE)
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
            if (state == BattleState.WIN || state == BattleState.EPILOGUE)
            {
                text.text = "You win!";
            }
            if (state == BattleState.LOSE)
            {
                text.text = "You lose!";
            }
        }

        public void SetBattleDialogue()
        {
            dialogueBox.SetActive(true);
            if (state == BattleState.BATTLE)
            {
                switch (enemyChoice)
                {
                    case BasicCardType.LUNGE:
                        dialogueText.text = enemyUnit.name + " tries to attack with a " + enemyChoice + "!";
                        break;
                    case BasicCardType.PARRY:
                        dialogueText.text = enemyUnit.name + " provokes an attack with a " + enemyChoice + "!";
                        break;
                    case BasicCardType.POUNCE:
                        dialogueText.text = enemyUnit.name + " attempts to bound with a " + enemyChoice + "!";
                        break;
                    case BasicCardType.SNEAK:
                        dialogueText.text = enemyUnit.name + " tries to evade with a " + enemyChoice + "!";
                        break;
                    case BasicCardType.FEINT:
                        dialogueText.text = enemyUnit.name + " attempts to divert an attack with a " + enemyChoice + "!";
                        break;
                }
            }
            else
            {
                switch (playerChoice)
                {
                    case BasicCardType.LUNGE:
                        dialogueText.text = playerUnit.name + " tries to attack with a " + playerChoice + "!";
                        break;
                    case BasicCardType.PARRY:
                        dialogueText.text = playerUnit.name + " provokes an attack with a " + playerChoice + "!";
                        break;
                    case BasicCardType.POUNCE:
                        dialogueText.text = playerUnit.name + " attempts to bound with a " + playerChoice + "!";
                        break;
                    case BasicCardType.SNEAK:
                        dialogueText.text = playerUnit.name + " tries to evade with a " + playerChoice + "!";
                        break;
                    case BasicCardType.FEINT:
                        dialogueText.text = playerUnit.name + " attempts to divert an attack with a " + playerChoice + "!";
                        break;
                }
            }
        }

        public void SetDialogue()
        {
            Debug.Log("Setting dialogue");
            if (state == BattleState.PREBATTLE)
            {
                switch (enemyUnit.name)
                {
                    case "Peanut":
                        dialogueText.text = "Peanut: Hi! Do you know why there's a line here? Maybe there's treats at the end!";
                        break;
                    case "Ollie":
                        dialogueText.text = "Ollie: This line is so boring. Oh! You should fight me!";
                        break;
                    case "Moses":
                        dialogueText.text = "Moses: You don't want to mess with me, I'm not in a good mood!";
                        break;
                    case "Snowball":
                        dialogueText.text = "Snowball: I don't feel to well, but I guess I'm in good enough shape to fight.";
                        break;
                    case "Fluffy":
                        dialogueText.text = "Fluffy: The queen isn't seeing visitors right now!";
                        break;
                }
            }
            if (state == BattleState.EPILOGUE)
            {
                switch (enemyUnit.name)
                {
                    case "Peanut":
                        dialogueText.text = "Peanut: Are you telling me there's really no food at the end of this line? Well, thanks for letting me know! I'm out of here.";
                        break;
                    case "Ollie":
                        dialogueText.text = "Ollie: It's no fun if you're the winner...";
                        break;
                    case "Moses":
                        dialogueText.text = "Moses: My darn kid ran off again. Tell Fluffy that they better be home by dinner if you see them.";
                        break;
                    case "Snowball":
                        dialogueText.text = "Snowball: I guess I really am not feeling my best.";
                        break;
                    case "Fluffy":
                        dialogueText.text = "Fluffy: I guess you caught me...";
                        break;

                }

            }
        }
    }
}