using UnityEngine;
using System.Collections;

public class Kolor : MonoBehaviour
{
    #region CLASS SETTINGS

    private static float scenewidth = 35.0f;
    private static float sceneheight = 20;
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
    private Vector3 previousMousePosition;
    public Vector3 currentMousePosition;
	void Start () {
        isClicked = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isClicked)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                currentMousePosition = new Vector3((Input.mousePosition.x - pixelwidth * 0.5f) / pixelperunitwidth, (Input.mousePosition.y - pixelheight * 0.5f) / pixelperunitheight,0);
                Vector3 deltaTranslate =currentMousePosition- previousMousePosition;
                clone.transform.Translate(new Vector3(deltaTranslate.x, deltaTranslate.y, Input.GetAxis("Mouse ScrollWheel")));
                previousMousePosition = currentMousePosition;
            }
            else
            {
                InputManager.Instance.MovementForwardAndBackBlocked = false;
                if (clone != null)
                {
                    currentMousePosition = new Vector3((Input.mousePosition.x - pixelwidth * 0.5f) / pixelperunitwidth, (Input.mousePosition.y - pixelheight * 0.5f) / pixelperunitheight, 0);
                    if (currentMousePosition.x > -5.5f && currentMousePosition.y > -2)
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
        if(clone==null)
        {
            previousMousePosition =new Vector3((Input.mousePosition.x - pixelwidth * 0.5f) / pixelperunitwidth, (Input.mousePosition.y - pixelheight * 0.5f) / pixelperunitheight,0);
            isClicked = true;
            InputManager.Instance.MovementForwardAndBackBlocked = true;
            clone = (GameObject)Instantiate(gameObject);
            clone.transform.localScale = Vector3.one;
            clone.transform.rotation = InputManager.Instance.Camera.rotation;
            clone.transform.position = transform.position;
            clone.transform.Translate(0, 0, 5);
            clone.layer = 0;
        }
    }

    void OnUpClick()
    {
        
    }
}
