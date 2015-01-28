using UnityEngine;
using System.Collections;

public class BottomPanelController : MonoBehaviour
{

    #region SCENE REFERENCES

    public GameObject kontener;
    public tk2dTextMesh KorzenTekst;
    public tk2dUITextInput InputTekst;
    public Kolor kolor;
    public tk2dUIScrollbar zielony;
    public tk2dUIScrollbar czerwony;
    public tk2dUIScrollbar niebieski;

    #endregion
    
    SelectableItem SelectedItem;
    bool forceUpdate;
    bool isKorzen;
    bool isVisible;
	// Use this for initialization
	void Start () 
    {
        SelectedItem = null;
        forceUpdate = false;
        isKorzen = false;
        kontener.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	    if(SelectedItem!=InputManager.Instance.SelectedItem)
        {
            SelectedItem = InputManager.Instance.SelectedItem;
            forceUpdate = true;
        }
        if(forceUpdate)
        {
            if(SelectedItem==null)
            {
                kontener.SetActive(false);
                isVisible = false;
            }
            else
            {
                kontener.SetActive(true);
                //ustawienie korzenia
                if (SelectedItem as Wierzcholek != null)
                {
                    isKorzen = (SelectedItem as Wierzcholek).Korzen == SelectedItem;
                    if (isKorzen)
                    {
                        KorzenTekst.text = "Jest Korzeniem";
                    }
                    else
                    {
                        KorzenTekst.text = "Ustaw jako Korzen";
                    }
                    KorzenTekst.transform.parent.gameObject.SetActive(true);
                }
                else
                {
                    KorzenTekst.transform.parent.gameObject.SetActive(false);
                }
                //kolor
                kolor.kolor = SelectedItem.Rdzen.renderer.material.color;
                kolor.renderer.material.color = kolor.kolor;
                zielony.Value = kolor.kolor.g;
                czerwony.Value = kolor.kolor.r;
                niebieski.Value = kolor.kolor.b;
                //zmiena tekstu
                InputTekst.Text=SelectedItem.etykieta;
                //koniec update'u
                forceUpdate = false;
                isVisible = true;
            }
        }
	}

    public void UstawJakoKorzen()
    {
        if (SelectedItem as Wierzcholek != null)
        {
            if (!isKorzen)
            {
                InputManager.Instance.UstawJakoKorzen(SelectedItem as Wierzcholek);
                InputManager.Instance.PrzeliczPotomkow(InputManager.Instance.WezDrzewoZDanymKorzeniem(SelectedItem as Wierzcholek));
                InputManager.Instance.UstawGraf();
                isKorzen = true;
                KorzenTekst.text = "Jest Korzeniem";
            }
        }
    }

    public void Usun()
    {
        if (InputManager.Instance.SelectedItem as Wierzcholek != null)
        {
            InputManager.Instance.UsunWierzcholek(InputManager.Instance.SelectedItem as Wierzcholek);
        }
        else
        {
            InputManager.Instance.UsunKrawedz(InputManager.Instance.SelectedItem as Krawedz);
        }
        InputManager.Instance.SelectedItem = null;
        SelectedItem = null;
        forceUpdate = true;
        //etykiety przy krawedziach
        //sprawdz zapis i wczytywanie
    }

    public void ZmianaTekstu()
    {
        if (isVisible)
        {
            SelectedItem.etykieta = InputTekst.Text;
            SelectedItem.Tekst.text = InputTekst.Text;
        }
    }

    public void ZmienilSieZielony()
    {
        kolor.ZmienilSieZielony();
        SelectedItem.Rdzen.renderer.material.color = kolor.kolor;
    }

    public void ZmienilSieCzerwony()
    {
        kolor.ZmienilSieCzerwony();
        SelectedItem.Rdzen.renderer.material.color = kolor.kolor;
    }
    public void ZmienilSieNiebieski()
    {
        kolor.ZmienilSieNiebieski();
        SelectedItem.Rdzen.renderer.material.color = kolor.kolor;
    }
}
