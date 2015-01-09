using UnityEngine;
using System.Collections;

public class Krawedz : SelectableItem
{
    #region CLASS SETTINGS

    private Vector3 originalScale = Vector3.one * 0.2f;

    #endregion

    #region SCENE REFERENCES

    public Wierzcholek pierwszy, drugi;
    public Vector3 pierwszyminuspozycja;
    public float magnitude;
    public float orginalsizey;
    #endregion

    void Start()
    {
        isMarked = false;
    }

    public void Ustaw()
    {
        transform.position = Vector3.Lerp(pierwszy.transform.position, drugi.transform.position, 0.5f);
        transform.localScale = new Vector3(originalScale.x, originalScale.y, (pierwszy.transform.position - transform.position).magnitude);
        //orginalsizey = originalScale.y;
        //pierwszyminuspozycja = (pierwszy.transform.position - transform.position);
        //magnitude = (pierwszy.transform.position - transform.position).magnitude;
        //transform.localScale = new Vector3(0,0, originalScale.y * (pierwszy.transform.position - transform.position).magnitude);
        transform.LookAt(drugi.transform.position);
        //transform.rotation =Quaternion.Euler(Quaternion.ToEulerAngles(transform.rotation) +new Vector3(270, 0, 0));
    }
}
