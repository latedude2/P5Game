using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerMovementPlayback : MonoBehaviour
{
    private StreamReader reader = null;
    FileInfo theSourceFile = null;
    private int counter = 0;
    [SerializeField] private int framesBetweenRecordTakes = 10; // How many frames between recording of gameobject coordinates

    // Start is called before the first frame update
    void Start()
    {
        string path = "Save files/test0.txt";
        theSourceFile = new FileInfo(path);

        if (theSourceFile != null && theSourceFile.Exists)
            reader = theSourceFile.OpenText();


        if (reader == null)
        {
            Debug.Log("test0.txt not found or not readable");
        }

    }

    //Does not depend on framerate
    void FixedUpdate()
    {
        counter++;
        if (counter % framesBetweenRecordTakes == 0)
        {
            string line = reader.ReadLine();
            if(line != null)
            {
                string[] data = line.Split(' ');
                if (data.Length > 2) //If this line has coordinates (not the end task tag)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = data[i].Replace('.', ','); //Some systems save the float values with wrong notation, we fix that here
                    }

                    Vector3 position = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
                    transform.position = position;
                    Quaternion target = new Quaternion(float.Parse(data[4]), float.Parse(data[5]), float.Parse(data[6]), float.Parse(data[3]));
                    transform.rotation = target;
                    //Debug.Log(data[7]);
                }
                else
                {
                    Debug.Log(line);
                }
            }
        }
    }

    void OnDisable()
    {
        reader.Close();
    }
}
