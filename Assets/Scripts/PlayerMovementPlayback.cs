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
        string path = "Assets/Resources/test.txt";
        theSourceFile = new FileInfo(path);

        if (theSourceFile != null && theSourceFile.Exists)
            reader = theSourceFile.OpenText();


        if (reader == null)
        {
            Debug.Log("puzzles.txt not found or not readable");
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
                if (!data[0].Equals("EndTaskCompleted") && data.Length > 2) //If this line has coordinates (not the end task tag)
                {
                    Vector3 position = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
                    transform.position = position;
                    // Rotate the cube by converting the angles into a quaternion.
                    Quaternion target = new Quaternion(float.Parse(data[4]), float.Parse(data[5]), float.Parse(data[6]), float.Parse(data[3]));

                    // Dampen towards the target rotation
                    transform.rotation = target;
                    Debug.Log(data[7]);
                }
            }
        }
    }

    void OnDisable()
    {
        reader.Close();
    }
}
