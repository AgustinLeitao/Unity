using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteligenciaArtificial {

    private System.Random randomNumber;
    private Casilla casillaResultado;
    private Casilla ultimaCasillaAtacada;
    private Casilla primeraCasillaAtacada;
    private AdministradorPartida Administrador;
	private Jugador jugadorIA;
	private Jugador jugadorH;
    private Tablero tableroAtaqueDeIA;
    private Tablero tableroPosicionJugador;
    List<Casilla> casillasAdyacentes;
    private bool acierto;

	private Habilidad habilidad;
	private int habilidadParaVibracion;

    private enum Modo {Normal=0, ConHabilidades=1};
    private enum Dificultad {Facil=1, Normal=2, Dificil=3};
    private enum ResultadoDelAtaque {Fallo=0, Acierto=1};
	private enum Direccion {Izquierda=0, Derecha=1, Arriba=2, Abajo=3};

	private int direccionActual;
    private int modoSeleccionado;
    private int DificultadSeleccionada;
    private Casilla proximaCasillaParaAtacar;

    private bool ATACAR_ADYACENTES;
    private bool ATACAR_EN_DIRECCION;
    private bool ATACAR_NUEVO_OBJETIVO;

    private bool [] habilidadesDisponibles;

	public InteligenciaArtificial(Jugador jugadorIA, Jugador jugadorH, int dificutad, int modo)
    {
        this.randomNumber = new System.Random();
		this.Administrador = GameObject.FindWithTag ("Administrador").GetComponent<AdministradorPartida>();
        this.DificultadSeleccionada = dificutad;
        this.modoSeleccionado = modo;
        this.ultimaCasillaAtacada = null;
        this.primeraCasillaAtacada = null;
        this.proximaCasillaParaAtacar = null;
        this.casillasAdyacentes = new List<Casilla>();
        this.ATACAR_ADYACENTES = false;
        this.ATACAR_EN_DIRECCION = false;
        this.ATACAR_NUEVO_OBJETIVO = true;
        this.acierto = false;
		this.jugadorIA = jugadorIA;
		this.jugadorH = jugadorH;
		this.tableroAtaqueDeIA = jugadorIA.TableroAtaque;
		this.tableroPosicionJugador = jugadorH.TableroPosicion;
        this.habilidadesDisponibles = new bool[11];
        for (int i = 0; i < habilidadesDisponibles.Length; i++) {
			habilidadesDisponibles[i] = false;
		}
    }


//TODO: Agregar un delay antes de cada ataque la IA
    public void Atacar()
    {
        Debug.Log("La dificultad es "+this.DificultadSeleccionada);
        int resultado;
        switch (this.DificultadSeleccionada)
        {
		case (int) Dificultad.Facil:
                //do
                //{
			resultado = AtaqueConDificultadFacil ();
			ValidarVibracionCamara (resultado);
                //}
                //while (resultado == (int) ResultadoDelAtaque.Acierto);
                break;

		    case (int) Dificultad.Normal:
                //do
                //{ 
            resultado = AtacarConDificultadMedia ();	
			ValidarVibracionCamara (resultado);
                //} while(resultado == (int) ResultadoDelAtaque.Acierto);
                break;
                
            case (int) Dificultad.Dificil:
                resultado = AtacarConDificultadMedia();
                ValidarVibracionCamara(resultado);
                break;

            default:
                break;
        }
    }

	private void ValidarVibracionCamara (int resultado) {
		if (resultado == 1) {
			Casilla [] casillaArray = new Casilla[1];
			casillaArray[0] = this.ultimaCasillaAtacada;
			Administrador.ActivarVibracion (casillaArray, -1);
		} else if (resultado == -1) {
			Administrador.ActivarVibracion(habilidad.listaCasillas, this.habilidadParaVibracion);
		}
	}
		
    private bool PuedeUsarHabilidad(int habilidaID)
    {
        habilidad = null;
        switch (habilidaID) {
		case Constantes.ARTILLERIA_RAPIDA_ID:
                habilidad = new Habilidad_ArtilleriaRapida(this.Administrador);
                break;
		case Constantes.PROYECTIL_HE_ID:
                habilidad = new Habilidad_ProyectilHE(this.Administrador);
                break;
		case Constantes.TORPEDO_ID:
                habilidad = new Habilidad_Torpedo(this.Administrador);
                break;
		case Constantes.DISPARO_DOBLE_ID:
                habilidad = new Habilidad_DisparoDoble(this.Administrador);
                break;
		case Constantes.REFORZAR_ARMADURA_ID:
                habilidad = new Habilidad_ReforzarArmadura(this.Administrador);
                break;
		case Constantes.TORMENTA_MISILES_ID:
                habilidad = new Habilidad_TormentaDeMisiles(this.Administrador);
                break;
		case Constantes.PULSO_ELECTROMAGNETICO_ID:
                habilidad = new Habilidad_PulsoElectromagnetico(this.Administrador);
                break;
		case Constantes.SABOTAJE_ID:
                habilidad = new Habilidad_Sabotaje(this.Administrador);
                break;
        }

        if (jugadorIA.puedeAñadirHabilidad(habilidad) == -1)
            return false;
        return true;
    }

    private int GenerarNumeroDeHabilidadRandom()
    {
        int habilidad = -1;
        /*
        if (jugadorH.PosicionesDisponibles >= 3) 
        {
            do { habilidad = randomNumber.Next(1, 11); }
            while (habilidad == 8 || habilidad == 9 || ! PuedeUsarHabilidad(habilidad));
        } else if(jugadorH.PosicionesDisponibles == 2 ) 
        {
            do { habilidad = randomNumber.Next(1, 8); }
            while (! PuedeUsarHabilidad(habilidad));
        } else if (jugadorH.PosicionesDisponibles == 1) 
        {
            do { habilidad = randomNumber.Next(1, 8); }
            while (habilidad == 4 || ! PuedeUsarHabilidad(habilidad));
        }
        */
        ActualizarHabilidadesDisponibles();
        habilidad = GetIndiceHabilidadDisponible();
		this.habilidadParaVibracion = habilidad;
        Debug.Log("HABILIDAD GENERADA RANDOM: " + habilidad);
        return habilidad;
    }

    public int GetIndiceHabilidadDisponible(){
		int[] indices = new int[habilidadesDisponibles.Length];
		int count = 0;
		for (int i = 1; i < habilidadesDisponibles.Length; i++) {
			if (habilidadesDisponibles[i] && PuedeUsarHabilidad(i)) {
				indices [count] = i;
				count++;
			}
		}

		if (count > 0) {
			int ran = UnityEngine.Random.Range (0, count);
			return indices [ran];
		} else {
			return -1;
		}
	}

    public void ActualizarHabilidadesDisponibles(){
		for (int i = 0; i < habilidadesDisponibles.Length; i++) {
			habilidadesDisponibles[i] = false;
		}
        for (int i = 0; i < jugadorIA.ListaBarcos.Length; i++) {
			if (jugadorIA.ListaBarcos[i].Vivo){

                switch (jugadorIA.ListaBarcos[i].GetTipoBarco()) {
                    case 2:
                            if (jugadorH.PosicionesDisponibles > 2)
                                habilidadesDisponibles[Constantes.SABOTAJE_ID] = true;
                            if (jugadorIA.PosicionesDisponibles > 1)
                                habilidadesDisponibles[Constantes.DISPARO_DOBLE_ID] = true;
                            break;
                    case 3:
                            habilidadesDisponibles[Constantes.TORPEDO_ID] = true;
                            habilidadesDisponibles[Constantes.REFORZAR_ARMADURA_ID] = true;
                            break;
                    case 4:
                            habilidadesDisponibles[Constantes.PROYECTIL_HE_ID] = true;
                            habilidadesDisponibles[Constantes.ARTILLERIA_RAPIDA_ID] = true;
                            break;
                    case 5:
                            habilidadesDisponibles[Constantes.PULSO_ELECTROMAGNETICO_ID] = true;
                            if (!jugadorIA.UsoTormentaMisiles && (jugadorIA.PosicionesDisponibles > 20))
                                habilidadesDisponibles[Constantes.TORMENTA_MISILES_ID] = true;
                            break;
                }

            }
		}
	}

    private int AtaqueConDificultadFacil() {        
        if(Administrador.HabilidadesActivadas && DeboUtilizarHabilidad(4) && this.modoSeleccionado == (int) Modo.ConHabilidades)
        {
            return EjecutarHabilidadEspecial(GenerarNumeroDeHabilidadRandom());
        }   
        else
        {
            return EjecutarAtaqueNormal();
        }
    }

    private int EjecutarAtaqueNormal() 
    {
        if (DificultadSeleccionada == (int)Dificultad.Dificil)
        {
            do
            {
                this.casillaResultado = GenerarPosicionRandom();
            } while (EsUnaPosicionAtacada(this.casillaResultado) && !EsUnaPosicionOcupada(this.casillaResultado));
            var randomNumber = UnityEngine.Random.Range(0, 3); // 33.3% de probabilidad para que ataque a una posicion ocupada
            if(randomNumber != 1)
            {
                do
                {
                    this.casillaResultado = GenerarPosicionRandom();
                } while (EsUnaPosicionAtacada(this.casillaResultado));
            }
        }
        else
        {
            do
            {
                this.casillaResultado = GenerarPosicionRandom();
            } while (EsUnaPosicionAtacada(this.casillaResultado));
        }

        int resultadoDelAtaque = -2;

        Debug.Log ("ATAQUE NORMAL: "+casillaResultado.Fila + "," + casillaResultado.Columna);
        resultadoDelAtaque = EjecutarAtaque(this.casillaResultado);
        if(resultadoDelAtaque == (int) ResultadoDelAtaque.Acierto) 
        {
            this.ultimaCasillaAtacada = casillaResultado;
            this.primeraCasillaAtacada = this.ultimaCasillaAtacada;
        }
        return resultadoDelAtaque;
    }

    private bool EsUnaPosicionOcupada(Casilla casillaResultado)
    {
        if (jugadorH.TableroPosicion.GetCasilla(casillaResultado.Fila, casillaResultado.Columna).HayBarco)
            return true;
        else
            return false;               
    }

    private Casilla BuscarNuevoAdyacente(){
		if (Administrador.UltimosHits.Count > 0) {
			Casilla cas;
			do {
				cas = (Casilla) Administrador.UltimosHits [0];
				Administrador.UltimosHits.RemoveAt (0);
				if (jugadorH.TableroPosicion.GetCasilla (cas.Fila, cas.Columna).Atacada && cas.Barco != null && cas.Barco.Vivo){
					ActualizarEstadoDeIA(atacarNuevoObjetivo: false, atacarAdyacentes: true, atacarEnDireccion: false);
					this.ultimaCasillaAtacada = cas;
					this.primeraCasillaAtacada = this.ultimaCasillaAtacada;
					return cas;
				}
			} while (Administrador.UltimosHits.Count > 0);
		}
		return null;
	}

	private int AtacarConDificultadMedia() {        
		if(Administrador.HabilidadesActivadas && DeboUtilizarHabilidad(2) && this.modoSeleccionado == (int) Modo.ConHabilidades && this.ATACAR_NUEVO_OBJETIVO)
        {
            return EjecutarHabilidadEspecial(GenerarNumeroDeHabilidadRandom());
        }   
        else
        {
			if (!ATACAR_ADYACENTES) {
				Casilla ady = BuscarNuevoAdyacente ();
			}
            return AtaqueConDificultadMedia() ? 1 : 0;
        }
    }


    private bool AtaqueConDificultadMedia() {
        int resultadoDelAtaque;

        //BUSCO UN NUEVO OBJETIVO HASTA ACERTAR A UN BARCO
        if(this.ATACAR_NUEVO_OBJETIVO)
        {
            resultadoDelAtaque = EjecutarAtaqueNormal();
            if(resultadoDelAtaque == (int) ResultadoDelAtaque.Acierto)
            {
                ActualizarEstadoDeIA(atacarNuevoObjetivo: false, atacarAdyacentes: true, atacarEnDireccion: false);
                if(! EsUnBarcoVivo(this.ultimaCasillaAtacada))
                    ActualizarEstadoDeIA(atacarNuevoObjetivo: true, atacarAdyacentes: false, atacarEnDireccion: false);
                return true;
            }
            else
            {
                ActualizarEstadoDeIA(atacarNuevoObjetivo: true, atacarAdyacentes: false, atacarEnDireccion: false);
                return false;
            }
        }
        //ENTRA AL IF SI LE ACERTE UNA POSICION (ENTRA HASTA DESCARTAR TODOS LOS ADYACENTES DE LA POSICION ACERTADA)
		else if(this.ATACAR_ADYACENTES) 
        {
            CargarListaAdyacentesValidos(this.primeraCasillaAtacada);
 
            if(this.casillasAdyacentes.Count > 0 && this.casillasAdyacentes[0] != null)
            {
                resultadoDelAtaque = EjecutarAtaque(this.casillasAdyacentes[0]);
                if(resultadoDelAtaque == (int) ResultadoDelAtaque.Acierto) 
                {
                    //SI AUN NO DERRIBE AL BARCO TENGO QUE DEFINIR LA DIRECCION DEL ATAQUE
                    this.ultimaCasillaAtacada = this.casillasAdyacentes[0];
                    this.direccionActual = DefinirDireccionDelAtaque(this.primeraCasillaAtacada, this.ultimaCasillaAtacada);
                    ActualizarEstadoDeIA(atacarNuevoObjetivo: false, atacarAdyacentes: false, atacarEnDireccion: true);
                    if(! EsUnBarcoVivo(this.ultimaCasillaAtacada))
                        ActualizarEstadoDeIA(atacarNuevoObjetivo: true, atacarAdyacentes: false, atacarEnDireccion: false);
                    return true;
                } 
                else 
                {
                    this.casillasAdyacentes.RemoveAt(0);
                    ActualizarEstadoDeIA(atacarNuevoObjetivo: false, atacarAdyacentes: true, atacarEnDireccion: false);
                    return false;
                }
            }
            else
            {
                 ActualizarEstadoDeIA(atacarNuevoObjetivo: true, atacarAdyacentes: false, atacarEnDireccion: false);
            }
                
        }

        //COMIENZO A ATACAR EN CIERTA DIRECCION -> HASTA QUE DERRIVE EL BARCO (SI FALLO O SI ME VOY DE LA MATRIZ TENGO QUE INTENTAR EN LA OTRA DIRECCION)
		else if(this.ATACAR_EN_DIRECCION) 
        {
			Casilla proximaCasilla = ObtenerProximaCasillaSegunDireccion (this.ultimaCasillaAtacada);
            resultadoDelAtaque = EjecutarAtaque(proximaCasilla);

			if(resultadoDelAtaque == (int) ResultadoDelAtaque.Acierto) 
			{
                this.ultimaCasillaAtacada = proximaCasilla;
				ActualizarEstadoDeIA(atacarNuevoObjetivo: false, atacarAdyacentes: false, atacarEnDireccion: true);
                if(! EsUnBarcoVivo(proximaCasilla))
                    ActualizarEstadoDeIA(atacarNuevoObjetivo: true, atacarAdyacentes: false, atacarEnDireccion: false);
                return true;
			}
			else
			{
				//SI TODAVIA NO DERRIBE EL BARCO CAMBIAR A LA DIRECCION CONTRARIA
				CambiarADireccionContraria();
                this.ultimaCasillaAtacada = primeraCasillaAtacada;	
                ActualizarEstadoDeIA(atacarNuevoObjetivo: false, atacarAdyacentes: false, atacarEnDireccion: true);
                return false;
			}
        }
        return false;
    }

    private bool EsUnBarcoVivo(Casilla casilla)
    { 
        return tableroPosicionJugador.GetCasilla(casilla.Fila, casilla.Columna).Barco.Vivo;
    }

    private void CambiarADireccionContraria()
	{
		if(this.direccionActual == (int) Direccion.Arriba)
			this.direccionActual = (int) Direccion.Abajo;
        else if(this.direccionActual == (int) Direccion.Abajo)
			this.direccionActual = (int) Direccion.Arriba;
        else if(this.direccionActual == (int) Direccion.Izquierda)
			this.direccionActual = (int) Direccion.Derecha;
		else if(this.direccionActual == (int) Direccion.Derecha)	
			this.direccionActual = (int) Direccion.Izquierda;
	}

	private Casilla ObtenerProximaCasillaSegunDireccion(Casilla ultimaCasillaAtacada)
	{	
		Casilla resultado = ObtenerNuevaCasillaSegunDireccion(ultimaCasillaAtacada);

		if (!EsUnaPosicionValida(resultado)) {
			CambiarADireccionContraria ();
			resultado = ObtenerNuevaCasillaSegunDireccion (primeraCasillaAtacada);
		}

		return resultado;
	}

	private Casilla ObtenerNuevaCasillaSegunDireccion(Casilla ultimaCasillaAtacada)
    {
		Casilla resultado = new Casilla(ultimaCasillaAtacada.Fila, ultimaCasillaAtacada.Columna);

		if(this.direccionActual == (int) Direccion.Arriba)
			resultado.setPosicion(ultimaCasillaAtacada.Fila - 1, ultimaCasillaAtacada.Columna);
		if(this.direccionActual == (int) Direccion.Abajo)
			resultado.setPosicion(ultimaCasillaAtacada.Fila + 1, ultimaCasillaAtacada.Columna);
		if(this.direccionActual == (int) Direccion.Izquierda)
			resultado.setPosicion(ultimaCasillaAtacada.Fila, ultimaCasillaAtacada.Columna - 1);
		if(this.direccionActual == (int) Direccion.Derecha)	
			resultado.setPosicion(ultimaCasillaAtacada.Fila, ultimaCasillaAtacada.Columna + 1);
		return resultado;
	}

    private int DefinirDireccionDelAtaque(Casilla casillaAtacadaAnterior, Casilla casillaAtacadaActual)
    {
        if(casillaAtacadaAnterior.Fila > casillaAtacadaActual.Fila)
			return (int) Direccion.Arriba;
		if(casillaAtacadaAnterior.Fila < casillaAtacadaActual.Fila)
			return (int) Direccion.Abajo;
		if(casillaAtacadaAnterior.Columna > casillaAtacadaActual.Columna)
			return (int) Direccion.Izquierda;
		if(casillaAtacadaAnterior.Columna < casillaAtacadaActual.Columna)
			return (int) Direccion.Derecha;
		return -1;
    }

    private void ActualizarEstadoDeIA(bool atacarNuevoObjetivo, bool atacarAdyacentes, bool atacarEnDireccion)
    {
        this.ATACAR_NUEVO_OBJETIVO = atacarNuevoObjetivo;
        this.ATACAR_ADYACENTES = atacarAdyacentes;
        this.ATACAR_EN_DIRECCION = atacarEnDireccion;
    }

    private void CargarListaAdyacentesValidos(Casilla casillaAtacada) 
    {
        this.casillasAdyacentes.Clear();

        Casilla casillaAdyacenteInferior = new Casilla(casillaAtacada.Fila + 1, casillaAtacada.Columna);
        Casilla casillaAdyacenteSuperior = new Casilla(casillaAtacada.Fila - 1, casillaAtacada.Columna);
        Casilla casillaAdyacenteIzquierda = new Casilla(casillaAtacada.Fila, casillaAtacada.Columna - 1);
        Casilla casillaAdyacenteDerecha = new Casilla(casillaAtacada.Fila, casillaAtacada.Columna + 1);

        if(EsUnaPosicionValida(casillaAdyacenteInferior))
            this.casillasAdyacentes.Add(casillaAdyacenteInferior);
        if(EsUnaPosicionValida(casillaAdyacenteSuperior))
            this.casillasAdyacentes.Add(casillaAdyacenteSuperior);
        if(EsUnaPosicionValida(casillaAdyacenteIzquierda))
            this.casillasAdyacentes.Add(casillaAdyacenteIzquierda);
        if(EsUnaPosicionValida(casillaAdyacenteDerecha))
            this.casillasAdyacentes.Add(casillaAdyacenteDerecha);
    }

	private int EjecutarAtaque(Casilla casillaAtacada) 
	{
		return this.Administrador.EjecutarAccionIA(casillaAtacada, null, null, (int) Modo.Normal);
	}

 private int EjecutarHabilidadEspecial (int habilidad){
        Casilla casillaAtacable1;
        Casilla casillaAtacable2;
        Casilla casillaAtacable3;
        bool flagAtaqueNormalD2 = false;
        //las habilidad 1,2,3 y 5 requieren de una sola casilla para activar su ataque.
        if (habilidad == -1)
            return -1;
        if (habilidad == Constantes.ARTILLERIA_RAPIDA_ID || habilidad == Constantes.PROYECTIL_HE_ID || habilidad == Constantes.TORPEDO_ID) 
        {
            casillaAtacable1 = GenerarCasillaRandomNoAtacada();
            Debug.Log("EJECUTO HABILIDAD " + habilidad + " SOBRE LA CASILLA ATACABLE 1: "  + casillaAtacable1.Fila + "," + casillaAtacable1.Columna);
            Administrador.EjecutarAccionIA(casillaAtacable1, null, null, habilidad);
        } 
        //la habilidad 4 requiere de 2 casillas para activar su ataque.
        else if (habilidad == Constantes.REFORZAR_ARMADURA_ID) 
        {
            casillaAtacable1 = GenerarCasillaRandomBarco();
            if (casillaAtacable1 == null){
                Debug.Log("FALLO EL INTENTO DE EJECUCION DE HABILIDAD REFORZAR ARMADURA");
                Atacar();
            } else {
                Debug.Log("EJECUTO HABILIDAD " + habilidad + " SOBRE LA CASILLA ATACABLE 1: "  + casillaAtacable1.Fila + "," + casillaAtacable1.Columna);
                Administrador.EjecutarAccionIA(casillaAtacable1, null, null, habilidad);
            }
        } 
        else if (habilidad == Constantes.DISPARO_DOBLE_ID) 
            {
                do{
                    casillaAtacable1 = GenerarCasillaRandomNoAtacada();
                    casillaAtacable2 = GenerarCasillaRandomNoAtacada();
					if(jugadorH.TableroPosicion.GetCasilla(casillaAtacable1.Fila, casillaAtacable1.Columna).Barco != null &&
                        jugadorH.TableroPosicion.GetCasilla(casillaAtacable1.Fila, casillaAtacable1.Columna).Barco.Vivo &&
						jugadorH.TableroPosicion.GetCasilla(casillaAtacable1.Fila, casillaAtacable1.Columna).Reforzada !=2 ) // FIXME: ¿reforzada != 2? Que carajos.
                        flagAtaqueNormalD2 = true;
					if(jugadorH.TableroPosicion.GetCasilla(casillaAtacable2.Fila, casillaAtacable2.Columna).Barco != null &&
                        jugadorH.TableroPosicion.GetCasilla(casillaAtacable2.Fila, casillaAtacable2.Columna).Barco.Vivo &&
                        jugadorH.TableroPosicion.GetCasilla(casillaAtacable2.Fila, casillaAtacable2.Columna).Reforzada !=2 )
                        flagAtaqueNormalD2 = true;
                } while (casillaAtacable1 == casillaAtacable2);
                Debug.Log("EJECUTO HABILIDAD " + habilidad + " SOBRE LA CASILLA ATACABLE 1: "  + casillaAtacable1.Fila + "," + casillaAtacable1.Columna + 
                " Y SOBRE LA CASILLA ATACABLE 2: "  + casillaAtacable2.Fila + "," + casillaAtacable2.Columna);
                Administrador.EjecutarAccionIA(casillaAtacable1, casillaAtacable2, null, habilidad);
            }
            //la habilidad 10 requiere de 3 casillas para activar su ataque.
            else if (habilidad == Constantes.SABOTAJE_ID)
                {
                //do{
                //    casillaAtacable1 = GenerarCasillaRandomNoAtacada();
                //    casillaAtacable2 = GenerarCasillaRandomNoAtacada();
                //    casillaAtacable3 = GenerarCasillaRandomNoAtacada(); 
                //} while (casillaAtacable1 == casillaAtacable2 || casillaAtacable1 == casillaAtacable3 || casillaAtacable2 == casillaAtacable3);
                Casilla [] posiciones = GenerarPosicionesSabotaje();
                if (posiciones == null){
                    return EjecutarAtaqueNormal();
                }
                casillaAtacable1 = posiciones[0];
                casillaAtacable2 = posiciones[1];
                casillaAtacable3 = posiciones[2];
                Debug.Log("EJECUTO HABILIDAD " + habilidad + " SOBRE LA CASILLA ATACABLE 1: "  + casillaAtacable1.Fila + "," + casillaAtacable1.Columna + 
                " Y SOBRE LA CASILLA ATACABLE 2: "  + casillaAtacable2.Fila + "," + casillaAtacable2.Columna + 
                " Y SOBRE LA CASILLA ATACABLE 3: "  + casillaAtacable3.Fila + "," + casillaAtacable3.Columna );
                Administrador.EjecutarAccionIA(casillaAtacable1, casillaAtacable2, casillaAtacable3, habilidad);
                return EjecutarAtaqueNormal();
                //la habilidad 6 y 7 no requieren casillas para activar su ataque.
                } else if (habilidad == Constantes.TORMENTA_MISILES_ID || habilidad == Constantes.PULSO_ELECTROMAGNETICO_ID)
                {
                    Debug.Log("EJECUTO HABILIDAD: " + habilidad + " QUE NO REQUIERE CASILLAS ATACABLES");
                    Administrador.EjecutarAccionIA(null, null, null, habilidad);
                }
        //if(habilidad == Constantes.SABOTAJE_ID || habilidad == Constantes.PULSO_ELECTROMAGNETICO_ID || habilidad == Constantes.REFORZAR_ARMADURA_ID || flagAtaqueNormalD2)
		// Ya no deberia ser necesario checkear por Disparo Doble
		if(habilidad == Constantes.SABOTAJE_ID || habilidad == Constantes.PULSO_ELECTROMAGNETICO_ID || habilidad == Constantes.REFORZAR_ARMADURA_ID)
            EjecutarAtaqueNormal();
        return -1;
    }

    private Casilla GenerarCasillaRandomNoAtacada()
    { 
        Casilla resultado;
        do
        {
            resultado = GenerarPosicionRandom();
        } while (EsUnaPosicionAtacada(resultado));
        return resultado;
    }

    private Casilla GenerarCasillaRandomBarco(){

        Casilla resultado = null;
        Casilla aux;
        int barcoRandom;
        int posRandom;
        int intentos = 0;
        do
        {  
            barcoRandom = jugadorIA.GetIndiceBarcoVivo();
            posRandom = jugadorIA.ListaBarcos[barcoRandom].GetIndicePosicionViva();
            aux = jugadorIA.TableroPosicion.GetCasilla(jugadorIA.ListaBarcos[barcoRandom].GetPosicionesOcupadas()[posRandom].Fila, jugadorIA.ListaBarcos[barcoRandom].GetPosicionesOcupadas()[posRandom].Columna);
            if (aux.Estado == Constantes.POSICION_BARCO && aux.Reforzada == 0 && aux.Revelada == false && aux.Contramedidas == false){
                resultado = new Casilla(aux.Fila, aux.Columna, 0);
            }
            intentos++;
        } while (resultado == null && intentos < 100);
        return resultado;
    }


    private bool EsUnaPosicionValida(Casilla casilla) 
	{
		return EsUnaPosicionEnRango(casilla) && ! EsUnaPosicionAtacada(casilla) ? true : false;
	}

    private bool EsUnaPosicionEnRango(Casilla casilla) 
	{
		return (casilla.Fila < 0 || casilla.Fila > 9 || casilla.Columna < 0 || casilla.Columna > 9) ? false : true;
	}

    private Casilla GenerarPosicionRandom() 
	{
		//return new Casilla(randomNumber.Next(0, 10), randomNumber.Next(0, 10));
        Casilla[] indices = new Casilla[100];
		int count = 0;
		for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 10; j++) {
                if (jugadorIA.TableroAtaque.GetEstadoCasilla(i,j) != Constantes.ATACADO_AGUA && jugadorIA.TableroAtaque.GetEstadoCasilla(i,j) != Constantes.ATACADO_BARCO) {
                    indices [count] = new Casilla(i,j,0);
                    count++;
                }
            }
		}

		if (count > 0) {
			int ran = UnityEngine.Random.Range (0, count);
			return indices [ran];
		} else {
			return null;
		}
	}

    private Casilla [] GenerarPosicionesSabotaje() 
	{
		//return new Casilla(randomNumber.Next(0, 10), randomNumber.Next(0, 10));
        Casilla[] indices = new Casilla[100];
		int count = 0;
		for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 10; j++) {
                if (jugadorIA.TableroPosicion.GetCasilla(i,j).HayBarco == false && jugadorIA.TableroPosicion.GetCasilla(i,j).Atacada == false && jugadorIA.TableroPosicion.GetCasilla(i,j).Trampa == 0) {
                    indices [count] = new Casilla(i,j,0);
                    count++;
                }
            }
		}

		if (count > 0) {
            Casilla [] res = new Casilla[3];
            do{
                res[0] = indices [UnityEngine.Random.Range (0, count)];
                res[1] = indices [UnityEngine.Random.Range (0, count)];
                res[2] = indices [UnityEngine.Random.Range (0, count)];
            }while (res[0] == res[1] || res[0] == res[2] || res[1] == res[2]);
			return res;
		} else {
			return null;
		}
	}

    private bool EsUnaPosicionAtacada(Casilla posicion) 
	{
		return this.tableroAtaqueDeIA.GetCasilla(posicion.Fila, posicion.Columna).Atacada ? true : false;
	} 

    private bool DeboUtilizarHabilidad(int cotaSup)
    {
		if (!jugadorIA.haySlotLibre ())
			return false;
        int numRandom = randomNumber.Next(0, cotaSup);
        if(numRandom == 0)
            return true;
        return false;
    }
}