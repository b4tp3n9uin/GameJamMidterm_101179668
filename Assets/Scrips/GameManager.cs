using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Pannels")]
    public GameObject MenuPannel;
    public GameObject InstructionsPannel;
    public GameObject CreditsPannel;


    // Start is called before the first frame update
    void Start()
    {
        MenuPannel.SetActive(true);
        InstructionsPannel.SetActive(false);
        CreditsPannel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPressed()
    {
        //Play Button
        SceneManager.LoadScene("SampleScene");
    }

    public void InstructionsPressed()
    {
        //Instructions Button
        MenuPannel.SetActive(false);
        InstructionsPannel.SetActive(true);
        CreditsPannel.SetActive(false);
    }

    public void CreditsPressed()
    {
        //Credits Button
        MenuPannel.SetActive(false);
        InstructionsPannel.SetActive(false);
        CreditsPannel.SetActive(true);
    }

    public void BackPressed()
    {
        //Back Button
        MenuPannel.SetActive(true);
        InstructionsPannel.SetActive(false);
        CreditsPannel.SetActive(false);
    }

    public void QuitPressed()
    {
        //Quit Button
        Debug.Log("Quit");
        Application.Quit();
    }
}
