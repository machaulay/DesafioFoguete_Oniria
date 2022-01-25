using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    //Nesse script uso um modelo de maquina de estado, bem simples que venho usando nos ultimos projetos
    //sempre adaptando para a necessidade
    //podendo optar por usar um contador no inicio, e tambem utilizar quantos estados forem necessarios
    //podendo ser trocas por qualquer script.

    public GameObject plataforma;
    public GameObject[] points;
    private GameStates state = GameStates.WAIT_TO_START;

    public KeyCode _sair;

    //UI
    int countToStart = 3;
    public Text countDownTxt;

    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();
            }
            return _instance;
        }
    }

    //Usar para determinar cada estado da gameplay
    public enum GameStates
    {
        WAIT_TO_START,
        PLAYING,
        WIN,
        GAMEOVER
    }

    public delegate void OnCountDownTick();
    public static OnCountDownTick OnCountDownTickEvent;

    public delegate void OnGameStart();
    public static OnGameStart OnGameStartEvent;
    public GameStates State { get => state; set => state = value; }

    //Diálogo


    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        countToStart--;
        if (countToStart < 1)
        {
            Debug.Log("Começe!");
            state = GameStates.PLAYING;
            countDownTxt.gameObject.SetActive(false);
            OnGameStartEvent?.Invoke();
            StartCoroutine(TrocaPlataforma());
        }
        else
        {
            Debug.Log("Espere...");
            countDownTxt.text = countToStart.ToString();
            OnCountDownTickEvent?.Invoke();
            StartCoroutine(Start());
        }
    }
    private void Update()
    {
        
        if (GameController.Instance.State == GameController.GameStates.GAMEOVER)
        {
            StartCoroutine(CarregaCena("Gameplay"));
        }
        if (GameController.Instance.State == GameController.GameStates.WIN)
        {
            StartCoroutine(CarregaCena("Menu"));
        }
        


        if (Input.GetKeyDown(_sair))
        {
            StartCoroutine(CarregaCena("Menu"));

        }
    }
    public static IEnumerator CarregaCena(string cena)
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(cena);
    }


    //Aqui fiz uma função para trocar a plataforma de lugar,
    //tentando dar um "desafio" basico para o teste do foguete.
    private IEnumerator TrocaPlataforma()
    {
        yield return new WaitForSeconds(5.0f);
        plataforma.transform.position = points[Random.Range(1, 3)].transform.position;

    }
}
