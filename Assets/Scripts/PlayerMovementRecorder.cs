using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerMovementRecorder : MonoBehaviour
{
    private StreamWriter writer;
    private int counter = 0;
    [SerializeField] private int framesBetweenRecordTakes = 50; // How many frames between recording of gameobject coordinates

    // Start is called before the first frame update
    void Start()
    {
        string path = "Assets/Resources/test.txt";

        File.Delete(path);
        //Write some text to the test.txt file
        writer = new StreamWriter(path, true);
        writer.WriteLine("Start of recording");
    }

    //Does not depend on framerate
    void FixedUpdate()
    {
        counter++;
        if(counter % framesBetweenRecordTakes == 0)   //
            writer.WriteLine(transform.position);
    }

    void OnDisable()
    {
        writer.Close();
    }
}
