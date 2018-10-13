using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

    /// <summary>
    /// Almacena el jugador que va a realizar el siguiente
    /// movimiento, se utiliza para saber que ficha
    /// colocar. Cuando el valor es 0 entonces se tira el 
    /// TACHE mientras que si el valor es 1 entonces se
    /// coloca CÍRCULO
    /// </summary>
    public int _playerGame;
    /// <summary>
    /// Prefab del Tache
    /// </summary>
    public GameObject Cross;
    /// <summary>
    /// Prefab del Círculo
    /// </summary>
    public GameObject Circle;

	public BoardPosition[] Positions;

    public string resultado;

    private int turn = 0;

    void OnGUI () {
        string NextPlayer = _playerGame == 0 ? "TACHE" : "CÍRCULO";
        GUILayout.Label("Es el turno del jugador: " + NextPlayer);
        GUILayout.Label(resultado);
    }

	// Use this for initialization
	void Start () {
	    _playerGame = 0;
    }
	
	// Update is called once per frame
	void Update () {
        resultado = CheckGame();

        if (_playerGame == 1)
        {
            if (Positions[4].token == "" && turn == 1) PlaceToken(Positions[4]);
            else if (Positions[4].token == "Cross" && turn == 1) PlaceToken(Positions[0]);
            else
            {
                int[] biTable = new int[9];

                for (int i=0; i<9; i++)
                {
                    if (Positions[i].Used) biTable[i] = Positions[i].Type;
                    else biTable[i] = 2;
                }

                int position = Minimax(biTable, turn, _playerGame).move;
                
                PlaceToken(Positions[position]);
                Debug.Log(position);
            }
        }
    }

    /// <summary>
    /// Coloca la pieza de juego en la posición dada
    /// </summary>
    /// <param name="bp">Objeto BoardPosition que representa la posición
    /// en el tablero</param>
    public void PlaceToken (BoardPosition bp) {
        // Si la posición no está ocupada entonces colocamos
        // la ficha, de lo contrario, no hacemos nada
        if (!bp.Used) {
            // Guardamos la posición a utilizar
            Vector3 pos = bp.gameObject.transform.position;
            // Colocamos la ficha según el tipo de _playerGame que tenemos,
            // recuerda si _playerGame es 0 entonces colocamos un tache
            // de lo contrario colocamos un círculo
            GameObject obj = Instantiate(_playerGame == 0 ? Cross : Circle,
                                                      pos,
                                                      Quaternion.identity) as GameObject;
            // Marcamos la casilla como ocupada
            bp.Used = true;

            // Marcamos el token con su nombre
            bp.token = _playerGame == 0 ? "Cross" : "Circle";


            bp.Type = _playerGame;
			Debug.Log("Hola mundo");
			            
            // Cambiamos la ficha para el siguiente juego
            _playerGame = _playerGame == 0 ? 1 : 0;
            turn++;
        }
    }

    public struct Moves
    {
        public int score;
        public int move;

        public Moves(int _score, int _move)
        {
            score = _score;
            move = _move;
        }
    }

    public Moves Minimax(int[] table, int tempTurn, int playerTurn)
    {

        if (WinningCondition(table, 1))
        {
            return new Moves(10-tempTurn, 0);
        }
        else if (WinningCondition(table, 0))
        {
            return new Moves(-10 - tempTurn, 0);
        }
        else if (tempTurn >= 9)
        {
            return new Moves(0, 0);
        }

        List<Moves> moves = new List<Moves>();
        
        for (int i = 0; i < 9; i++)
        {
            if (table[i] == 2)
            {
                table[i] = playerTurn;

                Moves temp = new Moves(Minimax(table, tempTurn + 1, playerTurn == 0 ? 1 : 0).score, i);
                moves.Add(temp);

                table[i] = 2;
            }
        }

        int maxmin = 0;
        Moves bestPosition = new Moves();
        for (int i=0; i<moves.Count; i++)
        {
            if (moves[i].score >= maxmin && playerTurn == 1)
            {
                maxmin = moves[i].score;
                bestPosition = moves[i];
            } else if (moves[i].score <= maxmin && playerTurn == 0)
            {
                maxmin = moves[i].score;
                bestPosition = moves[i];
            }
        }

        Debug.Log("Best: "+bestPosition.move);

        return bestPosition;
    }

    public int[] TBoardToken(int[] table, int position, int playerTurn)
    {
        table[position] = playerTurn;

        return table;
    }

    public bool WinningCondition(int[] Positions, int player)
    {
        if ((Positions[0] == player && Positions[1] == player && Positions[2] == player) ||
                (Positions[3] == player && Positions[4] == player && Positions[5] == player) ||
                (Positions[6] == player && Positions[7] == player && Positions[8] == player) ||
                (Positions[0] == player && Positions[3] == player && Positions[6] == player) ||
                (Positions[1] == player && Positions[4] == player && Positions[7] == player) ||
                (Positions[2] == player && Positions[5] == player && Positions[8] == player) ||
                (Positions[0] == player && Positions[4] == player && Positions[8] == player) ||
                (Positions[6] == player && Positions[4] == player && Positions[2] == player))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string CheckGame()
    {
        //Computadora
        //Horizontal
        if ((Positions[0].token == "Circle" && Positions[1].token == "Circle" && Positions[2].token == "Circle") ||
                (Positions[3].token == "Circle" && Positions[4].token == "Circle" && Positions[5].token == "Circle") ||
                (Positions[6].token == "Circle" && Positions[7].token == "Circle" && Positions[8].token == "Circle"))
        {
            return "Perdio";
        }
        //Vertical
        if ((Positions[0].token == "Circle" && Positions[3].token == "Circle" && Positions[6].token == "Circle") ||
                (Positions[1].token == "Circle" && Positions[4].token == "Circle" && Positions[7].token == "Circle") ||
                (Positions[2].token == "Circle" && Positions[5].token == "Circle" && Positions[8].token == "Circle"))
        {
            return "Perdio";
        }
        //Diagonal
        if ((Positions[0].token == "Circle" && Positions[4].token == "Circle" && Positions[8].token == "Circle") ||
                (Positions[6].token == "Circle" && Positions[4].token == "Circle" && Positions[2].token == "Circle"))
        {
            return "Perdio";
        }

        //Jugador
        //Horizontal
        if ((Positions[0].token == "Cross" && Positions[1].token == "Cross" && Positions[2].token == "Cross") ||
                (Positions[3].token == "Cross" && Positions[4].token == "Cross" && Positions[5].token == "Cross") ||
                (Positions[6].token == "Cross" && Positions[7].token == "Cross" && Positions[8].token == "Cross"))
        {
            return "Gano";
        }
        //Vertical
        if ((Positions[0].token == "Cross" && Positions[3].token == "Cross" && Positions[6].token == "Cross") ||
                (Positions[1].token == "Cross" && Positions[4].token == "Cross" && Positions[7].token == "Cross") ||
                (Positions[2].token == "Cross" && Positions[5].token == "Cross" && Positions[8].token == "Cross"))
        {
            return "Gano";
        }
        //Diagonal
        if ((Positions[0].token == "Cross" && Positions[4].token == "Cross" && Positions[8].token == "Cross") ||
                (Positions[6].token == "Cross" && Positions[4].token == "Cross" && Positions[2].token == "Cross"))
        {
            return "Gano";
        }

        if (turn >= 9)
        {
            return "Empate";
        }

        return "";
    }
}

