using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    //raio de visao
    public float viewRadius = 5;
    //angulo de visao
    public float viewAngle = 115;

    //variaveis para identificarmos o que eh obtaculo e o que deve ser detectado respectivamente
    public LayerMask obstacleMask, detectionMask;

    //vetor de possiveis alvos em nosso raio de visao
    public Collider2D[] targetsInRadius;

    //lista de posicoes de possiveis alvos visiveis
    public List<Transform> visibleTargets = new List<Transform>();
    
    private void Update()
    {
        FindVisibleTargets();
    }

    void FindVisibleTargets()
    {
        //cria um circulo centrado no detentor desse script, com raio definido pelo desenvolvedor e que ira retornar
        //quantos colisores que entrarem nesse circulo que estejam na camada de deteccao e ira colocar esses colisores
        // no vetor passado como terceiro parametro
        targetsInRadius = Physics2D.OverlapCircleAll(transform.position,
            viewRadius,
            detectionMask,
            -Mathf.Infinity,
            Mathf.Infinity);

        //limpa o vetor de alvos visiveis
        visibleTargets.Clear();

        //Debug.Log(targetsInRadius.Length);
        //percorre todo o vetor de alvos no raio de visao
        for (int i = 0; i < targetsInRadius.Length; i++)
        {
            //aloca a transformacao do objeto no vetor para uma variavel para facilitar a manipulacao/comparacao
            Transform target = targetsInRadius[i].transform;

            //calculo para descobrir o vetor direcao a partir do detentor desse script para o alvo
            Vector2 dirTarget = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);

            //tranform.right sempre aponta para o eixo x positivo
            //se o angulo entre o vetor de direçao entre o jogador e o alvo, e o vetor positivo X for menor que o 
            //angulo de visao dividido por 2
            Vector2 dir = new Vector2();
            dir = transform.right;

            if (Vector2.Angle(dirTarget, dir) < viewAngle / 2)
            {
                //armazena a distancia entre o detentor desse script e o alvo
                float distanceTarget = Vector2.Distance(transform.position, target.position);

                //Se ao lancarmos um raio na direcao do alvo e na distancia do alvo e nao colidir com nenhum objeto,
                //adicionamos esse alvo no vetor de alvos visiveis
                if (!Physics2D.Raycast(transform.position, dirTarget, distanceTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    public Vector2 DirFromAngle(float angleDeg, bool global)
    {
        // se os angulos não forem globais, transforma eles em globais
        if (!global)
        {
            angleDeg += transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Cos(angleDeg * Mathf.Deg2Rad), Mathf.Sin(angleDeg * Mathf.Deg2Rad));
    }
}
