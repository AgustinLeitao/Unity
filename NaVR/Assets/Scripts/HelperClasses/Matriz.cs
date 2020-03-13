using System.Collections.Generic;

public class Matriz
{
    shipEnum[,] tablero;
    shipEnum barcoActual;
    int randomColumn, randomRow, randomDirection;  
    const int cantidadBarcos5 = 1, cantidadBarcos4 = 2, cantidadBarcos3 = 2, cantidadBarcos2 = 4, barcosTotales = 9;

    public Matriz(shipEnum[,] tablero)
    {
        this.tablero = tablero;
    }

    public void InicilizarPosicionesMatriz()
    {         
        bool canIncreceRow, canDecreceRow, canDecreceColumn, canIncreceColumn, estaElBarcoAcomodado;
        List<int> randomNumbers = new List<int>();
        System.Random randomNumber = new System.Random();
        int indicador, counter, limiteInferior, limiteSuperior;      

        for (int i = 0; i < barcosTotales; i++)
        {
            if (i == 0)
            {
                barcoActual = shipEnum.barcoCincoPosiciones;
            }
            else
            {
                if (i == 1 || i == 2)
                {
                    barcoActual = shipEnum.barcoCuatroPosiciones;
                }
                else
                {
                    if (i == 3 || i == 4)
                    {
                        barcoActual = shipEnum.barcoTresPosiciones;
                    }
                    else
                    {
                        if (i >= 5 && i < 9)
                        {
                            barcoActual = shipEnum.barcoDosPosiciones;
                        }
                    }
                }
            }

            estaElBarcoAcomodado = false;

            while (!estaElBarcoAcomodado)
            {
                do
                {
                    randomColumn = randomNumber.Next(0, 10);
                    randomRow = randomNumber.Next(0, 10);
                } while (tablero[randomRow, randomColumn] != shipEnum.posicionLibre || TieneAdyacentes(randomRow, randomColumn));

                canIncreceRow = randomRow + (int)barcoActual - 1 <= 9 ? true : false;
                canDecreceRow = randomRow - (int)barcoActual + 1 >= 0 ? true : false;
                canIncreceColumn = randomColumn + (int)barcoActual - 1 <= 9 ? true : false;
                canDecreceColumn = randomColumn - (int)barcoActual + 1 >= 0 ? true : false;

                counter = 0;
                randomNumbers.Clear();

                while (counter < 4 && !estaElBarcoAcomodado)
                {
                    do
                    {
                        randomDirection = randomNumber.Next(0, 4);
                    } while (randomNumbers.Contains(randomDirection));

                    indicador = -1;

                    switch (randomDirection)
                    {
                        case 0: if (canIncreceRow) indicador = 0; break;
                        case 1: if (canDecreceRow) indicador = 1; break;
                        case 2: if (canIncreceColumn) indicador = 2; break;
                        case 3: if (canDecreceColumn) indicador = 3; break;
                    }

                    if (indicador != -1)
                    {
                        if (indicador == 0)
                        {
                            limiteInferior = randomRow;
                            limiteSuperior = randomRow + (int)barcoActual - 1;
                            estaElBarcoAcomodado = EsPosicionValida(limiteInferior, limiteSuperior, true);
                        }
                        else
                        {
                            if (indicador == 1)
                            {
                                limiteInferior = randomRow - (int)barcoActual + 1;
                                limiteSuperior = randomRow;
                                estaElBarcoAcomodado = EsPosicionValida(limiteInferior, limiteSuperior, true);
                            }
                            else
                            {
                                if (indicador == 2)
                                {
                                    limiteInferior = randomColumn;
                                    limiteSuperior = randomColumn + (int)barcoActual - 1;
                                    estaElBarcoAcomodado = EsPosicionValida(limiteInferior, limiteSuperior, false);
                                }
                                else
                                {
                                    limiteInferior = randomColumn - (int)barcoActual + 1;
                                    limiteSuperior = randomColumn;
                                    estaElBarcoAcomodado = EsPosicionValida(limiteInferior, limiteSuperior, false);
                                }
                            }
                        }
                    }
                    counter++;
                }
            }
        }
    }

    private bool TieneAdyacentes(int filaSeleccionada, int columnaSeleccionada)
    {
        var canIncreaseRow = filaSeleccionada < 9;
        var canIncreaseColumn = columnaSeleccionada < 9;
        var canDecreaseRow = filaSeleccionada > 0;
        var canDecreaseColumn = columnaSeleccionada > 0;

        var maxRow = canIncreaseRow ? filaSeleccionada + 1 : filaSeleccionada;
        var minRow = canDecreaseRow ? filaSeleccionada - 1 : filaSeleccionada;
        var maxColumn = canIncreaseColumn ? columnaSeleccionada + 1 : columnaSeleccionada;
        var minColumn = canDecreaseColumn ? columnaSeleccionada - 1 : columnaSeleccionada;

        for (int fila = minRow; fila <= maxRow; fila++)
        {
            for (int columna = minColumn; columna <= maxColumn; columna++)
            {
                if (!(fila == filaSeleccionada && columna == columnaSeleccionada) && !(fila != filaSeleccionada && columna != columnaSeleccionada))
                {
                    if (tablero[fila, columna] != shipEnum.posicionLibre)
                        return true;
                }
            }
        }
        return false;
    }

    private bool EsPosicionValida(int limiteInferior, int limiteSuperior, bool isRowVariable)
    {
        for (int i = limiteInferior; i <= limiteSuperior; i++)
        {
            if (isRowVariable)
            {
                if (tablero[i, randomColumn] != shipEnum.posicionLibre || TieneAdyacentes(i, randomColumn))
                    return false;
            }
            else
            {
                if (tablero[randomRow, i] != shipEnum.posicionLibre || TieneAdyacentes(randomRow, i))
                    return false;
            }
        }

        // Si es valida recien ahora se actualiza el tablero de la cpu

        for (int i = limiteInferior; i <= limiteSuperior; i++)
        {
            if (isRowVariable)
            {
                tablero[i, randomColumn] = barcoActual;
            }
            else
            {
                tablero[randomRow, i] = barcoActual;
            }
        }
        return true;
    }
}
