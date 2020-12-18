using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CotrolesJugador : MonoBehaviour
{
    #region variables privadas
    private float direccionEntradaMovimiento, temporizadorDelSalto, TemporizadorCambio, temporizadorSaltoPared, tiempoRestanteDash, ultimaImagenXpos, ultimoDash = -100f;

    private int cantidadSaltosRestantes, DireccionApuntadoPersonaje = 1, direccionUltimoSalto;

    private bool estaMirandoALaDerecha = true, estaAndando, tocaSuelo, tocaPared, estaDeslizandose, puedeSaltarNormal, puedeSaltarEnMuro, estaIntentandoSaltar,
                 comprobarSaltoMultiplicador, puedeMoverse, puedeVoltearse, haSaltadoPared, estaDasheando;

    private Rigidbody2D rigidbody;

    private Animator animador;
    //private bool isTouchingLedge;
    //private bool canClimbLedge = false;
    //private bool ledgeDetected;
    //private Vector2 ledgePosBot;
    //private Vector2 ledgePos1;
    //private Vector2 ledgePos2;
    #endregion
    #region variables publicas;
    public int cantidadSaltos = 1;
    public float cantidadVida=1000;
    public float velocidadMovimiento = 10.0f;
    public float fuerzaSalto = 16.0f;
    public float radioComprobacionPared;
    public float comprobarDistanciaPared;
    public float velocidadDeslizamientoPared;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float MultiplicadorVariableSaltoPared = 0.5f;
    public float FuerzaDejarCaerDePared;
    public float fuerzaSaltoPared;
    public float ajustadorTemporizadorSalto = 0.15f;
    public float ajustadorTemporizadorVoltear = 0.1f;
    public float ajustadorSaltoPared = 0.5f;
    public float tiempoDash;
    public float velocidadDash;
    public float distanciaEntreImagenes;
    public float CooldownDash;
    public Collider2D colliderJugador;

    public Vector2 direccionDejarCaerDePared, direccionSaltoPared;
    public Transform ColliderGO;

    public Transform comprobarSuelo;
    public Transform comprobarPared;
    
    public LayerMask queEsSuelo;
    //public float ledgeClimbXOffset1 = 0f;
    //public float ledgeClimbYOffset1 = 0f;
    //public float ledgeClimbXOffset2 = 0f;
    //public float ledgeClimbYOffset2 = 0f;
    //public Transform ledgeCheck;
    #endregion
    #region metodos de unity
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animador = GetComponent<Animator>();
        cantidadSaltosRestantes = cantidadSaltos;
        direccionDejarCaerDePared.Normalize();
        direccionSaltoPared.Normalize();
    }

    void Update()
    {
        comprobarEntradas();
        comprobarDireccionMovimiento();
        ActualizarAnimaciones();
        comprobarSiPuedeSaltar();
        comprobarDeslizarPared();
        comprobarSalto();
        comprobarDash();
        //CheckLedgeClimb();
    }

    private void FixedUpdate()
    {
        AplicarMovimiento();
        comprobarAlrededores();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(comprobarSuelo.position, radioComprobacionPared);

        Gizmos.DrawLine(comprobarPared.position, new Vector3(comprobarPared.position.x + comprobarDistanciaPared, comprobarPared.position.y, comprobarPared.position.z));
    }
