using UnityEngine;
using System.Collections;
using System;

public class Wierzcholek : SelectableItem
{
    public Wierzcholek Korzen;
    public Wierzcholek Parent;
    public bool isLisc;
    public int iloscpotomkow;
    public float iloscStopniPionowo;
    public float iloscStopniPoziomo;
    public float poczatkowyStopienPionowo;
    public float poczatkowyStopienPoziomo;
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
        if(rigidbody.velocity.magnitude>0)
        {
            rigidbody.velocity = Vector3.zero;
            InputManager.Instance.PoprawKrawedzie(this);
        }
	}

    

    void OnMouseEnter()
    {
        isHover = true;
        InputManager.Instance.RotationBlocked = true;
    }

    void OnMouseExit()
    {
        isHover = false;
        InputManager.Instance.RotationBlocked = false;
    }

    public string Save()
    {
        etykieta = Tekst.text;
        kolor = Rdzen.renderer.material.color;
        return Korzen.id + "@" + ((Parent != null) ? Parent.id : -1) + "@" + id + "@" + kolor.r + "@" + kolor.g + "@" + kolor.b + "@" + etykieta + "@" + isLisc + "@" + iloscpotomkow + "@" + transform.position.x + "@" + transform.position.y + "@" + transform.position.z;
    }

    public void Load(string data)
    {
        string[] wierzcholekParams = data.Split('@');
        Korzen = InputManager.Instance.WezWierzcholekZIndeksem(Convert.ToInt32(wierzcholekParams[0]));
        Parent = InputManager.Instance.WezWierzcholekZIndeksem(Convert.ToInt32(wierzcholekParams[1]));
        transform.position = new Vector3(Convert.ToSingle(wierzcholekParams[9]), Convert.ToSingle(wierzcholekParams[10]), Convert.ToSingle(wierzcholekParams[11]));
        etykieta = wierzcholekParams[6];
        Tekst.text = etykieta;
        RodzicTekstu.rotation = InputManager.Instance.Camera.rotation;
        kolor.r = Convert.ToSingle(wierzcholekParams[3]);
        kolor.g = Convert.ToSingle(wierzcholekParams[4]);
        kolor.b = Convert.ToSingle(wierzcholekParams[5]);
        kolor.a = 1;
        Rdzen.renderer.material = new Material(Rdzen.renderer.material);
        Rdzen.renderer.material.color = kolor;
        iloscpotomkow = Convert.ToInt32(wierzcholekParams[8]);
        isLisc = Convert.ToBoolean(wierzcholekParams[7]);
    }
}
