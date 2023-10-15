using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public GameObject textArea;
    public GameObject tacoButton;
    
    // Dictation
    public DictationController dictationController;
    
    // Jars for comparison
    public GameObject guessJar;
    public GameObject actualJar;
    
    // Borger
    public GameObject burgerFactory;
    
    private TextMeshProUGUI textBox;
    private TacoTrigger tacoTrigger;

    private BurgerFactoryScript guessCoinScript;
    private BurgerFactoryScript actualCoinScript;
    private BurgerFactoryScript burgerSpawner;
    
    // Start is called before the first frame update
    void Start()
    {
        textBox = textArea.GetComponent<TextMeshProUGUI>();
        tacoTrigger = tacoButton.GetComponent<TacoTrigger>();

        guessCoinScript = guessJar.GetComponent<BurgerFactoryScript>();
        actualCoinScript = actualJar.GetComponent<BurgerFactoryScript>();
        burgerSpawner = burgerFactory.GetComponent<BurgerFactoryScript>();
        
        StartCoroutine(gameCoroutine());
    }
    
    IEnumerator gameCoroutine()
    {
        
        textBox.SetText("Welcome to Show Me The Money! Hit the taco to start.");

        // Wait for the trigger volume to be hit
        while (!tacoTrigger.hit)
        {
            yield return new WaitForSeconds(0.5F);
        }
        
        textBox.SetText("First question! How much did you spend on McDonald's this month?");
        
        dictationController.toggleRecord();
        
        yield return new WaitForSeconds(6);
        
        dictationController.toggleRecord();
        
        textBox.SetText("Processing...");

        while (!dictationController.doneProcessing)
        {
            yield return new WaitForSeconds(0.5F);
        }

        int guessedMoney = parseAnswer(dictationController.resultText);
        dictationController.resultText = "";
        
        
        
        yield return new WaitForSeconds(5);

        textBox.SetText("Wrong bitch. 10 milion.");

        // Spawn coins
        guessCoinScript.burgersToSpawn = guessedMoney;
        actualCoinScript.burgersToSpawn = 500;
        
        // Spawn burger
        burgerSpawner.burgersToSpawn = 500;
    }
    
    // i love regex
    private int parseAnswer(String matchText)
    {
        var pattern = @"[0-9]+";
        Regex compiledRegex = new Regex(pattern, RegexOptions.IgnoreCase);
        Match m = compiledRegex.Match(matchText);

        if (m.Success)
        {
            return int.Parse(m.Value);
        }
        else
        {
            // fuck it we ball
            return 50;
        }
    }
}

