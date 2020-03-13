using System;

[Serializable]
public class EstadisticaHabilidad
{
    public int contador;
    public string habilidadEspecial;

    public EstadisticaHabilidad(string habilidad)
    {
        habilidadEspecial = habilidad;
    }
}
