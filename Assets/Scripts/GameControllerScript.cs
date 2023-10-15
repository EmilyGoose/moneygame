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
    
    // API wrapper
    private APIWrapper backend;
    
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

        backend = GetComponent<APIWrapper>();
        
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
        
        textBox.SetText("First question! How much did you spend on food this month?");
        
        dictationController.toggleRecord();
        
        yield return new WaitForSeconds(6);
        
        dictationController.toggleRecord();
        
        textBox.SetText("Processing audio...");

        while (!dictationController.doneProcessing)
        {
            yield return new WaitForSeconds(0.5F);
        }

        int guessedMoney = parseAnswer(dictationController.resultText);
        dictationController.resultText = "";
        
        textBox.SetText("Contacting server...");
        
        backend.RequestAnswer(guessedMoney, "Food");
        
        while (!backend.requestComplete)
        {
            yield return new WaitForSeconds(0.5F);
        }
        backend.requestComplete = false;
        
        // Output result
        textBox.SetText(backend.geePeeTee);

        // Spawn coins
        guessCoinScript.burgersToSpawn = guessedMoney;
        actualCoinScript.burgersToSpawn = backend.actualValue;
        
        // Spawn burgers (around $5 per burger fuck it)
        burgerSpawner.burgersToSpawn = backend.actualValue / 5;
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

