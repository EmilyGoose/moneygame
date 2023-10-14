using System.Collections;
using TMPro;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{

    public GameObject textArea;
    public GameObject tacoButton;
    
    // Jars for comparison
    public GameObject guessJar;
    public GameObject actualJar;
    
    private TextMeshProUGUI textBox;
    private TacoTrigger tacoTrigger;
    
    
    // Start is called before the first frame update
    void Start()
    {
        textBox = textArea.GetComponent<TextMeshProUGUI>();
        tacoTrigger = tacoButton.GetComponent<TacoTrigger>();
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
    }
}
