using UnityEngine;
using System.Collections;

public class Kolor : MonoBehaviour
{
    #region CLASS SETTINGS

    private static float scenewidth = 32.2f;
    private static float sceneheight = 15.6f;
    private static float pixelwidth = Screen.width;
    private static float pixelheight = Screen.height;
    private static float pixelperunitwidth = pixelwidth / scenewidth;
    private static float pixelperunitheight = pixelheight / sceneheight;

    #endregion

    #region SCENE REFERENCES

    public tk2dTextMesh etykieta;

    #endregion
    // Use this for initialization
    public string kolor;
    private bool isClicked;
    private GameObject clone;
	void Start () {
        isClicked = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isClicked)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                clone.transform.localPosition = new Vector3((Input.mousePosition.x - pixelwidth * 0.5f) / pixelperunitwidth, (Input.mousePosition.y - pixelheight * 0.5f) / pixelperunitheight, clone.transform.localPosition.z);
                if (Input.GetAxis("Mouse ScrollWheel")!=0) // forward
                 {
                     clone.transform.localPosition += new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel"));
                 }
            }
            else
            {
                if (clone != null)
                {
                    if (clone.transform.localPosition.x > -8.0f && clone.transform.localPosition.y > -3.5f)
                    {
                        InputManager.Instance.DodajWierzcholek(clone.transform.localPosition, etykieta.text, kolor);
                    }
                    Destroy(clone);
                }
                isClicked = false;
            }
        }
	}

    void OnDownClick()
    {
        isClicked = true;
        if(clone==null)
        {
            clone = (GameObject)Instantiate(gameObject);
            clone.transform.localScale = Vector3.one;
        }
        clone.layer = 0;
    }

    void OnUpClick()
    {
        
    }
}
