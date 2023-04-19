using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts;
using System;

public class NPCDialogue : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public string[] dialogue;
    public bool inRange;
    public float textSpeed;
    public bool dialogueActive;
    public string NPC_name;
    public BattleSystem battleSystem;
    public Unit playerUnit;
    public Unit enemyUnit;

    private int index;

    void Start()
    {

    }

    void Update()
    {
        if (battleSystem.state == BattleState.PLAYER)
        {
            if (!dialogueActive)
            {
                //StartDialogue();
                Debug.Log("Dialogue");
            }
            else
            {
                if (dialogueText.text == dialogue[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    dialogueText.text = dialogue[index];
                }
            }
            
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in dialogue[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if(index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueBox.SetActive(false);
            dialogueActive = false;
            dialogueText.text = string.Empty;
            //player.canMove = true;
            //player.GetComponent<Animator>().enabled = true;
        }
    }

    void StartDialogue()
    {
        index = 0;
        nameText.text = NPC_name;
        dialogueActive = true;
        dialogueBox.SetActive(true);
        StartCoroutine(TypeLine());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            dialogueBox.SetActive(false);
            dialogueActive = false;
            //player.GetComponent<Animator>().enabled = true;
            //player.canMove = true;
        }
    }
}
