using UnityEngine;
using System.Collections;
using System;

public class Krawedz : SelectableItem
{
    #region CLASS SETTINGS

    private Vector3 originalScale = Vector3.one * 0.2f;

    #endregion

    #region SCENE REFERENCES

    public Wierzcholek pierwszy, drugi;
    public Transform srodek;
    public BoxCollider kolider;

    #endregion

    void Start()
    {
        isMarked = false;
    }

    public void Ustaw()
    {
        transform.position = Vector3.Lerp(pierwszy.transform.position, drugi.transform.position, 0.5f);
        srodek.transform.localScale = new Vector3(originalScale.x, originalScale.y, (pierwszy.transform.position - transform.position).magnitude);
        srodek.transform.LookAt(drugi.transform.position);
    }

    public string Save()
    {
        etykieta = Tekst.text;
        kolor = Rdzen.renderer.material.color;
        return pierwszy.id + "@" + drugi.id + "@" + id + "@" + etykieta + "@" + kolor.r + "@" + kolor.g + "@" + kolor.b;
    }

    public void Load(string data)
    {
        string[] krawedzParams = data.Split('@');
        id = Convert.ToInt32(krawedzParams[2]);
        pierwszy = InputManager.Instance.WezWierzcholekZIndeksem(Convert.ToInt32(krawedzParams[0]));
        drugi = InputManager.Instance.WezWierzcholekZIndeksem(Convert.ToInt32(krawedzParams[1]));
        etykieta = krawedzParams[3];
        RodzicTekstu.rotation = InputManager.Instance.Camera.rotation;
        Tekst.text = etykieta;
        kolor.r = Convert.ToSingle(krawedzParams[4]);
        kolor.g = Convert.ToSingle(krawedzParams[5]);
        kolor.b = Convert.ToSingle(krawedzParams[6]);
        kolor.a = 1;
        Rdzen.renderer.material = new Material(Rdzen.renderer.material);
        Rdzen.renderer.material.color = kolor;
        Ustaw();
    }
}
