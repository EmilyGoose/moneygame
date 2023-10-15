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

    // Labels
    public TextMeshProUGUI guessBox;
    public TextMeshProUGUI actualBox;
    
    // Borger
    public GameObject burgerFactory;

    private TextMeshProUGUI textBox;
    private TacoTrigger tacoTrigger;

    private BurgerFactoryScript guessCoinScript;
    private BurgerFactoryScript actualCoinScript;
    private BurgerFactoryScript burgerSpawner;
    public BurgerFactoryScript coffeeSpawner;

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
        // Kinda hacky way to run it twice without duplicate code
        // Ideally I should be storing this text better but this is my middle of the night hack
        // I will look back at this and cry
        foreach (
            var currentStringSet in
            new[]
            {
                new[]
                {
                    "Welcome to Show Me The Money! Hit the taco to start.",
                    "First question! How much did you spend on Food this month?",
                    "Grocery" // todo actual
                },
                new[]
                {
                    "When you're ready for the next question, hit the taco!",
                    "Next question! How much did you spend at Tim's?",
                    "Fuel" // todo actual
                }
            })
        {
            tacoTrigger.hit = false;
            textBox.SetText(currentStringSet[0]);

            // Wait for the trigger volume to be hit
            while (!tacoTrigger.hit)
            {
                yield return new WaitForSeconds(0.5F);
            }

            // Clear anything left in spawners or text
            guessCoinScript.burgersToSpawn = 0;
            actualCoinScript.burgersToSpawn = 0;
            burgerSpawner.burgersToSpawn = 0;
            coffeeSpawner.burgersToSpawn = 0;
            guessBox.SetText("");
            actualBox.SetText("");

            textBox.SetText(currentStringSet[1]);

            // Record dictation for 6s
            dictationController.toggleRecord();
            yield return new WaitForSeconds(6);
            dictationController.toggleRecord();

            textBox.SetText("Processing audio...");

            // Wait for dictation to process
            while (!dictationController.doneProcessing)
            {
                yield return new WaitForSeconds(0.5F);
            }

            // Grab the first int in the user's answer and hit the API
            int guessedMoney = parseAnswer(dictationController.resultText);
            guessBox.SetText($"You guessed: ${guessedMoney}");
            dictationController.resultText = "";

            textBox.SetText("Contacting server...");

            backend.RequestAnswer(guessedMoney, currentStringSet[2]);

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
            guessBox.SetText($"Actual amount: ${backend.actualValue}");

            // Spawn burgers (or coffee) (around $5 per burger and $2 per coffee)
            if (currentStringSet[2].Equals("Grocery"))
            {
                burgerSpawner.burgersToSpawn = backend.actualValue / 5;
            }
            else
            {
                coffeeSpawner.burgersToSpawn = backend.actualValue / 2;
            }
            
            yield return new WaitForSeconds(15F);
        }
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