using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//PARA USAR ESTE SCRIPT HAY QUE HACER LO SIGUIENTE:
     // Atacharlo sobre el chasis de un cañon.
       // El chasis debe tener como hijo el cañon en si, y debe llamarse "cannon".
       // Se invoca llamando al metodo "Disparar" y se le pasa el GameObject al cual queremos dispararle o a "DispararAVariosObjetivos" y se le pasa una lista de string
       // con los nombres de los gameobjects objetivos.

public class Disparo
{
	private GameObject objetivo;
	private float offsetx;
	private float offsety;
	private bool objetivoEsPos;
   

	public Disparo (GameObject objetivo, float offsetx, float offsety, bool esPos){
		this.objetivo = objetivo;
		this.offsetx = offsetx;
		this.offsety = offsety;
		this.objetivoEsPos = esPos;      
	}

	public GameObject Objetivo {
		get {
			return this.objetivo;
		}
		set {
			objetivo = value;
		}
	}

	public float Offsetx {
		get {
			return this.offsetx;
		}
		set {
			offsetx = value;
		}
	}

	public float Offsety {
		get {
			return this.offsety;
		}
		set {
			offsety = value;
		}
	}

	public bool ObjetivoEsPos {
		get {
			return this.objetivoEsPos;
		}
		set {
			objetivoEsPos = value;
		}
	}
}
     
public class AnimacionCanon : MonoBehaviour
{
    //VARIABLES PARA EL MOVIMIENTO DEL CAÑON
    private GameObject objetivo;
    private Quaternion targetRotationChasis;
    private Quaternion targetRotationCannon;
    private Transform cannon;
    private AudioSource sonidosDelJuego;

    private float offsetx;
	private float offsety;

    //VARIABLES FLAGS
    private bool activarDisparo;
    private bool rotarChasis;
    private bool rotarCanon;
    private bool lanzarProyectil;

	// Esta variable indica que el objetivo se envio en forma de posicion
	// Asi el script sabe que debe destruir el "objetivo" falso despues de impactar
	private bool objetivoEsPos;

    //VARIABLES PARA LA TRAYECTORIA DEL PROYECTIL
    public float firingAngle = 30.0f;
    public float gravity = 9.8f;
    public Transform projectile;      
    private Transform myTransform;
	public Transform projectileExtra;

	public GameObject efectoDisparo;

	private ArrayList listaDisparos;
	private Disparo disparoActual;

	// Properties flags
	public bool esTorpedo;
	public bool centrado;
	public bool esIA;

	// Para avisarle que se termino la animacion
	private AdministradorPartida Administrador;
	private GameObject centroEnemigos;
	private GameObject centroJugador;

	private GameObject efectoFuego;
	private GameObject efectoExplosion;

    void Start()
    {
        sonidosDelJuego = GameObject.Find("SonidosDelJuego").GetComponents<AudioSource>()[1];
        this.activarDisparo = false;
        this.rotarChasis = true;
        this.rotarCanon = false;
        this.lanzarProyectil = false;
		this.objetivoEsPos = false;
		this.Administrador = GameObject.FindWithTag ("Administrador").GetComponent<AdministradorPartida>();
		this.centroEnemigos = GameObject.Find ("Markers/BarcosEnemigos");
		this.centroJugador = GameObject.Find ("Markers/BarcosJugador");
		this.efectoFuego = (GameObject)Resources.Load("Prefabs/Efectos/FlamesParticleEffectCheap", typeof(GameObject));
		this.efectoExplosion = (GameObject)Resources.Load("Prefabs/Efectos/BigExplosionEffectCheap", typeof(GameObject));
		this.offsetx = 0;
		this.offsety = 0;
		this.listaDisparos = new ArrayList ();
		this.disparoActual = null;
		this.gravity = this.gravity * 1.5f;

        this.myTransform = transform;
        this.projectile.position = myTransform.position;
        this.cannon = this.transform.Find("cannon");
    }

