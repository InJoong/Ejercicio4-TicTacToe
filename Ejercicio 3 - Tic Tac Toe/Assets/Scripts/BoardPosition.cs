using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardPosition : MonoBehaviour {
    /// <summary>
    /// Variable del tablero, establecida desde el Inspector
    /// </summary>
    public Board TheBoard;
    /// <summary>
    /// Indicamos si esta posición ya esta ocupado o no
    /// </summary>
    public bool Used;

    //Nombre del token
    public string token;

	public int Type;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown() {
        TheBoard.PlaceToken(this);
    }
}
