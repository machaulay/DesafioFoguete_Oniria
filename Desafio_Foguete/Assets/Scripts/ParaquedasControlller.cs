using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaquedasControlller : MonoBehaviour
{
    public KeyCode left;
    public KeyCode right;
    public Vector3 delta;
    public float speed;

    void Update()
    {
        //Função para aplicar o movimento na posição do paraquedas
        ApplyMovement();

        //Optei por usar inputs definidos peloo inpector para ficar mais facil a implementação
        if (Input.GetKey(left))
        {
            //Uma variavel delta do tipo Vector3 armazena se o input acionado foi negativo ou positivo, ou seja, esquerda ou direita.
            delta.x = -speed * Time.deltaTime;
        }

        if (Input.GetKey(right))
        {
            delta.x = speed * Time.deltaTime;
        }
    }

    public void ApplyMovement()
    {
        if (Input.GetKey(left) || Input.GetKey(right))
        {
            transform.position += delta;

        }
        else
        {
            delta.x = 0;
        }
    }
}