    void Update()
    {
		if (this.activarDisparo) 
        {
			// No seleccione un objetivo todavia
			if (this.disparoActual == null && listaDisparos.Count != 0) {
				this.disparoActual = (Disparo) this.listaDisparos [0];
				this.listaDisparos.RemoveAt (0);
				this.objetivo = this.disparoActual.Objetivo;
				this.offsetx = this.disparoActual.Offsetx;
				this.offsety = this.disparoActual.Offsety;
				this.objetivoEsPos = this.disparoActual.ObjetivoEsPos;
			}

			// Si ya tengo objetivo
			if (this.disparoActual != null) {
				if (this.rotarChasis) { 
					if (!centrado) {
						SetearRotacionHaciaElTarget (this.objetivo);
						this.transform.rotation = Quaternion.Slerp (transform.rotation, targetRotationChasis, Time.deltaTime * 3f);
					} else {
						if (!esIA)
							SetearRotacionHaciaElTarget (this.centroEnemigos);
						else
							SetearRotacionHaciaElTarget (this.centroJugador);
						this.transform.rotation = Quaternion.Slerp (transform.rotation, targetRotationChasis, Time.deltaTime * 3f);
					}
	                
					//Chequeamos si termino de rotar el cañon hacia el objetivo para poder disparar (comparamos contra un umbral de rotacion)
					if (Quaternion.Angle (this.transform.rotation, this.targetRotationChasis) < 2f) {
						//this.cannon.rotation = this.transform.rotation;
						this.rotarChasis = false;
						this.rotarCanon = true;
					}
				}

				//Movimiento para arriba/Abajo del cañon en si mismo
				if (this.rotarCanon) {
					this.targetRotationCannon = Quaternion.Euler (firingAngle, cannon.eulerAngles.y, cannon.eulerAngles.z);
					this.cannon.rotation = Quaternion.Slerp (cannon.rotation, this.targetRotationCannon, Time.deltaTime * 3f);
	                  
					//Chequeamos si termino de rotar el cañon hacia el objetivo para poder disparar (comparamos contra un umbral de rotacion)
					if (Quaternion.Angle (this.cannon.rotation, this.targetRotationCannon) < 2f) {
						this.rotarCanon = false;
						this.lanzarProyectil = true;
					}
				}

				if (this.lanzarProyectil) { 
					this.activarDisparo = false;
					//Lanzamos la rutina que lanza el proyectil
					StartCoroutine (SimulateProjectile (this.objetivo,this.objetivoEsPos,listaDisparos.Count == 0));
					StartCoroutine (WaitForNext ());
				}
			}
        }
    }

    //Seteamos la rotacion del cañon completo en el eje "y" hacia objetivo
    private void SetearRotacionHaciaElTarget(GameObject objetivo)
    {
		Vector3 aux = new Vector3(objetivo.transform.position.x,0,objetivo.transform.position.z);
        this.targetRotationChasis = Quaternion.LookRotation(transform.position - aux, Vector3.up);
        this.targetRotationChasis.z = 0;
        this.targetRotationChasis.x = 0;
    }

	IEnumerator WaitForNext(){
		yield return new WaitForSeconds (1f);

		this.lanzarProyectil = false;
		this.rotarChasis = true;
		this.disparoActual = null;

		// Si ya termine todos los disparos, dejo de disparar
		if (listaDisparos.Count != 0) {
			this.activarDisparo = true;
		}
	}

	IEnumerator WaitToFire(){
		yield return new WaitForSeconds (2f);
		this.activarDisparo = true;
	}

