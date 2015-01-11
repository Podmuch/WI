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
        return Korzen.id+"@"+((Parent!=null)?Parent.id:-1)+"@"+id+"@"+kolor+"@"+etykieta+"@"+isLisc+"@"+iloscpotomkow+"@"+transform.position.x+"@"+transform.position.y+"@"+transform.position.z;
    }

    public void Load(string data)
    {
        string[] wierzcholekParams = data.Split('@');
        Korzen = InputManager.Instance.WezWierzcholekZIndeksem(Convert.ToInt32(wierzcholekParams[0]));
        Parent = InputManager.Instance.WezWierzcholekZIndeksem(Convert.ToInt32(wierzcholekParams[1]));
        transform.position = new Vector3(Convert.ToSingle(wierzcholekParams[7]), Convert.ToSingle(wierzcholekParams[8]), Convert.ToSingle(wierzcholekParams[9]));
        etykieta = wierzcholekParams[4];
        kolor = wierzcholekParams[3];
        Kolor = (kolor == "czerwony") ? InputManager.Instance.czerwony : (kolor == "zielony") ? InputManager.Instance.zielony : InputManager.Instance.niebieski;
        Rdzen.renderer.material = Kolor;
        iloscpotomkow = Convert.ToInt32(wierzcholekParams[6]);
        isLisc = Convert.ToBoolean(wierzcholekParams[5]);
    }
}
