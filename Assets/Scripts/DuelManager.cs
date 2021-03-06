﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DuelManager : MonoBehaviour
{

    bool buttonPressed = false;
    bool doublePress = false;
    bool initialized = false;
    string buttonPressedName = "";


    public Inventory inventory;
    public List<string> currentSequence = new List<string>();

    public TextMeshProUGUI spellName;
    public TextMeshProUGUI spellDamage;
    public Image spellIcon;
    public List<Image> combo, spellChoice;
    public Sprite transparent;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized)
            OnPlayerCreated();


        if (Input.GetAxisRaw("Horizontal") <= 0.2
            && Input.GetAxisRaw("Vertical") <= 0.2
            && Input.GetAxisRaw("Horizontal") >= -0.2
            && Input.GetAxisRaw("Vertical") >= -0.2
            && (buttonPressedName == "Vertical" || buttonPressedName == "Horizontal"))
        {
            buttonPressed = false;
            buttonPressedName = "";
        }
        else if (Input.GetAxisRaw("Horizontal") <= 0.2
        && Input.GetAxisRaw("Vertical") <= 0.2
        && Input.GetAxisRaw("Horizontal") >= -0.2
        && Input.GetAxisRaw("Vertical") >= -0.2)
        {
            if (buttonPressedName != "")
            {
                if (Input.GetButtonUp(buttonPressedName))
                {
                    buttonPressed = false;
                    buttonPressedName = "";
                }
            }
            else
            {
                buttonPressed = false;
                buttonPressedName = "";
            }
        }

        if (!buttonPressed)
        {
            //A
            if (Input.GetButtonDown("A"))
            {
                OnBoutonDown("A");
            }

            //B
            if (Input.GetButtonDown("B"))
            {
                OnBoutonDown("B");
            }

            //X
            if (Input.GetButtonDown("X"))
            {
                OnBoutonDown("X");
            }

            //Y
            if (Input.GetButtonDown("Y"))
            {
                OnBoutonDown("Y");
            }

            //joystick gauche
            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                OnBoutonDown("LEFT");
                buttonPressedName = "Horizontal";
            }

            //joystick droite
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                OnBoutonDown("RIGHT");
                buttonPressedName = "Horizontal";
            }

            //joystick bas
            if (Input.GetAxisRaw("Vertical") == -1)
            {
                OnBoutonDown("UP");
                buttonPressedName = "Vertical";
            }

            //joystick haut
            if (Input.GetAxisRaw("Vertical") == 1)
            {
                OnBoutonDown("DOWN");
                buttonPressedName = "Vertical";
            }



            if (doublePress)
            {
                currentSequence.Clear();
                //Do Stuff
            }
            else if (currentSequence.Count > 0)
                VerifySequence();


        }

        //Dpad droite
        if (Input.GetAxis("Horizontal2") >= 0.7f)
        {
            if (inventory.spellCount >= 2)
                UpdateHeader(inventory.spells[1]);
        }

        //Dpad gauche
        if (Input.GetAxis("Horizontal2") <= -0.7f)
        {
            if (inventory.spellCount >= 4)
                UpdateHeader(inventory.spells[3]);
        }

        //Dpad haut
        if (Input.GetAxis("Vertical2") <= -0.7f)
        {
            UpdateHeader(inventory.spells[0]);
        }

        //Dpad bas
        if (Input.GetAxis("Vertical2") >= 0.7f)
        {
            if (inventory.spellCount >= 3)
                UpdateHeader(inventory.spells[2]);
        }

        doublePress = false;

    }

    public void VerifySequence()
    {
        bool allFailed = true;
        Spell spellCasted = null;

        foreach (Spell spell in inventory.spells)
        {
            bool failed = false;
            for (int i = 0; i < currentSequence.Count; i++)
            {
                if (i < spell.listeInputs.Count)
                {
                    if (currentSequence[i] == spell.listeInputs[i].name && !failed)
                    {
                        if (i + 1 == spell.listeInputs.Count)
                        {
                            spellCasted = spell;
                            currentSequence.Clear();
                        }
                    }
                    else
                    {
                        failed = true;
                    }
                }
                else
                    failed = true;

            }
            if (!failed)
                allFailed = false;
        }

        if (allFailed)
        {
            currentSequence.Clear();
            Debug.Log("fail");
        }
        if (spellCasted != null)
        {
            //Do stuff
            Debug.Log(spellCasted.name + " casted");
            currentSequence.Clear();
        }
    }

    public void OnBoutonDown(string name)
    {
        if (buttonPressed)
            doublePress = true;
        buttonPressed = true;
        currentSequence.Add(name);
        buttonPressedName = name;
        Debug.Log(name);
    }


    public void UpdateHeader(Spell spell)
    {
        spellName.text = spell.name;
        spellDamage.text = "Damage: " + spell.damage;
        spellIcon.sprite = spell.sprite;
        for (int i = 0; i < combo.Count; i++)
        {
            if (i >= spell.listeInputs.Count)
                combo[i].enabled = false;

            else
            {
                combo[i].enabled = true;
                combo[i].sprite = spell.listeInputs[i].sprite;
            }
        }
    }

    public void OnPlayerCreated()
    {
        currentSequence.Clear();
        for (int i = 0; i < spellChoice.Count; i++)
        {
            spellChoice[i].enabled = false;
        }
        for (int i = 0; i < inventory.spells.Count; i++)
        {
            spellChoice[i].enabled = true;
            spellChoice[i].sprite = inventory.spells[i].sprite;
        }
        Debug.Log(inventory.spells[0].name);
        UpdateHeader(inventory.spells[0]);
        initialized = true;
    }
}
