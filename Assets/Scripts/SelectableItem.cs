using UnityEngine;
using System.Collections;

public class SelectableItem : MonoBehaviour {

    #region SCENE REFERENCES

    public Transform Podswietlenie;
    public Transform Rdzen;

    #endregion

    #region PREFAB REFERENCES

    public Material Kolor;
    public Material AktywnyKolor;
    public Material NieAktywnyKolor;

    #endregion

    protected bool isMarked;
    public int id;

    public void Zaznacz()
    {
        if (isMarked)
        {
            isMarked = false;
            InputManager.Instance.SelectedItem = null;
            Podswietlenie.renderer.material = NieAktywnyKolor;
        }
        else
        {
            if (InputManager.Instance.SelectedItem != null)
            {
                InputManager.Instance.SelectedItem.Zaznacz();
            }
            InputManager.Instance.SelectedItem = this;
            isMarked = true;
            Podswietlenie.renderer.material = AktywnyKolor;
        }
    }
}
