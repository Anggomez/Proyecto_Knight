// Esta clase esta dirigida a asignar la IA al enemigo Minotauro
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class comportamientoMinotauro : MonoBehaviour
{
    #region Variables Publicas
    public float distanciaAtaque; //Distancia minima para atacar
    public float velocidadMovimiento; // Asigna velocidad
    public float contador; //Timer para el coolDown
    public Transform limiteIzquierdo; // Delimitador izquierdo
    public Transform limiteDerecho; // Delimitador derecho
    [HideInInspector] public Transform tr; // Transform
    [HideInInspector] public bool enRango; // Comprueba si esta en rango
    public GameObject minotauro; // Referencia al propio objeto
    public GameObject hotZone; // Zona en la que el enemigo te sigue
    public GameObject areaActivacion; // Area en la que detecta
    public BoxCollider2D hachaCollider; // Collider del hacha

    #endregion

    #region Variables Privadas
    private Animator anim; // Objeto animator
    private float distancia; //Guarda distancia Enemigo-Jugador
    private bool modoAtaque;
    private bool recibeDmg=true;
    private bool coolDown; // Cooldown del ataque
    private float intContador;
    private bool puedeHacerDmg = true;
    [SerializeField] private float cantidadSaludTotal;
    private float cantidadSaludRestante;
    [SerializeField] private GameObject canvas;
    private CotrolesJugador controlesJugador;
    private bool estaVivo = true;

    #endregion

    #region metodos de unity
    void Start() // Se ejecuta al lanzarse
    {
        SeleccionarObjetivo();
        intContador = contador; // Guarda el primer valor del contador
        cantidadSaludRestante = cantidadSaludTotal;
        anim = GetComponent<Animator>();
        controlesJugador = GameObject.Find("Caballero").GetComponent<CotrolesJugador>();
        //canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
    }
    
    void Update() // Se ejecuta en cada frame
    {
        if (estaVivo)
        {
            if (!modoAtaque)
            {
                Mover();
            }

            if (!dentroLimites() && !enRango && !anim.GetCurrentAnimatorStateInfo(0).IsName("Minotauro_ataque"))
            {
                SeleccionarObjetivo();
            }

            if (enRango)
            {
                logicaEnemigo();
            }
        }

    }
    #endregion

    #region comprobaciones
    void logicaEnemigo()  // Logica del enemigo
    {
        distancia = Vector2.Distance(transform.position, tr.position);

        if (distancia > distanciaAtaque) // Si no esta a rango para de atacar
        {
            PararAtaque();
        }
        else if (distanciaAtaque >= distancia && coolDown == false) // Si esta en rango y puede atacar ataca
        {
            atacar();
        }

        if (coolDown) // si no puede atacar, resetea el ataque
        {
            Cooldown();
            anim.SetBool("Ataque", false);
        }
    }

    #endregion

    #region acciones
    void Mover() // Mueve al personaje
    {
        anim.SetBool("Andar", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Minotauro_ataque") && estaVivo)
        {
            Vector2 targetPosition = new Vector2(tr.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, velocidadMovimiento * Time.deltaTime);
        }
    }

    void atacar() // Indica que ataque al enemigo
    {
        contador = intContador; // Resetea el contador
        modoAtaque = true; // comprueba si el enemigo sigue atacando

        anim.SetBool("Andar", false);
        anim.SetBool("Ataque", true);
    }

    void Cooldown() // reduce el cooldown
    {
        contador -= Time.deltaTime;

        if (contador <= 0 && coolDown && modoAtaque)
        {
            coolDown = false;
            contador = intContador;
        }
    }

    void PararAtaque() // Para el ataque e inicia el cooldown
    {
        coolDown = false;
        modoAtaque = false;
        anim.SetBool("Ataque", false);
    }

    public void activarCooldown() // Se lanza desde animacion para resetear ooldown
    {
        coolDown = true;
    }

    private bool dentroLimites()
    {
        return transform.position.x > limiteIzquierdo.position.x && transform.position.x < limiteDerecho.position.x;
    }

    public void SeleccionarObjetivo() // Asigna la direccion y rotacion
    {
        float distanciaIzquierda = Vector3.Distance(transform.position, limiteIzquierdo.position);
        float distanciaDerecha = Vector3.Distance(transform.position, limiteDerecho.position);

        if (distanciaIzquierda > distanciaDerecha)
        {
            tr = limiteIzquierdo;
        }
        else
        {
            tr = limiteDerecho;
        }

        // Se podria cambiar el if por un operador ternario, pero asi es mas legible
        //target = distanceToLeft > distanceToRight ? leftLimit : rightLimit;

        Rotar();
    }

    public void Rotar()// intercambia la direccion
    {
        Vector3 rotacion = transform.eulerAngles;
        if (transform.position.x > tr.position.x)
        {
            rotacion.y = 180;
        }
        else
        {
            rotacion.y = 0;
        }

        //Tambien se podria poner operador ternario aqui
        //rotation.y = (currentTarget.position.x < transform.position.x) ? rotation.y = 180f : rotation.y = 0f;

        transform.eulerAngles = rotacion;
    }
    public void daniar(float cantidadDano)
    {
        cantidadSaludRestante -= cantidadDano;
        Debug.Log(cantidadSaludRestante);
        if (cantidadSaludRestante <= 0.0f && recibeDmg)
        {
            estaVivo = false;
            morir();
        }
    }
    private void morir() // desactiva el psj
    {
        recibeDmg = false;
        anim.SetBool("estaMuerto", true);
        hotZone.SetActive(false);
        areaActivacion.SetActive(false);
        canvas.SendMessage("aumentarContadorMinotauros");
    }
    private void morirDeltodo()
    {
        anim.SetBool("muertisimo", true);
        anim.SetBool("estaMuerto", false);
    }
    private void comprobarHitBoxAtaqueM() //comprueba si ha golpeado al jugador
    {
        if (hachaCollider.IsTouching(controlesJugador.colliderJugador)){

            controlesJugador.SendMessage("recibirDmg", 10);
        }
     }
    #endregion
}
