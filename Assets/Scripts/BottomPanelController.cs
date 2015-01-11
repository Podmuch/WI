using UnityEngine;
using System.Collections;

public class BottomPanelController : MonoBehaviour
{

    #region SCENE REFERENCES

    public GameObject kontener;
    public Transform CzerwonePodswietlenie;
    public Transform NiebieskiePodswietlenie;
    public Transform ZielonePodswietlenie;
    public tk2dTextMesh KorzenTekst;
    public tk2dUITextInput InputTekst;

    #endregion

    #region PREFAB REFERENCES

    public Material Zielony;
    public Material Czerwony;
    public Material Niebieski;
    public Material AktywnyKolor;
    public Material NieAktywnyKolor;

    #endregion
    
    Wierzcholek SelectedItem;
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
	    if(SelectedItem!=InputManager.Instance.SelectedItem as Wierzcholek)
        {
            SelectedItem = InputManager.Instance.SelectedItem as Wierzcholek;
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
                //wybor koloru
                if(SelectedItem.kolor=="zielony")
                {
                    UstawZielony();
                }
                else if(SelectedItem.kolor =="czerwony")
                {
                    UstawCzerwony();
                }
                else
                {
                    UstawNiebieski();
                }
                //ustawienie korzenia
                isKorzen=SelectedItem.Korzen == SelectedItem;
                if (isKorzen)
                {
                    KorzenTekst.text = "Jest Korzeniem";
                }
                else
                {
                    KorzenTekst.text = "Ustaw jako Korzen";
                }
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
        if (!isKorzen)
        {
            InputManager.Instance.UstawJakoKorzen(SelectedItem);
            InputManager.Instance.PrzeliczPotomkow(InputManager.Instance.WezDrzewoZDanymKorzeniem(SelectedItem));
            InputManager.Instance.UstawGraf();
            isKorzen = true;
            KorzenTekst.text = "Jest Korzeniem";
        }
    }

    public void ZmianaTekstu()
    {
        if (isVisible)
        {
            SelectedItem.etykieta = InputTekst.Text;
        }
    }

    public void UstawZielony()
    {
        CzerwonePodswietlenie.renderer.material = NieAktywnyKolor;
        NiebieskiePodswietlenie.renderer.material = NieAktywnyKolor;
        ZielonePodswietlenie.renderer.material = AktywnyKolor;
        SelectedItem.kolor = "zielony";
        SelectedItem.Kolor = Zielony;
        SelectedItem.Rdzen.renderer.material = SelectedItem.Kolor;
    }
    public void UstawCzerwony()
    {
        ZielonePodswietlenie.renderer.material = NieAktywnyKolor;
        NiebieskiePodswietlenie.renderer.material = NieAktywnyKolor;
        CzerwonePodswietlenie.renderer.material = AktywnyKolor;
        SelectedItem.kolor = "czerwony";
        SelectedItem.Kolor = Czerwony;
        SelectedItem.Rdzen.renderer.material = SelectedItem.Kolor;
    }
    public void UstawNiebieski()
    {
        CzerwonePodswietlenie.renderer.material = NieAktywnyKolor;
        ZielonePodswietlenie.renderer.material = NieAktywnyKolor;
        NiebieskiePodswietlenie.renderer.material = AktywnyKolor;
        SelectedItem.kolor = "niebieski";
        SelectedItem.Kolor = Niebieski;
        SelectedItem.Rdzen.renderer.material = SelectedItem.Kolor;
    }
}
