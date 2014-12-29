using System;
using System.IO;
using UnityEngine;
using System.Windows.Forms;

public class LeftPanelController : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	}

    void WczytajClick()
    {
        Stream myStream = null;
        OpenFileDialog openFileDialog1 = new OpenFileDialog();

        openFileDialog1.InitialDirectory = "c:\\";
        //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
        //openFileDialog1.FilterIndex = 2;
        //openFileDialog1.RestoreDirectory = true;

        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        {
            try
            {
                myStream = openFileDialog1.OpenFile();
                using (myStream)
                {

                    // Insert code to read the stream here.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }
        }
    }

    void ZapiszClick()
    {
        Stream myStream;
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();

        //saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
        //saveFileDialog1.FilterIndex = 2;
        //saveFileDialog1.RestoreDirectory = true;

        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        {
            myStream = saveFileDialog1.OpenFile();
            // Code to write the stream goes here.
            myStream.Close();
        }
    }
}
