using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CargaDeEstadisticas : MonoBehaviour
{
    public GameObject victoriasModoClasico, victoriasModoContraReloj, victoriasModoNormal, derrotasModoClasico, derrotasModoContraReloj, derrotasModoNormal, habilidadEspecialFavorita, empatesModoContraReloj;
    private DatosEstadisticas estadisticasDelJugador;

    void Start ()
    {
        //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.GetString("EstadisticasDelJugador", string.Empty).Equals(string.Empty))
        {
            victoriasModoClasico.GetComponent<Text>().text = "MODO CLASICO - CANTIDAD DE VICTORIAS: 0";
            victoriasModoContraReloj.GetComponent<Text>().text = "MODO CONTRARELOJ - CANTIDAD DE VICTORIAS: 0";
            victoriasModoNormal.GetComponent<Text>().text = "MODO NORMAL - CANTIDAD DE VICTORIAS: 0";
            derrotasModoClasico.GetComponent<Text>().text = "MODO CLASICO - CANTIDAD DE DERROTAS: 0";
            derrotasModoContraReloj.GetComponent<Text>().text = "MODO CONTRARELOJ - CANTIDAD DE DERROTAS: 0";
            derrotasModoNormal.GetComponent<Text>().text = "MODO NORMAL - CANTIDAD DE DERROTAS: 0";
            empatesModoContraReloj.GetComponent<Text>().text = "MODO CONTRARELOJ - CANTIDAD DE EMPATES: 0";
            habilidadEspecialFavorita.GetComponent<Text>().text = "HABILIDAD ESPECIAL FAVORITA: NO SE UTILIZO NINGUNA HABILIDAD ESPECIAL";
        }
        else
        {
            estadisticasDelJugador = Serializador.Deserializar<DatosEstadisticas>(PlayerPrefs.GetString("EstadisticasDelJugador"));

            victoriasModoClasico.GetComponent<Text>().text = "MODO CLASICO - CANTIDAD DE VICTORIAS: " + estadisticasDelJugador.victoriasModoClasico;
            victoriasModoContraReloj.GetComponent<Text>().text = "MODO CONTRARELOJ - CANTIDAD DE VICTORIAS: " + estadisticasDelJugador.victoriasModoContraReloj;
            victoriasModoNormal.GetComponent<Text>().text = "MODO NORMAL - CANTIDAD DE VICTORIAS: " + estadisticasDelJugador.victoriasModoNormal;
            derrotasModoClasico.GetComponent<Text>().text = "MODO CLASICO - CANTIDAD DE DERROTAS: " + estadisticasDelJugador.derrotasModoClasico;
            derrotasModoContraReloj.GetComponent<Text>().text = "MODO CONTRARELOJ - CANTIDAD DE DERROTAS: " + estadisticasDelJugador.derrotasModoContraReloj;
            derrotasModoNormal.GetComponent<Text>().text = "MODO NORMAL - CANTIDAD DE DERROTAS: " + estadisticasDelJugador.derrotasModoNormal;
            empatesModoContraReloj.GetComponent<Text>().text = "MODO CONTRARELOJ - CANTIDAD DE EMPATES: " + estadisticasDelJugador.empatesModoContraReloj;

            var contadorMaximoHabilidadesEspeciales = estadisticasDelJugador.estadisticaHabilidades.Max(campo => campo.contador);
            if(contadorMaximoHabilidadesEspeciales == 0)
                habilidadEspecialFavorita.GetComponent<Text>().text = "HABILIDAD ESPECIAL FAVORITA: NO SE UTILIZO NINGUNA HABILIDAD ESPECIAL";
            else
                habilidadEspecialFavorita.GetComponent<Text>().text = "HABILIDAD ESPECIAL FAVORITA: " + estadisticasDelJugador.estadisticaHabilidades.Where(campo => campo.contador == contadorMaximoHabilidadesEspeciales).FirstOrDefault().habilidadEspecial.ToUpper();
        }
    }		
}
