using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    #region PREFAB REFERENCES

    public GameObject WierzcholekPrefab;
    public Material czerwony, zielony, niebieski;
    public GameObject KrawedzPrefab;

    #endregion

    #region SCENE REFERENCES

    public SelectableItem SelectedItem;

    #endregion

    private List<Wierzcholek> wierzcholki;
    private int licznikWierzcholkow;
    private List<Krawedz> krawedzie;
    private int licznikKrawedzi;
    public static InputManager Instance { get; private set; }
	// Use this for initialization
    void Awake()
    {
        Instance = this;
        wierzcholki = new List<Wierzcholek>();
        licznikWierzcholkow = 0;
        krawedzie = new List<Krawedz>();
        licznikKrawedzi = 0;
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (SelectedItem != null)
        {
            if(Input.GetKeyDown(KeyCode.D))
            {
                if(SelectedItem as Wierzcholek!= null)
                {
                    UsunWierzcholek();
                }
                else
                {
                    UsunKrawedz();
                }
            }
        }
	}

    public void DodajWierzcholek(Vector3 position, string etykieta, string kolor)
    {
        GameObject nowywierzcholek = (GameObject)Instantiate(WierzcholekPrefab);
        nowywierzcholek.transform.position = position;
        Wierzcholek wierzcholek = nowywierzcholek.GetComponent<Wierzcholek>();
        wierzcholek.etykieta = etykieta;
        wierzcholek.kolor = kolor;
        wierzcholek.Kolor = (kolor == "czerwony") ? czerwony : (kolor == "zielony") ? zielony : niebieski;
        wierzcholek.Rdzen.renderer.material = wierzcholek.Kolor;
        wierzcholki.Add(wierzcholek);
        licznikWierzcholkow++;
        wierzcholek.id = licznikWierzcholkow;
        wierzcholek.iloscpotomkow = 0;
        wierzcholek.Korzen = wierzcholek;
        wierzcholek.Parent = null;
        wierzcholek.isLisc = true;
    }

    public void DodajKrawedz(Wierzcholek pierwszy, Wierzcholek drugi)
    {
        if (pierwszy.Korzen != drugi.Korzen)
        {
            if(pierwszy.Korzen.iloscpotomkow>drugi.Korzen.iloscpotomkow||
                (pierwszy.Korzen.iloscpotomkow==drugi.Korzen.iloscpotomkow&&pierwszy==SelectedItem))
            {
                //mniejsze drzewo
                List<Wierzcholek> mniejszeDrzewo = wierzcholki.FindAll((w)=>w.Korzen==drugi.Korzen);
                //przygotowanie pierwszego do podlaczenia
                pierwszy.isLisc = false;
                //ustalenie drugiego korzeniem drzewa
                UstawJakoKorzen(drugi);
                //podlaczenie
                drugi.Parent = pierwszy;
                //ustawienie glownego korzenia
                for(int i=0;i<mniejszeDrzewo.Count;i++)
                {
                    mniejszeDrzewo[i].Korzen = pierwszy.Korzen;
                }
                //przeliczenie na nowo potomkow
                List<Wierzcholek> noweDrzewo = wierzcholki.FindAll((w) => w.Korzen == pierwszy.Korzen);
                PrzeliczPotomkow(noweDrzewo);
            }
            else
            {
                //mniejsze drzewo
                List<Wierzcholek> mniejszeDrzewo = wierzcholki.FindAll((w) => w.Korzen == pierwszy.Korzen);
                //przygotowanie pierwszego do podlaczenia
                drugi.isLisc = false;
                //ustalenie drugiego korzeniem drzewa
                UstawJakoKorzen(pierwszy);
                //podlaczenie
                pierwszy.Parent = drugi;
                //ustawienie glownego korzenia
                for (int i = 0; i < mniejszeDrzewo.Count; i++)
                {
                    mniejszeDrzewo[i].Korzen = drugi.Korzen;
                }
                //przeliczenie na nowo potomkow
                List<Wierzcholek> noweDrzewo = wierzcholki.FindAll((w) => w.Korzen == drugi.Korzen);
                PrzeliczPotomkow(noweDrzewo);
            }
            GameObject nowakrawedz = (GameObject)Instantiate(KrawedzPrefab);
            Krawedz krawedz = nowakrawedz.GetComponent<Krawedz>();
            krawedzie.Add(krawedz);
            licznikKrawedzi++;
            krawedz.id = licznikKrawedzi;
            krawedz.pierwszy = pierwszy;
            krawedz.drugi = drugi;
            krawedz.Ustaw();
        }
    }

    public void UsunKrawedz()
    {

    }

    public void UsunWierzcholek()
    {

    }

    public void UstawJakoKorzen(Wierzcholek wierzcholek)
    {
        Wierzcholek tmp = wierzcholek;
        Wierzcholek tmpParent = tmp.Parent;
        Wierzcholek tmp3;
        tmp.Parent = null;
        tmp.Korzen= tmp;
        if (tmpParent != null)
        {
            wierzcholek.isLisc = false;
        }
        while (tmpParent != null)
        {
            //zapisuje obecny wierzcholek
            tmp3 = tmp;
            //biore rodzica obecnego rodzica
            tmp = tmpParent.Parent;
            //ustalam jako rodzica obecnego rodzica aktualny wierzcholek
            tmpParent.Parent = tmp3;
            tmpParent.Korzen = wierzcholek;
            //zapisuje rodzica
            tmp3 = tmpParent;
            //na kolejnego rodzica ustalam rodzica obecnego rodzica
            tmpParent = tmp;
            //na obecny wierzcholek ustalam obecnego rodzica
            tmp = tmp3;
        }
        if(!wierzcholki.Exists((w)=>w.Korzen==tmp))
        {
            tmp.isLisc = true;
        }
    }

    private void PrzeliczPotomkow(List<Wierzcholek> drzewo)
    {
        for(int i=0;i<drzewo.Count;i++)
        {
            drzewo[i].iloscpotomkow = 0;
        }
        List<Wierzcholek> Liscie = drzewo.FindAll((w) => w.isLisc);
        foreach(Wierzcholek Lisc in Liscie)
        {
            Wierzcholek tmp=Lisc;
            while(tmp.Parent!=null)
            {
                int iloscpotomkow = tmp.iloscpotomkow;
                tmp = tmp.Parent;
                tmp.iloscpotomkow += iloscpotomkow + 1;
            }
        }
    }

}
