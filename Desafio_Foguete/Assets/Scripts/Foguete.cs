using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Foguete : MonoBehaviour
{
    Rigidbody _rb;
    Rigidbody _rbMotor01;
    Rigidbody _rbMotor02;
    public GameObject Motor01;
    public GameObject Motor02;
    public GameObject Paraquedas;
    public GameObject paraquedasPoint;
    public GameObject gameOverObj;
    public ParticleSystem fogueteParticle;
    public Gasolina gasolina;
    public Menu menu;
    public float speed;
    public float fallDrag;
    public int maxGas = 5;

    AudioSource fogueteSFX;
    private float currentGas;

    private bool abrirParaquedas;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rbMotor01 = Motor01.GetComponent<Rigidbody>();
        _rbMotor02 = Motor02.GetComponent<Rigidbody>();
        fogueteSFX = GetComponent<AudioSource>();
        fogueteSFX.Play(0);
        fogueteSFX.volume = .6f;
        fogueteParticle.Play();

        //Reseta variaveis
        gameOverObj.SetActive(false);
        abrirParaquedas = true;
        Paraquedas.SetActive(false);
        currentGas = maxGas;
        gasolina.SetMaxGas(maxGas);
    }

    void Update()
    {
        if (GameController.Instance.State != GameController.GameStates.PLAYING)
        {
            return;

        }

        //Primeiro o else é chamado para fazer o foguete subir
        //Quando o timer é <= a 0 o if é chamado
        //Depois de 5 segundos o motor é separado
        if (currentGas <= 0)
        {
            gameOverObj.SetActive(true);

            //Função para separar o motor do foguete
            SepararMotor();

            //Quando o foguete tiver o impulso nulo
            if (_rb.velocity.y < 1)
            {
                //Start na coroutine que faz um fadeout no som de foguete
                StartCoroutine(audioFadeOut());
                fogueteParticle.Stop();
                //É checada a condição se o paraquedas pode ser aberto
                if (abrirParaquedas)
                {
                    //Se a variavel for verdadeira o paraquedas é aberto e a descida pode ser controlada pelo script contido no paraquedas
                    Paraquedas.transform.position = paraquedasPoint.transform.position;
                    Paraquedas.SetActive(true);
                    abrirParaquedas = false;

                }   
            }
        }
        else
        {
            //Foguete recebe força no eixo Y
            _rb.AddForce(Vector3.up * speed);


            //Timer é subtraido 
            currentGas -= Time.deltaTime;
            gasolina.SetGas(((int)currentGas));
        }
        

    }

    //Coroutina que tira .1 de volume do audio para dar o efeito de fadeout
    private IEnumerator audioFadeOut()
    {
        yield return new WaitForSeconds(.6f);
        if (fogueteSFX.volume > 0)
        {
            fogueteSFX.volume -= 0.1f;
            print("volume -.1");

        }

    }

    //Função para ser chamada no update
    //Tive algumas dificuldades para fazer a ejeção dos motores, então resolvi manipular o rigidibody
    //deixando kinematic até ele precisar ser ejetado
    //como também ligando sua gravidade
    // e o tirando do parentesco do foguete
    private void SepararMotor()
    {
        _rbMotor01.isKinematic = false;
        _rbMotor02.isKinematic = false;
        _rbMotor01.useGravity = true;
        _rbMotor02.useGravity = true;
        Motor01.transform.parent = null;
        Motor02.transform.parent = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("gameover"))
        {
            GameController.Instance.State = GameController.GameStates.GAMEOVER;

        }
        if (collision.gameObject.CompareTag("win"))
        {
            GameController.Instance.State = GameController.GameStates.WIN;

        }
    }
}
