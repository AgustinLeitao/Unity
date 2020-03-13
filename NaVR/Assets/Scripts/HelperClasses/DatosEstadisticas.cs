using System;

[Serializable]
public class DatosEstadisticas
{
    public int victoriasModoClasico, victoriasModoContraReloj, victoriasModoNormal, derrotasModoClasico, derrotasModoContraReloj, derrotasModoNormal, empatesModoContraReloj;
    public EstadisticaHabilidad[] estadisticaHabilidades;

    public DatosEstadisticas()
    {
        estadisticaHabilidades = new EstadisticaHabilidad[8];
    }
}