    //Rutina que lanza el proyectil hacia el target
	IEnumerator SimulateProjectile(GameObject objetivo, bool esPos, bool esFinal)
    {
		GameObject effect;
        // Short delay added before Projectile is thrown
		yield return new WaitForSeconds(0.5f);

		//sonidosDelJuego.Play();
		if (!esIA){
			Administrador.AudioManager.PlayAudioPoint("Sonidos/Grenade Explosion", cannon.position, 500);
		} else {
			Administrador.AudioManager.PlayAudioPoint("Sonidos/Grenade Explosion", cannon.position, 900);
		}
        Transform projectile2 = Transform.Instantiate (projectile);

		// Creo una copia del objetivo, para poder lanzar mas proyectiles antes de impactar
		GameObject objetivo2 = new GameObject ();
		GameObject objetivoAux = null;
		objetivo2.transform.position = objetivo.transform.position;
		if (esTorpedo) {
			objetivoAux = new GameObject ();
			objetivoAux.transform.position = objetivo2.transform.position;
			objetivo2.transform.position = new Vector3 (cannon.position.x, objetivo2.transform.position.y, cannon.position.z);
			objetivo2.transform.position = Vector3.MoveTowards (objetivo2.transform.position,objetivo.transform.position, 20);
		}

		// Si el objetivo se envio en forma de posicion, elimino el objetivo falso
		if (esPos)
			GameObject.Destroy (objetivo);
		
		GameObject efecto = GameObject.Instantiate (efectoDisparo,cannon.position,Quaternion.Euler(firingAngle+180, cannon.eulerAngles.y, cannon.eulerAngles.z));
		efecto.transform.localPosition = new Vector3 (efecto.transform.localPosition.x+offsetx, efecto.transform.localPosition.y + offsety, efecto.transform.localPosition.z);
		projectile2.gameObject.SetActive (true);
       
        // Move projectile to the position of throwing object + add some offset if needed.
		projectile2.position = cannon.position + new Vector3(0, 0.0f, 0);
		projectile2.localPosition = new Vector3 (projectile2.localPosition.x, projectile2.localPosition.y + offsetx, projectile2.localPosition.z + offsety);
       
        // Calculate distance to target
        float target_Distance = Vector3.Distance(projectile2.position, objetivo2.transform.position);
 
        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
 
        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
 
        // Calculate flight time.
        float flightDuration = target_Distance / Vx;
   
        // Rotate projectile to face the target.
        projectile2.rotation = Quaternion.LookRotation(objetivo2.transform.position - projectile2.position);
       
        float elapse_time = 0;
 
        while (elapse_time < flightDuration)
        {
            projectile2.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
           
            elapse_time += Time.deltaTime;
 
            yield return null;
        }

		if (!esPos && !esTorpedo) {
			if (objetivo.GetComponent<BarcoFisico> ().Hundido) {
				Administrador.MensajeBot.DisplayMessage ("¡Hundido!", 2f, 200);
				objetivo.GetComponent<BarcoFisico> ().AnimTermino = true;
                if (Administrador.EsFinDePartida())
                    Administrador.finalizarPartida();
			} else {
				Administrador.MensajeBot.DisplayMessage ("¡Averiado!", 2f, 200);
			}
		}

		if (esFinal == true && !esTorpedo) {
			this.Administrador.FinalizoAnimacion ();
		}

		if (esTorpedo) {
			//StartCoroutine (SimulateTorpedo (objetivo2,objetivoAux,projectile2.position));
			// Codigo para torpedo

			Transform projectile4 = Transform.Instantiate (projectileExtra);
			GameObject.Destroy (objetivo2);
			projectile4.gameObject.SetActive (true);
			projectile4.position = new Vector3(projectile2.position.x,1.5f,projectile2.position.z);
			projectile4.rotation = Quaternion.LookRotation (objetivoAux.transform.position - projectile4.position);

			float speed = 0.02f;
			while (Vector3.Distance (projectile4.position, objetivoAux.transform.position) > 10) {
				speed += 0.015f;
				if (speed > 2)
					speed = 2;
				projectile4.position = Vector3.MoveTowards (projectile4.position, objetivoAux.transform.position, speed);
				yield return null;
			}

			if (!esPos) {
				if (objetivo.GetComponent<BarcoFisico> ().Hundido) {
					objetivo.GetComponent<BarcoFisico> ().AnimTermino = true;
					Administrador.MensajeBot.DisplayMessage ("¡Hundido!", 2f, 200);
                    if (Administrador.EsFinDePartida())
                        Administrador.finalizarPartida();
                } else {
					Administrador.MensajeBot.DisplayMessage ("¡Averiado!", 2f, 200);
				}
			}

			this.Administrador.FinalizoAnimacion ();

			if (!esPos) {
				effect = GameObject.Instantiate (efectoFuego);
				effect.SetActive (true);
				if (esIA)
					effect.transform.localScale = new Vector3 (effect.transform.localScale.x * 0.5f, effect.transform.localScale.y * 0.5f, effect.transform.localScale.z * 0.5f);
				effect.transform.position = objetivoAux.transform.position;
				effect = GameObject.Instantiate (efectoExplosion);
				if (!esIA){
					Administrador.AudioManager.PlayAudioPoint("Sonidos/bomb_explosion_1", objetivoAux.transform.position, 800);
				} else {
					Administrador.AudioManager.PlayAudioPoint("Sonidos/bomb_explosion_1", objetivoAux.transform.position, 1800);
				}
				effect.SetActive (true);
				if (esIA)
					effect.transform.localScale = new Vector3 (effect.transform.localScale.x * 0.5f, effect.transform.localScale.y * 0.5f, effect.transform.localScale.z * 0.5f);
				effect.transform.position = objetivoAux.transform.position;
			}
			GameObject.Destroy (projectile4.gameObject);

		} else {
			if (!esPos) {
				effect = GameObject.Instantiate (efectoFuego);
				effect.SetActive (true);
				if (esIA)
					effect.transform.localScale = new Vector3 (effect.transform.localScale.x * 0.5f, effect.transform.localScale.y * 0.5f, effect.transform.localScale.z * 0.5f);
				effect.transform.position = objetivo2.transform.position;
				effect = GameObject.Instantiate (efectoExplosion);
				if (!esIA){
					Administrador.AudioManager.PlayAudioPoint("Sonidos/bomb_explosion_1", objetivo2.transform.position, 800);
				} else {
					Administrador.AudioManager.PlayAudioPoint("Sonidos/bomb_explosion_1", objetivo2.transform.position, 1800);
				}
				effect.SetActive (true);
				if (esIA)
					effect.transform.localScale = new Vector3 (effect.transform.localScale.x * 0.5f, effect.transform.localScale.y * 0.5f, effect.transform.localScale.z * 0.5f);
				effect.transform.position = objetivo2.transform.position;
			}
		}
		yield return new WaitForSeconds(2);

		GameObject.Destroy(projectile2.gameObject);
		GameObject.Destroy(objetivoAux);
		GameObject.Destroy(objetivo2);
    }  

