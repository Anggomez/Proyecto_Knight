using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotauroBehaviourScript : MonoBehaviour
{
    #region  Variables Publicas
    public Transform raycast;
    public LayerMask raycastMascara;
    public float rayCastLongitud;
    public float ataqueDistancia; // Distancia minima para atacar
    public float VelocidadMovimiento;
    public float cooldown; // Tiempo que tarda entre ataques
    #endregion
    #region Private Variables
    private RaycastHit2D hit;
    private GambeObject target;
    private Animator anim;
    private float distancia; // Distancia entre enemigo y jugador
    private bool modoAtaque;
    private bool enRango;
    private bool comprobarCooldown;
    private float intTemporizador;
    #endregion

    void Awake()
    {
        intTemporizador = cooldown;
        anim = GetComponent<>();

    }
    // Se le llama 1 vez por cada Frame
    void Update()
    {
        if (InRange)
        {
            hit = Physics2D.RayCast(raycast.position, Vector2.left, rayCastLongitud, raycastMascara);
            RayCastDebugger();
        }
        // Cuando detecta el jugador
        if (hit.collider != null)
        {
            logicaEnemigo();

        }
        else if (hit.collider == null)
        {
            inRange = false;
        }

        if (inRange == false)
        {
            anim.SetBool("andar", false);
            StopAttack();
        }
    }
    void OnTriggerEnter2D(Collider2D trig)
    {// Si el tag del objeto en contacto, es el del jugador, 
        if (trig.gameObject.tag == "Player")
        {
            target = trig.gameObject;
            enRango = true;
        }
    }
    void logicaEnemigo()
    {
        distancia = Vector2.Distance(Transform.position, target.transform.position);

        if (distancia > ataqueDistancia)
        {
            Mover();
            PararAtaque();
        }
        else if (ataqueDistancia >= distancia && cooldown == false)
        {
            Atacar();
        }
        if (cooldown)
        {
            anim.setBool("ataque", false);
        }
    }
    void RayCastDebugger()
    {
        if (distancia > ataqueDistancia)
        {
            Debug.DrawRay(raycast.position, Vector2.left * rayCastLongitud, color.red);
        }
        else if (ataqueDistancia > distance)
        {
            Debug.DrawRay(raycast.position, Vector2.left * rayCastLongitud, color.green);
        }
    }

    void Move()
    {
        anim.setBool("andar", true);
        if (!anim.getCurrentAnimatorStateInfo(0).IsName("Minotauro_ataque"))
        {
            Vector2 targetPosicion = new Vector2(target.transform.positon.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosicion, VelocidadMovimiento * time.deltaTime);

        }
    }
    void Attack()
    {
        cooldown = intTemporizador; // resetea el tiempo desde que en jugador entra en rango de ataque
        modoAtaque = true;

        anim.setBool("andar", false);
        anim.setBool("ataque", false);
    }
    void StopAttack()
    {
        cooldown = false;
        modoAtaque = false;
        anim.setBool("atacar", false);

    }
}
