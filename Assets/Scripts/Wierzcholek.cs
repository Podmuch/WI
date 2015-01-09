using UnityEngine;
using System.Collections;

public class Wierzcholek : SelectableItem
{
    public Wierzcholek Korzen;
    public Wierzcholek Parent;
    public bool isLisc;
    public int iloscpotomkow;
    public string kolor;
    public string etykieta;
    private bool isHover;
	// Use this for initialization
	void Start ()
	{
	    isMarked = false;
        isHover = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isHover && !isMarked && Input.GetMouseButtonDown(1))
        {
            if (InputManager.Instance.SelectedItem as Wierzcholek!= null)
            {
                InputManager.Instance.DodajKrawedz((InputManager.Instance.SelectedItem.id > id) ? this : InputManager.Instance.SelectedItem as Wierzcholek,
                    (InputManager.Instance.SelectedItem.id < id) ? this : InputManager.Instance.SelectedItem as Wierzcholek);
            }
        }
	}

    

    void OnMouseEnter()
    {
        isHover = true;
    }

    void OnMouseExit()
    {
        isHover = false;
    }
}
