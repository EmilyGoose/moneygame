using System.Collections;
using TMPro;
using UnityEngine;
// using Whisper;
// using Whisper.Utils;

public class GameControllerScript : MonoBehaviour
{
    public GameObject textArea;
    public GameObject tacoButton;
    
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

        yield return new WaitForSeconds(5);
        
        yield return new WaitForSeconds(2);

        textBox.SetText("Wrong bitch. 10 milion.");

        // Spawn coins
        guessCoinScript.burgersToSpawn = 10;
        actualCoinScript.burgersToSpawn = 500;
        
        // Spawn burger
        burgerSpawner.burgersToSpawn = 500;
    }
}
