using UnityEngine;
using System.Collections;

public class Wierzcholek : MonoBehaviour
{
    #region SCENE REFERENCES

    public Transform Podswietlenie;
    public Transform Rdzen;

    #endregion

    #region PREFAB REFERENCES

    public Material Kolor;
    public Material AktywnyKolor;
    public Material NieAktywnyKolor;

    #endregion

    private bool isMarked;
	// Use this for initialization
	void Start ()
	{
	    isMarked = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void Zaznacz()
    {
        if (isMarked)
        {
            isMarked = false;
            Podswietlenie.renderer.material = NieAktywnyKolor;
        }
        else
        {
            isMarked = true;
            Podswietlenie.renderer.material = AktywnyKolor;
        }
    }
}
