using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial_Miercoles_Estructura_Gonzalo.Carmona
{
    enum Casillero
    {
        Normal,
        Relay,
        Portal,
        Batalla,
        AgujeroNegro
    }


    /*
	EL JUEGO DE LA OCA ESPACIAL
	 Este juego es similar al juego de la oca pero ocurre en el espacio!!
	 El juego permite hasta 5 jugadores compitiendo en un viaje interestelar para determinar quien es el mejor astronauta.
	 El juego ocurre por turnos, los jugadores se turnaran para lanzar el dado, un dado de 6 caras, y mover su pieza en consecuencia.
	 Cada jugador tendra una ficha de color que sera asignada segun el orden en el que jugaran. El orden sera: Rojo, Magenta, Verde, Amarillo y finalmente Cian.
	 Las reglas del juego son las siguientes:
		* El tablero consta de 64 casillas comenzando por la número 0, la plataforma de lanzamiento y finalizando en la casilla 63. 
		  Quien alcance la casilla 63 primero será el ganador.
		* Hay casillas especiales que pueden complicarnos o ayudarnos, estas son:
			- Relays: Las casillas 1, 5, 14, 23, 27, 36, 41, 45, 50 y 54 son relays interestelares, cada uno esta conectado con su siguiente.
			  Si un jugador cae en alguno sera propulzado a la velocidad de la luz hasta el próximo relay. 
			  En el caso del ultimo no sucede nada ya que no tiene conexion. 
			  Por ej: si cae en la casilla 14, sera transportado a la 23
			
			- Portales: Las casillas 6, 12, 35, 42, cada una es un extremo de un portal, esto es que las casillas 6 y 12 estan conectadas y la 35 con la 42.
			  Cuando un jugador cae en alguna de estas será transportado al otro extremo del portal. 
			  Por ej: si cae en 6 será enviado a 12, si cae en 12, será enviado a 6.
			
			- Batallas: Los casilleros 16, 26, 46, 56 se encuentran en sectores en disputa por lo que el jugador quedara afectado a asistir en batalla a su bando.
			  El jugador deberá lanzar el dado al comienzo de su proximo turno, si el dado es 4 o 6, el jugador queda desafectado y puede volver a lanzar el dado para avanzar en su viaje, 
			  de lo contrario pierde el turno y debera repetir esto el proximo turno.
			
			- Agujeros negros: Los casilleros 31, 38, 53 se encuentran sobre agujeros negros, estos atrapan todo lo que este cerca y lo devuelven al pasado. 
			  Si un jugador cae aqui, vuelve al casillero 0.
			
			- Los portales y relay se encuentran en territorio declarado neutral por lo que no se puede entrar en conflicto, pero, en todos los demas
			  cuando nos crucemos con otro jugador entraremos en conflicto.
			  Cada jugador lanzará el dado y ganara quien saque un valor mas alto, el recien llegado se encuentra en desventaja y 
			  por eso si resultara en empate, perderá. El perdedor vuelve al 0.
	Antes de comenzar, el juego deberia preguntar al usuario cuantos jugadores van a participar. 
    */
    class Program
    {

        const int MAX_PIEZAS = 5;
        const int CANTIDAD_CASILLEROS = 64;
        const int CASILLEROS_POR_FILA = 20;


        static ConsoleColor[] COLORES_PIEZAS = new ConsoleColor[] { ConsoleColor.Red,  ConsoleColor.Magenta,
                                                                    ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Cyan };


        static readonly int[] CASILLEROS_RELAY = new int[] { 1, 5, 14, 23, 27, 36, 41, 45, 50, 54 };
        static readonly int[] CASILLEROS_PORTAL = new int[] { 6, 12, 35, 42 };
        static readonly int[] CASILLEROS_BATALLA = new int[] { 16, 26, 46, 56 };
        static readonly int[] CASILLEROS_AGUJERO_NEGRO = new int[] { 31, 38, 53 };

        static readonly Random RANDOM_GEN = new Random();



        static void EscribirMensajeConConfirmacion(string msj)
        {
            Console.WriteLine(msj);
            Console.ReadLine();
        }


        /// <summary>
        /// Retorna un valor aleatorio en el rango de [1;6]
        /// </summary>
        /// <returns></returns>
        static int LanzarDado()
        {
            return RANDOM_GEN.Next(1, 7);
        }


        /// <summary>
        /// Determina si nroCasillero es Relay o no
        /// </summary>
        /// <param name="nroCasillero"></param>
        /// <returns>true, si nroCasillero se encuentra en CASILLEROS_RELAY, de lo contrario, false</returns>
        static bool EsRelay(int nroCasillero)
        {
            for (int i = 0; i < CASILLEROS_RELAY.Length; i++)
            {
                if (nroCasillero == CASILLEROS_RELAY[i])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determina si nroCasillero es Portal o no
        /// </summary>
        /// <param name="nroCasillero"></param>
        /// <returns>true, si nroCasillero se encuentra en CASILLEROS_PORTAL, de lo contrario, false</returns>
        static bool EsPortal(int nroCasillero)
        {
            for (int i = 0; i < CASILLEROS_PORTAL.Length; i++)
            {
                if (nroCasillero == CASILLEROS_PORTAL[i])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determina si nroCasillero es Batalla o no
        /// </summary>
        /// <param name="nroCasillero"></param>
        /// <returns>true, si nroCasillero se encuentra en CASILLEROS_BATALLA, de lo contrario, false</returns>
        static bool EsBatalla(int nroCasillero)
        {
            for (int i = 0; i < CASILLEROS_BATALLA.Length; i++)
            {
                if (nroCasillero == CASILLEROS_BATALLA[i])
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Determina si nroCasillero es Agujero Negro o no
        /// </summary>
        /// <param name="nroCasillero"></param>
        /// <returns>true, si nroCasillero se encuentra en CASILLEROS_AGUJERO_NEGRO, de lo contrario, false</returns>
        static bool EsAgujeroNegro(int nroCasillero)
        {
            for (int i = 0; i < CASILLEROS_AGUJERO_NEGRO.Length; i++)
            {
                if (nroCasillero == CASILLEROS_AGUJERO_NEGRO[i])
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Crea un nuevo tablero con CANTIDAD_CASILLEROS cantidad de casilleros donde cada casillero especial se encuentra determinado por su valor del enum Casillero.
        /// Cada casillero del array retornado sera:
        /// - Casillero.Portal si el indice del casillero se encuentra en el array CASILLEROS_PORTAL.
        /// - Casillero.AgujeroNegro si el indice del casillero se encuentra en el array CASILLEROS_AGUJERO_NEGRO.
        /// - Casillero.Batalla si el indice del casillero se encuentra en el array CASILLEROS_BATALLA.
        /// - Casillero.Relay si el indice del casillero se encuentra en el array CASILLEROS_RELAY.
        /// - Casillero.Normal de lo contrario.
        /// </summary>
        /// <param name="nroCasillero"></param>
        /// <returns>Un tablero con todos sus casilleros especiales ubicados.</returns>
        static Casillero[] CrearTablero()
        {
            Casillero[] nuevoTablero = new Casillero[CANTIDAD_CASILLEROS];

            for (int i = 0; i < CANTIDAD_CASILLEROS; i++)
            {
                if (EsPortal(i))
                {
                    nuevoTablero[i] = Casillero.Portal;
                }
                else if (EsAgujeroNegro(i))
                {
                    nuevoTablero[i] = Casillero.AgujeroNegro;
                }
                else if (EsBatalla(i))
                {
                    nuevoTablero[i] = Casillero.Batalla;
                }
                else if (EsRelay(i))
                {
                    nuevoTablero[i] = Casillero.Relay;
                }
                else
                {
                    nuevoTablero[i] = Casillero.Normal;
                }
            }

            return nuevoTablero;
        }


        /// <summary>
        /// Retorna el nro de casillero del porixmo Relay.
        /// El proximo relay de un relay es el que se encuentra en la posicion siguiente a la de la de nroCasillero en CASILLEROS_RELAY.
        /// Si nroCasillero fuera el ultimo elemento del array, retorna nroCasillero
        /// </summary>
        /// <param name="nroCasillero"></param>
        /// <returns>El casillero del proximo relay o nroCasillero, si es el ultimo</returns>
        static int ObtenerProximoRelay(int nroCasillero)
        {
            for (int i = 0; i < CASILLEROS_RELAY.Length - 1; i++)
            {
                if (CASILLEROS_RELAY[i] == nroCasillero)
                {
                    return CASILLEROS_RELAY[i + 1];
                }
            }

            return nroCasillero;
        }

        /// <summary>
        /// Retorna el numero de casillero del otro extremo del portal.
        /// Cada Portal esta compuesto por dos casilleros.
        /// Si nroCasillero se encuentra en un index PAR de CASILLEROS_PORTAL, retorna el elemento siguiente.
        /// Si no, retorna el elemento previo.
        /// </summary>
        /// <param name="nroCasillero"></param>
        /// <returns></returns>
        static int ObtenerExtremoPortal(int nroCasillero)
        {

            for (int i = 0; i < CASILLEROS_PORTAL.Length-1; i++)
            {
                if (CASILLEROS_PORTAL[i] == nroCasillero)
                {
                    if (i % 2 == 0)
                    {
                        return CASILLEROS_PORTAL[i + 1];
                    }
                    else
                    {
                        return CASILLEROS_PORTAL[i - 1];
                    }
                }
            }
            return CASILLEROS_PORTAL[2];
        }

        /// <summary>
        /// Determina si se puede salir de un casillero de Batalla.
        /// Esto es si el jugador lanza un dado y el resultado es 4 o 6.
        /// </summary>
        /// <returns>Si el jugador lanzo un 4 o 6, true, de lo contrario false.</returns>
        static bool PuedeSalirDeBatalla()
        {
            int resultadoDado;
            Console.WriteLine("Se encuentra en Batalla, debe lanzar un 4 o 6 para salir.");
            resultadoDado = LanzarDado();
            Console.WriteLine($"Lanzo un: {resultadoDado}");
            if (resultadoDado == 4 || resultadoDado == 6)
                return true;
            return false;
        }


        /// <summary>
        /// Ejecuta el movimiento correspondiente al jugador indxJugador, esto es:
        /// - Indica a quien le corresponde mover.
        /// - Si el jugador se encuentra en batalla, lanza el dado para intentar salir.
        /// - Lanza el dado correspondiente al turno.
        /// - Avanza la posicion del jugador indxJugador tantas posiciones como el resultado del dado
        /// - Como excepcion a lo anterior, en caso de que la posicion final del jugador sea mayor al ultimo casillero, el jugador no se mueve.
        /// </summary>
        /// <param name="tablero"></param>
        /// <param name="posicionesJugadores"></param>
        /// <param name="indxJugador"></param>
        static void EjecutarMovimiento(Casillero[] tablero, int[] posicionesJugadores, int indxJugador)
        {
            int casilleroActual = posicionesJugadores[indxJugador];
            int resultadoDado;

            Console.Write($"Turno de  ");
            WriteColoreado($">\n", COLORES_PIEZAS[indxJugador]);

            if (tablero[casilleroActual] == Casillero.Batalla)
            {
                if (!PuedeSalirDeBatalla())
                {
                    EscribirMensajeConConfirmacion("Sigue en batalla, ENTER para continuar...");
                    return;
                }
                else
                {
                    Console.WriteLine("Fin de batalla...");
                }
            }


            EscribirMensajeConConfirmacion("Presione ENTER para lanzar el dado...");


            resultadoDado = LanzarDado();

            Console.WriteLine($"Lanzo un: {resultadoDado}");


            int casilleroDestino = posicionesJugadores[indxJugador] + resultadoDado;


            if (casilleroDestino >= tablero.Length)
            {
                casilleroDestino = casilleroActual;
            }

            posicionesJugadores[indxJugador] = casilleroDestino;
        }



        /// <summary>
        /// Verifica y resuelve posibles conflictos entre el jugador indxJugador y cualquier jugador que se encuentre en el mismo casillero.
        /// Cada jugador lanza el dado, si el dado del jugador es mayor al del adversario, 
        /// el adversario vuelve al principio, de lo contrario el jugador vuelve al principio.
        /// 
        /// </summary>
        /// <param name="indxJugador"></param>
        /// <param name="posicionesJugadores"></param>
        static void ResolverConflictos(int indxJugador, int[] posicionesJugadores)
        {


            for (int indxAdversario = 0; indxAdversario < posicionesJugadores.Length; indxAdversario++)
            {
                if (indxAdversario != indxJugador && posicionesJugadores[indxJugador] != 0 && posicionesJugadores[indxAdversario] == posicionesJugadores[indxJugador])
                {
                    int dadoJugador = LanzarDado();
                    int dadoAdversario = LanzarDado();

                    Console.Write($"Conflicto entre ");
                    WriteColoreado($"> ({dadoJugador})", COLORES_PIEZAS[indxJugador]);
                    Console.Write($" y ");
                    WriteColoreado($"> ({dadoAdversario})\n", COLORES_PIEZAS[indxAdversario]);

                    if (dadoJugador > dadoAdversario)
                    {
                        Console.WriteLine($"Gano {COLORES_PIEZAS[indxJugador]}, {COLORES_PIEZAS[indxAdversario]} vuelve al principio. ");
                        posicionesJugadores[indxAdversario] = 0;
                    }
                    else if (dadoAdversario == dadoJugador)
                    {
                        Console.WriteLine("Empataron, no gano ninguno");
                    }
                    else
                    {
                        Console.WriteLine($"Gano {COLORES_PIEZAS[indxAdversario]}, {COLORES_PIEZAS[indxJugador]} vuelve al principio. ");
                        posicionesJugadores[indxJugador] = 0;
                    }


                }
            }
            return;
        }


        /// <summary>
        /// Valida el casillero al que llego el jugador indxJugador.
        /// Si el jugador llega a un relay, lo mueve al relay que corresponda.
        /// Si el jugador llega a un portal, lo transporta a la punta del portal correspondiente.
        /// Si el jugador llega a un agujero negro, vuelve al principio.
        /// Si el jugador llega a un casillero normal, Intenta resolver posibles conflictos .
        /// </summary>
        /// <param name="tablero"></param>
        /// <param name="posicionesJugadores"></param>
        /// <param name="indxJugador"></param>
        static void ProcesarDestino(Casillero[] tablero, int[] posicionesJugadores, int indxJugador)
        {
            int casilleroActual = posicionesJugadores[indxJugador];


            Console.ForegroundColor = COLORES_PIEZAS[indxJugador];

            switch (tablero[casilleroActual])
            {
                case Casillero.Relay:
                    posicionesJugadores[indxJugador] = ObtenerProximoRelay(casilleroActual);
                    Console.WriteLine($"Se fue de {casilleroActual} a {posicionesJugadores[indxJugador]}");
                    break;
                case Casillero.Portal:
                    posicionesJugadores[indxJugador] = ObtenerExtremoPortal(casilleroActual);
                    Console.WriteLine($"Se transporto de {casilleroActual} a {posicionesJugadores[indxJugador]}");
                    break;
                case Casillero.AgujeroNegro:
                    posicionesJugadores[indxJugador] = 0;
                    Console.WriteLine($"Volvio al principio de {casilleroActual} a {posicionesJugadores[indxJugador]}");
                    break;
                case Casillero.Normal:
                    Console.WriteLine($"Llegando a {casilleroActual}");
                    Console.ResetColor();
                    ResolverConflictos(indxJugador, posicionesJugadores);
                    break;
                default:
                    break;
            }
            Console.ResetColor();

        }




        /// <summary>
        /// Retorna el index del primer jugador en llegar al ultimo casillero.
        /// </summary>
        /// <param name="posicionJugadores"></param>
        /// <returns>el index del primer jugador en llegar al ultimo casillero o -1 si no hay ganadores.</returns>
        static int ObtenerGanador(int[] posicionJugadores)
        {
            for (int i = 0; i <= posicionJugadores.Length - 1; i++)
            {
                if (posicionJugadores[i] == CANTIDAD_CASILLEROS-1)
                    return i;
            }

            return -1;
        }

        static int ObtenerSiguienteJugador(int jugadorActual, int cantJugadores)
        {
            if (jugadorActual == cantJugadores)
            {
                return 0;
            }
            for (int i = 0; i < cantJugadores; i++)
            {
                if (i > jugadorActual)
                    return i;
            }
            return 0;
        }
        static void WriteColoreado(string txt, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(txt);
            Console.ResetColor();
        }

        static ConsoleColor ObtenerColorCasillero(Casillero tipoDeCasillero)
        {
            switch (tipoDeCasillero)
            {
                case Casillero.AgujeroNegro:
                    return ConsoleColor.DarkBlue;
                case Casillero.Batalla:
                    return ConsoleColor.DarkRed;
                case Casillero.Portal:
                    return ConsoleColor.DarkCyan;
                case Casillero.Relay:
                    return ConsoleColor.DarkGreen;
            }

            return ConsoleColor.White;
        }

        static void DibujarReferencias()
        {

            Casillero[] tiposCasillero = (Casillero[])Enum.GetValues(typeof(Casillero));
            Console.Write("Referencias: ");
            for (int i = 0; i < tiposCasillero.Length; i++)
            {
                WriteColoreado($"{tiposCasillero[i]}", ObtenerColorCasillero(tiposCasillero[i]));
                Console.Write("|");
            }
            Console.WriteLine();

        }
        static void DibujarJuego(Casillero[] tablero, int[] posicionesJugadores)
        {
            int fila = 0;
            int columna = 0;

            for (int nroCasillero = 0; nroCasillero < tablero.Length; nroCasillero++)
            {
                Console.SetCursorPosition(columna, fila);
                int filaDibujo = fila;

                Console.Write("|");
                for (int j = 0; j < posicionesJugadores.Length; j++)
                {
                    int nroJugador = j + 1;

                    if (posicionesJugadores[j] == nroCasillero)
                    {
                        WriteColoreado(">", COLORES_PIEZAS[j]);
                    }
                    else
                    {
                        Console.Write($" ");
                    }


                    if (nroJugador % 2 == 0)
                    {
                        Console.SetCursorPosition(columna, ++filaDibujo);
                        if (nroJugador < posicionesJugadores.Length)
                            Console.Write("|");
                    }
                }


                if (posicionesJugadores.Length % 2 == 0)
                    Console.SetCursorPosition(columna, filaDibujo);
                else
                    Console.SetCursorPosition(columna, filaDibujo + 1);

                Console.Write("|");

                WriteColoreado($"{nroCasillero}", ObtenerColorCasillero(tablero[nroCasillero]));

                columna += 3;

                int nuevaFila = (int)Math.Ceiling(posicionesJugadores.Length / 2f);

                if (nroCasillero % CASILLEROS_POR_FILA == 0)
                {
                    for (int fin = 0; fin <= nuevaFila; fin++)
                    {
                        Console.SetCursorPosition(columna, fila + fin);
                        Console.Write("|");
                    }

                    fila += nuevaFila + 3;
                    columna = 0;
                }
            }
            Console.WriteLine("\n");
            DibujarReferencias();

        }

        static void Jugar(int cantJugadores)
        {
            int jugadorActual = 0;


            Casillero[] tablero = CrearTablero();

            int[] posicionesJugadores = new int[cantJugadores];

            int ganador = -1;


            while ((ganador = ObtenerGanador(posicionesJugadores)) == -1)
            {
                DibujarJuego(tablero, posicionesJugadores);
                EjecutarMovimiento(tablero, posicionesJugadores, jugadorActual);
                Console.ReadLine();
                Console.Clear();

                DibujarJuego(tablero, posicionesJugadores);
                ProcesarDestino(tablero, posicionesJugadores, jugadorActual);
                Console.ReadLine();
                Console.Clear();


                jugadorActual = ObtenerSiguienteJugador(jugadorActual, cantJugadores);
            }

            Console.Write("Ganador: ");
            WriteColoreado($">\n", COLORES_PIEZAS[ganador]);

            Console.ReadLine();
        }

        static int ElegirJugadores() 
        {
            Console.WriteLine("EL JUEGO DE LA OCA ESPACIAL");
            Console.WriteLine("Este juego es similar al juego de la oca pero ocurre en el espacio!!");
            Console.WriteLine("El juego permite hasta 5 jugadores compitiendo en un viaje interestelar para determinar quien es el mejor astronauta.");
            Console.WriteLine("El juego ocurre por turnos, los jugadores se turnaran para lanzar el dado, un dado de 6 caras, y mover su pieza en consecuencia.");
            Console.WriteLine("Cada jugador tendra una ficha de color que sera asignada segun el orden en el que jugaran. El orden sera: Rojo, Magenta, Verde, Amarillo y finalmente Cian.");
            EscribirMensajeConConfirmacion("ENTER para continuar...");
            Console.Clear();

            int cantidadJugadores = 0;
            Console.WriteLine("¿Cuantos jugadores van a jugar?");
            cantidadJugadores = int.Parse(Console.ReadLine());
            while(cantidadJugadores > 5 || cantidadJugadores < 1)
            {
                Console.WriteLine("El juego solo puede ser de 1 a 5 jugadores");
                cantidadJugadores = int.Parse(Console.ReadLine());
            }
            Console.Clear();
            return cantidadJugadores;
        }



        static void Main(string[] args)
        {
            Jugar(ElegirJugadores());
        }
    }
}

