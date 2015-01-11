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
    #endregion

    void Start()
    {
        isMarked = false;
    }

    public void Ustaw()
    {
        transform.position = Vector3.Lerp(pierwszy.transform.position, drugi.transform.position, 0.5f);
        transform.localScale = new Vector3(originalScale.x, originalScale.y, (pierwszy.transform.position - transform.position).magnitude);
        transform.LookAt(drugi.transform.position);
    }

    public string Save()
    {
        return pierwszy.id + "@" + drugi.id + "@" + id;
    }

    public void Load(string data)
    {
        string[] krawedzParams = data.Split('@');
        id = Convert.ToInt32(krawedzParams[2]);
        pierwszy = InputManager.Instance.WezWierzcholekZIndeksem(Convert.ToInt32(krawedzParams[0]));
        drugi = InputManager.Instance.WezWierzcholekZIndeksem(Convert.ToInt32(krawedzParams[1]));
        Ustaw();
    }
}
