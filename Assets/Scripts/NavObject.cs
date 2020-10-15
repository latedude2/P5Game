using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavObject : MonoBehaviour
{
    public bool isChecked = false; // already passed
    public bool isFollowed = false; // currently followed

    public void setChecked(bool isChecked)
    {
        this.isChecked = isChecked;
        if (isChecked == true)
        {
            isFollowed = false;
        }
    }
}