#endregion
    private void comprobarDeslizarPared()
    {
        if (tocaPared && direccionEntradaMovimiento == DireccionApuntadoPersonaje && rigidbody.velocity.y < 0)//&& !canClimbLedge)
        {
            estaDeslizandose = true;
        }
        else
        {
            estaDeslizandose = false;
        }
    }

    //private void CheckLedgeClimb()
    //{
    //    if (ledgeDetected && !canClimbLedge)
    //    {
    //        canClimbLedge = true;

    //        if (isFacingRight)
    //        {
    //            ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
    //            ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
    //        }
    //        else
    //        {
    //            ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
    //            ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
    //        }

    //        canMove = false;
    //        canFlip = false;

    //        anim.SetBool("canClimbLedge", canClimbLedge);
    //    }

    //    if (canClimbLedge)
    //    {
    //        transform.position = ledgePos1;
    //    }
    //}

    //public void FinishLedgeClimb()
    //{
    //    canClimbLedge = false;
    //    transform.position = ledgePos2;
    //    canMove = true;
    //    canFlip = true;
    //    ledgeDetected = false;
    //    anim.SetBool("canClimbLedge", canClimbLedge);
    //}

    private void comprobarAlrededores()
    {
        tocaSuelo = Physics2D.OverlapCircle(comprobarSuelo.position, radioComprobacionPared, queEsSuelo);

        tocaPared = Physics2D.Raycast(comprobarPared.position, transform.right, comprobarDistanciaPared, queEsSuelo);
        //isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);

        //if (isTouchingWall && !isTouchingLedge && !ledgeDetected)
        //{
        //    ledgeDetected = true;
        //    ledgePosBot = wallCheck.position;
        //}
    }

    private void comprobarSiPuedeSaltar()
    {
        if (tocaSuelo && rigidbody.velocity.y <= 0.01f)
        {
            cantidadSaltosRestantes = cantidadSaltos;
        }

        if (tocaPared)
        {
            comprobarSaltoMultiplicador = false;
            puedeSaltarEnMuro = true;
        }

        if (cantidadSaltosRestantes <= 0)
        {
            puedeSaltarNormal = false;
        }
        else
        {
            puedeSaltarNormal = true;
        }

    }

    private void comprobarDireccionMovimiento()
    {
        if (estaMirandoALaDerecha && direccionEntradaMovimiento < 0)
        {
            Voltear();
        }
        else if (!estaMirandoALaDerecha && direccionEntradaMovimiento > 0)
        {
            Voltear();
        }

        if (Mathf.Abs(rigidbody.velocity.x) >= 0.01f)
        {
            estaAndando = true;
        }
        else
        {
            estaAndando = false;
        }
    }

    private void ActualizarAnimaciones()
    {
        animador.SetBool("estaEscalando", estaAndando);
        animador.SetBool("estaEnSuelo", tocaSuelo);
        animador.SetFloat("velocidadY", rigidbody.velocity.y);
        animador.SetBool("estaDeslizandose", estaDeslizandose);
    }

    private void comprobarEntradas()
    {
        direccionEntradaMovimiento = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (tocaSuelo || (cantidadSaltosRestantes > 0 && !tocaPared))
            {
                SaltoNormal();
            }
            else
            {
                temporizadorDelSalto = ajustadorTemporizadorSalto;
                estaIntentandoSaltar = true;
            }
        }

        if (Input.GetButtonDown("Horizontal") && tocaPared)
        {
            if (!tocaSuelo && direccionEntradaMovimiento != DireccionApuntadoPersonaje)
            {
                puedeMoverse = false;
                puedeVoltearse = false;

                TemporizadorCambio = ajustadorTemporizadorVoltear;
            }
        }

        if (TemporizadorCambio >= 0)
        {
            TemporizadorCambio -= Time.deltaTime;

            if (TemporizadorCambio <= 0)
            {
                puedeMoverse = true;
                puedeVoltearse = true;
            }
        }

        if (comprobarSaltoMultiplicador && !Input.GetButton("Jump"))
        {
            comprobarSaltoMultiplicador = false;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y * MultiplicadorVariableSaltoPared);
        }

        if (Input.GetButtonDown("Dash"))
        {
            if (Time.time >= (ultimoDash + CooldownDash))
                intentarDashear();
        }

    }

    private void intentarDashear()
    {
        estaDasheando = true;
        tiempoRestanteDash = tiempoDash;
        ultimoDash = Time.time;

        ImagenDeDespuesPool.Instancia.sacarDelPool();
        ultimaImagenXpos = transform.position.x;
    }

    public int getDireccionApuntado()
    {
        return DireccionApuntadoPersonaje;
    }

    private void comprobarDash()
    {
        if (estaDasheando)
        {
            if (tiempoRestanteDash > 0)
            {
                puedeMoverse = false;
                puedeVoltearse = false;
                rigidbody.velocity = new Vector2(velocidadDash * DireccionApuntadoPersonaje, 0.0f);
                tiempoRestanteDash -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - ultimaImagenXpos) > distanciaEntreImagenes)
                {
                    ImagenDeDespuesPool.Instancia.sacarDelPool();
                    ultimaImagenXpos = transform.position.x;
                }
            }

            if (tiempoRestanteDash <= 0 || tocaPared)
            {
                estaDasheando = false;
                puedeMoverse = true;
                puedeVoltearse = true;
            }

        }
    }

    private void comprobarSalto()
    {
        if (temporizadorDelSalto > 0)
        {
            //WallJump
            if (!tocaSuelo && tocaPared && direccionEntradaMovimiento != 0 && direccionEntradaMovimiento != DireccionApuntadoPersonaje)
            {
                SaltoPared();
            }
            else if (tocaSuelo)
            {
                SaltoNormal();
            }
        }

        if (estaIntentandoSaltar)
        {
            temporizadorDelSalto -= Time.deltaTime;
        }

        if (temporizadorSaltoPared > 0)
        {
            if (haSaltadoPared && direccionEntradaMovimiento == -direccionUltimoSalto)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0.0f);
                haSaltadoPared = false;
            }
            else if (temporizadorSaltoPared <= 0)
            {
                haSaltadoPared = false;
            }
            else
            {
                temporizadorSaltoPared -= Time.deltaTime;
            }
        }
    }

    private void SaltoNormal()
    {
        if (puedeSaltarNormal)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, fuerzaSalto);
            cantidadSaltosRestantes--;
            temporizadorDelSalto = 0;
            estaIntentandoSaltar = false;
            comprobarSaltoMultiplicador = true;
        }
    }

    private void SaltoPared()
    {
        if (puedeSaltarEnMuro)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0.0f);
            estaDeslizandose = false;
            cantidadSaltosRestantes = cantidadSaltos;
            cantidadSaltosRestantes--;
            Vector2 forceToAdd = new Vector2(fuerzaSaltoPared * direccionSaltoPared.x * direccionEntradaMovimiento, fuerzaSaltoPared * direccionSaltoPared.y);
            rigidbody.AddForce(forceToAdd, ForceMode2D.Impulse);
            temporizadorDelSalto = 0;
            estaIntentandoSaltar = false;
            comprobarSaltoMultiplicador = true;
            TemporizadorCambio = 0;
            puedeMoverse = true;
            puedeVoltearse = true;
            haSaltadoPared = true;
            temporizadorSaltoPared = ajustadorSaltoPared;
            direccionUltimoSalto = -DireccionApuntadoPersonaje;

        }
    }

    private void AplicarMovimiento()
    {

        if (!tocaSuelo && !estaDeslizandose && direccionEntradaMovimiento == 0)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x * airDragMultiplier, rigidbody.velocity.y);
        }
        else if (puedeMoverse)
        {
            rigidbody.velocity = new Vector2(velocidadMovimiento * direccionEntradaMovimiento, rigidbody.velocity.y);
        }


        if (estaDeslizandose)
        {
            if (rigidbody.velocity.y < -velocidadDeslizamientoPared)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, -velocidadDeslizamientoPared);
            }
        }
    }

    public void desabilitarVolteo()
    {
        puedeVoltearse = false;
    }

    public void HabilitarVolteo()
    {
        puedeVoltearse = true;
    }

    private void Voltear()
    {
        if (!estaDeslizandose && puedeVoltearse)
        {
            DireccionApuntadoPersonaje *= -1;
            estaMirandoALaDerecha = !estaMirandoALaDerecha;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }
    private void morir()
    {
        SceneManager.LoadScene("main");
    }
    private void recibirDmg(float dmg)
    {
        cantidadVida -= dmg;
        if(cantidadVida <= 0)
        {
            morir();
        }
    }
}