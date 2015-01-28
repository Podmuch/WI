using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

public class InputManager : MonoBehaviour
{
    #region PREFAB REFERENCES

    public GameObject WierzcholekPrefab;
    public Material czerwony, zielony, niebieski;
    public GameObject KrawedzPrefab;
    public Transform Camera;

    #endregion

    #region SCENE REFERENCES

    public SelectableItem SelectedItem;

    #endregion

    public bool MovementForwardAndBackBlocked;
    public bool RotationBlocked;
    private List<Wierzcholek> wierzcholki;
    private int licznikWierzcholkow;
    private List<Krawedz> krawedzie;
    private int licznikKrawedzi;
    private Vector3 previousMousePosition;
    private bool isScrollPressed;
    private bool isRightButtonPressed;
    public static InputManager Instance { get; private set; }
	// Use this for initialization
    void Awake()
    {
        Instance = this;
        wierzcholki = new List<Wierzcholek>();
        licznikWierzcholkow = 0;
        krawedzie = new List<Krawedz>();
        licznikKrawedzi = 0;
        MovementForwardAndBackBlocked = false;
        isScrollPressed = false;
        isRightButtonPressed = false;
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (SelectedItem != null)
        {
            if(Input.GetKeyDown(KeyCode.Delete))
            {
                if(SelectedItem as Wierzcholek!= null)
                {
                    UsunWierzcholek(SelectedItem as Wierzcholek);
                }
                else
                {
                    UsunKrawedz(SelectedItem as Krawedz);
                }
                SelectedItem = null;
            }
        }
        if(!MovementForwardAndBackBlocked)
        {
            if (!isScrollPressed)
            {
                if (Input.GetMouseButtonDown(2))
                {
                    previousMousePosition = Input.mousePosition;
                    isScrollPressed = true;
                }
            }
            else
            {
                if (Input.GetMouseButton(2))
                {
                    Camera.Translate((Input.mousePosition - previousMousePosition)*0.01f);
                    previousMousePosition = Input.mousePosition;
                }
                else 
                {
                    isScrollPressed = false;
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                Camera.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel"));
            }
        }
        if(!RotationBlocked)
        {
            if (!isRightButtonPressed)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    previousMousePosition = Input.mousePosition;
                    isRightButtonPressed = true;
                }
            }
            else
            {
                if (Input.GetMouseButton(1))
                {
                    Vector3 deltaRotation = (Input.mousePosition - previousMousePosition) * 0.1f;
                    float tmpX = deltaRotation.x;
                    deltaRotation.x = deltaRotation.y;
                    deltaRotation.y = tmpX;
                    Camera.rotation = Quaternion.Euler(Camera.rotation.eulerAngles + deltaRotation);
                    previousMousePosition = Input.mousePosition;
                    for(int i=0;i<wierzcholki.Count;i++)
                    {
                        wierzcholki[i].RodzicTekstu.rotation = Camera.rotation;
                    }
                    for (int i = 0; i < krawedzie.Count; i++)
                    {
                        krawedzie[i].RodzicTekstu.rotation = Camera.rotation;
                    }
                }
                else
                {
                    isRightButtonPressed = false;
                }
            }
        }
	}

    public void DodajWierzcholek(Vector3 position, string etykieta, Material material)
    {
        GameObject nowywierzcholek = (GameObject)Instantiate(WierzcholekPrefab);
        nowywierzcholek.transform.position = position;
        Wierzcholek wierzcholek = nowywierzcholek.GetComponent<Wierzcholek>();
        wierzcholek.etykieta = etykieta;
        wierzcholek.Tekst.text = etykieta;
        wierzcholek.kolor = material.color;
        wierzcholek.Rdzen.renderer.material = material;
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
                UstawGraf();
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
                UstawGraf();
            }
            GameObject nowakrawedz = (GameObject)Instantiate(KrawedzPrefab);
            Krawedz krawedz = nowakrawedz.GetComponent<Krawedz>();
            krawedzie.Add(krawedz);
            licznikKrawedzi++;
            krawedz.id = licznikKrawedzi;
            krawedz.pierwszy = pierwszy;
            krawedz.drugi = drugi;
            krawedz.etykieta = "";
            krawedz.Tekst.text = krawedz.etykieta;
            krawedz.Ustaw();
        }
    }

    public void UsunKrawedz(Krawedz usuwanaKrawedz)
    {
        Wierzcholek pierwszy = usuwanaKrawedz.pierwszy;
        Wierzcholek drugi = usuwanaKrawedz.drugi;
        if(pierwszy==drugi.Parent)
        {
            //pierwszy jest rodzicem drugiego, sprawdzenie czy pierwszy teraz nie jest lisciem
            drugi.Parent = null;
            if(!wierzcholki.Exists((w)=>w.Parent==pierwszy))
            {
                pierwszy.isLisc = true;
            }
            //ustaw nowy korzen w powstalym drzewie i przeliczenie potomkow nowych drzew
            UstawJakoKorzen(drugi);
            PrzeliczPotomkow(wierzcholki.FindAll((w) => w.Korzen == drugi));
            PrzeliczPotomkow(wierzcholki.FindAll((w) => w.Korzen == pierwszy.Korzen));
            UstawGraf();
        }
        else
        {
            //drugi jest rodzicem pierwszego, sprawdzenie czy drugi teraz nie jest lisciem
            pierwszy.Parent = null;
            if (!wierzcholki.Exists((w) => w.Parent == drugi))
            {
                drugi.isLisc = true;
            }
            //ustaw nowy korzen w powstalym drzewie i przeliczenie potomkow nowych drzew
            UstawJakoKorzen(pierwszy);
            PrzeliczPotomkow(wierzcholki.FindAll((w) => w.Korzen == pierwszy));
            PrzeliczPotomkow(wierzcholki.FindAll((w) => w.Korzen == drugi.Korzen));
            UstawGraf();
        }
        //usun krawedz
        krawedzie.Remove(usuwanaKrawedz);
        Destroy(usuwanaKrawedz.gameObject);
        //przenumeruj na nowo krawedzie
        licznikKrawedzi = 0;
        foreach (Krawedz k in krawedzie)
        {
            licznikKrawedzi++;
            k.id = licznikKrawedzi;
        }
    }

    public void UsunWierzcholek(Wierzcholek usuwanyWierzcholek)
    {
        if (usuwanyWierzcholek.Parent != null)
        {
            //jezeli wierzcholek nie jest korzeniem to trzeba oddzielic go od rodzica
            Krawedz krawedz = krawedzie.Find((k) => (k.pierwszy == usuwanyWierzcholek && k.drugi == usuwanyWierzcholek.Parent) ||
                                                    (k.pierwszy == usuwanyWierzcholek.Parent && k.drugi == usuwanyWierzcholek));
            if (krawedz != null)
            {
                UsunKrawedz(krawedz);
            }
        }
        //teraz usuwany wierzcholek jest korzeniem
        List<Wierzcholek> dzieciUsuwanegoWierzcholka = wierzcholki.FindAll((w) => w.Parent == usuwanyWierzcholek);
        foreach(Wierzcholek dziecko in dzieciUsuwanegoWierzcholka)
        {
            Krawedz krawedz = krawedzie.Find((k) => (k.pierwszy == dziecko && k.drugi == usuwanyWierzcholek) ||
                                                    (k.pierwszy == usuwanyWierzcholek && k.drugi == dziecko));
            if (krawedz != null)
            {
                UsunKrawedz(krawedz);
            }
        }
        //Usuwamy odseparowany wierzcholek
        wierzcholki.Remove(usuwanyWierzcholek);
        Destroy(usuwanyWierzcholek.gameObject);
        //przenumeruj na nowo wierzcholki
        licznikWierzcholkow = 0;
        foreach (Wierzcholek w in wierzcholki)
        {
            licznikWierzcholkow++;
            w.id = licznikWierzcholkow;
        }
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
        //zmien jego potomkom korzen
        UstawNowyKorzenPotomkom(wierzcholek, wierzcholek);
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
            UstawNowyKorzenPotomkom(tmp, wierzcholek);
        }
        if(!wierzcholki.Exists((w)=>w.Korzen==tmp))
        {
            tmp.isLisc = true;
        }
    }

    private void UstawNowyKorzenPotomkom(Wierzcholek rodzic, Wierzcholek korzen)
    {
        List<Wierzcholek> potomkowie = wierzcholki.FindAll((w) => w.Parent == rodzic);
        foreach(Wierzcholek potomek in potomkowie)
        {
            potomek.Korzen = korzen;
            UstawNowyKorzenPotomkom(potomek, korzen);
        }
    }

    public void PrzeliczPotomkow(List<Wierzcholek> drzewo)
    {
        for(int i=0;i<drzewo.Count;i++)
        {
            drzewo[i].iloscpotomkow = 0;
        }
        foreach (Wierzcholek Lisc in drzewo)
        {
            Wierzcholek tmp=Lisc;
            while(tmp.Parent!=null)
            {
                tmp = tmp.Parent;
                tmp.iloscpotomkow ++;
            }
        }
    }

    public List<Wierzcholek> WezDrzewoZDanymKorzeniem(Wierzcholek Korzen)
    {
        return wierzcholki.FindAll((w) => w.Korzen == Korzen);
    }

    public void UstawGraf()
    {
        List<Wierzcholek> Korzenie = wierzcholki.FindAll((w) => w.Korzen == w);
        foreach (Wierzcholek korzen in Korzenie)
        {
            korzen.iloscStopniPionowo = 360;
            korzen.iloscStopniPoziomo = 360;
            korzen.poczatkowyStopienPionowo = 0;
            korzen.poczatkowyStopienPoziomo = 0;
            UstawDzieciWierzcholka(korzen, wierzcholki.FindAll((w) => w.Parent == korzen));
        }
        foreach(Krawedz kr in krawedzie)
        {
            kr.Ustaw();
        }
        for (int i = 0; i < wierzcholki.Count; i++)
        {
            wierzcholki[i].RodzicTekstu.rotation = Camera.rotation;
        }
        for (int i = 0; i < krawedzie.Count; i++)
        {
            krawedzie[i].RodzicTekstu.rotation = Camera.rotation;
        }
    }

    public void UstawDzieciWierzcholka(Wierzcholek rodzic, List<Wierzcholek> dzieci, bool podzialpionowy=true)
    {
        float iloscpodziałow = rodzic.iloscpotomkow;
        if(podzialpionowy)
        {
            float stopnieprzedzialu = rodzic.iloscStopniPionowo / iloscpodziałow;
            float poczatkowystopien = rodzic.poczatkowyStopienPionowo;
            foreach(Wierzcholek dziecko in dzieci)
            {
                //przydziel przestrzen
                dziecko.iloscStopniPoziomo = rodzic.iloscStopniPoziomo;
                dziecko.poczatkowyStopienPoziomo = rodzic.poczatkowyStopienPoziomo;
                dziecko.iloscStopniPionowo = stopnieprzedzialu * (dziecko.iloscpotomkow + 1);
                dziecko.poczatkowyStopienPionowo = poczatkowystopien;
                poczatkowystopien += dziecko.iloscStopniPionowo;
                //ustaw w odpowiedniej pozycji
                dziecko.transform.rotation = Quaternion.Euler(new Vector3(dziecko.poczatkowyStopienPoziomo + dziecko.iloscStopniPoziomo / 2,
                                                            dziecko.poczatkowyStopienPionowo + dziecko.iloscStopniPionowo / 2, 0));
                dziecko.transform.position = rodzic.transform.position;
                dziecko.transform.Translate(5, 0, 0);
                //ustaw wierzcholki dla dzieci
                UstawDzieciWierzcholka(dziecko, wierzcholki.FindAll((w) => w.Parent == dziecko), !podzialpionowy);
            }
        }
        else
        {
            float stopnieprzedzialu = rodzic.iloscStopniPoziomo / iloscpodziałow;
            float poczatkowystopien = rodzic.poczatkowyStopienPoziomo;
            foreach (Wierzcholek dziecko in dzieci)
            {
                //przydziel przestrzen
                dziecko.iloscStopniPionowo = rodzic.iloscStopniPionowo;
                dziecko.poczatkowyStopienPionowo = rodzic.poczatkowyStopienPionowo;
                dziecko.iloscStopniPoziomo = stopnieprzedzialu * (dziecko.iloscpotomkow + 1);
                dziecko.poczatkowyStopienPoziomo = poczatkowystopien;
                poczatkowystopien += dziecko.iloscStopniPoziomo;
                //ustaw w odpowiedniej pozycji
                dziecko.transform.rotation = Quaternion.Euler(dziecko.poczatkowyStopienPoziomo + dziecko.iloscStopniPoziomo / 2,
                                                            dziecko.poczatkowyStopienPionowo + dziecko.iloscStopniPionowo / 2, 0);
                dziecko.transform.position = rodzic.transform.position;
                dziecko.transform.Translate(0, 5, 0);
                //ustaw wierzcholki dla dzieci
                UstawDzieciWierzcholka(dziecko, wierzcholki.FindAll((w) => w.Parent == dziecko), !podzialpionowy);
            }
        }
        
    }

    public void PoprawKrawedzie(Wierzcholek wierzcholek)
    {
        List<Krawedz> krawedzieDoPoprawy = krawedzie.FindAll((k) => k.pierwszy == wierzcholek || k.drugi == wierzcholek);
        foreach (Krawedz kr in krawedzieDoPoprawy)
        {
            kr.Ustaw();
        }
    }

    public byte[] GetCurrentStateToSave()
    {
        StringBuilder builder=new StringBuilder();
        builder.AppendLine(Camera.position.x + "@" + Camera.position.y + "@" + Camera.position.z + "@" +
                            Camera.rotation.eulerAngles.x + "@" + Camera.rotation.eulerAngles.y + "@" + Camera.rotation.eulerAngles.z);
        builder.AppendLine(wierzcholki.Count.ToString());
        foreach (Wierzcholek w in wierzcholki)
        {
            builder.AppendLine(w.Save());
        }
        builder.AppendLine(krawedzie.Count.ToString());
        foreach (Krawedz kr in krawedzie)
        {
            builder.AppendLine(kr.Save());
        }
        return System.Text.Encoding.ASCII.GetBytes(builder.ToString());
    }

    public void SetCurrentStateFromLoad(byte[] buffer)
    {
        //wyczysc screen
        for (int i = 0; i < wierzcholki.Count; i++)
        {
            Destroy(wierzcholki[i].gameObject);
        }
        wierzcholki.Clear();
        licznikWierzcholkow = 0;
        for (int i = 0; i < krawedzie.Count; i++)
        {
            Destroy(krawedzie[i].gameObject);
        }
        krawedzie.Clear();
        licznikKrawedzi = 0;
        SelectedItem = null;
        //wczytanie danych
        try
        {
            string text = System.Text.Encoding.ASCII.GetString(buffer);
            string[] lines = text.Replace("\r", "").Split('\n');
            string[] cameraParams = lines[0].Split('@');
            Camera.position = new Vector3(Convert.ToSingle(cameraParams[0]), Convert.ToSingle(cameraParams[1]), Convert.ToSingle(cameraParams[2]));
            Camera.rotation = Quaternion.Euler(new Vector3(Convert.ToSingle(cameraParams[3]), Convert.ToSingle(cameraParams[4]), Convert.ToSingle(cameraParams[5])));
            licznikWierzcholkow = Mathf.RoundToInt(Convert.ToSingle(lines[1]));
            for (int i = 2; i < 2 + licznikWierzcholkow; i++)
            {
                GameObject nowywierzcholek = (GameObject)Instantiate(WierzcholekPrefab);
                Wierzcholek wierzcholek = nowywierzcholek.GetComponent<Wierzcholek>();
                wierzcholki.Add(wierzcholek);
                string[] wierzcholekParams = lines[i].Split('@');
                wierzcholek.id = Convert.ToInt32(wierzcholekParams[2]);
            }
            for (int i = 2; i < 2 + licznikWierzcholkow; i++)
            {
                wierzcholki[i - 2].Load(lines[i]);
            }
            licznikKrawedzi = Mathf.RoundToInt(Convert.ToSingle(lines[2 + licznikWierzcholkow]));
            for (int i = 3 + licznikWierzcholkow; i < 3 + licznikWierzcholkow + licznikKrawedzi; i++)
            {
                GameObject nowakrawedz = (GameObject)Instantiate(KrawedzPrefab);
                Krawedz krawedz = nowakrawedz.GetComponent<Krawedz>();
                krawedzie.Add(krawedz);
                krawedz.Load(lines[i]);
            }
        }
        catch (Exception ex)
        {
            for (int i = 0; i < wierzcholki.Count; i++)
            {
                Destroy(wierzcholki[i].gameObject);
            }
            wierzcholki.Clear();
            licznikWierzcholkow = 0;
            for (int i = 0; i < krawedzie.Count; i++)
            {
                Destroy(krawedzie[i].gameObject);
            }
            krawedzie.Clear();
            licznikKrawedzi = 0;
            SelectedItem = null;
        }
    }

    public Wierzcholek WezWierzcholekZIndeksem(int indeks)
    {
        return wierzcholki.Find((w) => w.id == indeks);
    }
}
