using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerMovementRecorder : MonoBehaviour
{
    private StreamWriter writer;
    private int counter = 0;
    [SerializeField] private int framesBetweenRecordTakes = 10; // How many frames between recording of gameobject coordinates
    [System.NonSerialized] public bool endTaskCompleted = false;
    [System.NonSerialized] public bool testEnded = false;
    [SerializeField] private GameObject mistakeTriggerParent;
    [SerializeField] private GameObject shortcutTriggerParent;

    private List<MistakeTrigger> mistakeTriggers = new List<MistakeTrigger>();
    private List<ShortcutTrigger> shortcutTriggers = new List<ShortcutTrigger>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in mistakeTriggerParent.transform)
        {
            mistakeTriggers.Add(child.GetComponent<MistakeTrigger>());
        }
        foreach (Transform child in shortcutTriggerParent.transform)
        {
            shortcutTriggers.Add(child.GetComponent<ShortcutTrigger>());
        }

        FirstTimeSetup();
        Save(0);
    }

    //Does not depend on framerate
    void FixedUpdate()
    {
        if (endTaskCompleted)
        {
            writer.WriteLine("EndTaskCompleted");
            endTaskCompleted = false;
        }
        counter++;
        if (counter % framesBetweenRecordTakes == 0)
        {
            writer.WriteLine(transform.position.x + " " + transform.position.y + " " + transform.position.z + " " + transform.GetChild(0).rotation.w + " " + transform.GetChild(0).rotation.x + " " + transform.GetChild(0).rotation.y + " " + transform.GetChild(0).rotation.z + " " + Time.fixedTime);
        }
        if(testEnded)
        {
            this.enabled = false;
        }
    }

    private void FirstTimeSetup()
    {
        if (!Directory.Exists("Save files"))
        {
            Directory.CreateDirectory("Save files");
        }
    }

    private void Save(int num)
    {
        string path = "Save files/test" + num + ".txt";

        if (System.IO.File.Exists(path))
        {
            num++;
            Save(num);
        }
        else
        {
            //File.Delete(path);
            //Write some text to the test.txt file
            writer = new StreamWriter(path, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        MistakeTrigger mistakeTrigger = other.GetComponent<MistakeTrigger>();
        if(mistakeTrigger != null)
        {
            mistakeTrigger.visited = true; 
        }

        ShortcutTrigger shortcutTrigger = other.GetComponent<ShortcutTrigger>();
        if (shortcutTrigger != null)
        {
            shortcutTrigger.visited = true;
        }
    }

    void OnDisable()
    {
        int mistakeCount = 0;
        int shortcutCount = 0;
        for (int i = 0; i < mistakeTriggers.Count; i++)
        {
            if(mistakeTriggers[i].visited)
            {
                mistakeCount++;
            }
        }
        for (int i = 0; i < shortcutTriggers.Count; i++)
        {
            if (shortcutTriggers[i].visited)
            {
                shortcutCount++;
            }
        }
        writer.WriteLine(mistakeCount + " " + shortcutCount);
        writer.Close();
    }
}