	public void Disparar(GameObject objetivo, float offsetx = 0, float offsety = 0)
    {
		// Si no estoy disparando, empezar a disparar
		Disparo aux = new Disparo(objetivo,offsetx,offsety,false);
		listaDisparos.Add (aux);
		StartCoroutine (WaitToFire ());
    }

	public void Disparar(string objetivo, float offsetx = 0, float offsety = 0)
    {
        GameObject objetivo2 = GameObject.Find(objetivo);
		if (!objetivo2)
			return;
		Disparo aux = new Disparo(objetivo2,offsetx,offsety,false);
		listaDisparos.Add (aux);
		StartCoroutine (WaitToFire ());
    }

	public void Disparar(Vector3 pos, float offsetx = 0, float offsety = 0)
	{
		
		GameObject objetivo2 = new GameObject ();
		// Si el cañon es de la IA, uso la posicion enviada por parametro
		// Si es del jugador, uso una posicion aleatoria
		if (esIA) {
			objetivo2.transform.position = pos;
		} else {
			objetivo2.transform.position = new Vector3 (centroEnemigos.transform.position.x + Random.Range (-200, 200), centroEnemigos.transform.position.y, centroEnemigos.transform.position.z - 50);
		}
		Disparo aux = new Disparo(objetivo2,offsetx,offsety,true);
		listaDisparos.Add (aux);
		StartCoroutine (WaitToFire ());
	}

	public void Disparar(float offsetx = 0, float offsety = 0) // Posicion Random
	{
		Vector3 pos;
		if (esIA) {
			pos = new Vector3 (centroJugador.transform.position.x + Random.Range (-100, 100), centroJugador.transform.position.y, centroJugador.transform.position.z + 100);
		} else {
			pos = new Vector3 (centroEnemigos.transform.position.x + Random.Range (-200, 200), centroEnemigos.transform.position.y, centroEnemigos.transform.position.z - 50);
		}

		GameObject objetivo2 = new GameObject ();
		objetivo2.transform.position = pos;
		Disparo aux = new Disparo(objetivo2,offsetx,offsety,true);
		listaDisparos.Add (aux);
		StartCoroutine (WaitToFire ());
	}

    public void DispararAVariosObjetivos(string[] objetivos)
    { 
        foreach(string objetivo in objetivos)
        {
            Disparar(GameObject.Find(objetivo));
            //TODO: Agregar un tiempo de espera hasta que termine cada disparo
        }
    }

}
