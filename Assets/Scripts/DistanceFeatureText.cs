using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DistanceFeatureText : MonoBehaviour
{
    public GameObject cam;
    public Vector3 cam_pos;
    public GameObject feature;
    public GameObject quad;
    public bool isFeet;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        isFeet = true;
    }

    // Update is called once per frame
    void Update()
    {
        cam_pos = cam.GetComponent<Transform>().position;
        CalcFeatureDist();
    }

    void CalcFeatureDist()
    {
        float x_distance = (float)Math.Pow(quad.transform.position.x - cam_pos.x, 2);
        float z_distance = (float)Math.Pow(quad.gameObject.transform.position.z - cam_pos.z, 2);

        if (isFeet)
        {
            x_distance = (float)(x_distance * 3.281);
            z_distance = (float)(z_distance * 3.281);
        }
        float distance = (float)Math.Round((float)Math.Sqrt(x_distance + z_distance) * 10f) / 10f;
        
        var feature_text = feature.transform.Find("Feature_Text").GetComponent<TextMeshPro>();
        var type = feature.transform.Find(string.Format("type"));
        string feature_type = type.transform.GetChild(0).name;

        if (isFeet)
        {
            feature_text.text = feature_type + " - " + distance.ToString() + "ft";
            //display_dist_text.text = distance.ToString() + "ft";

        }
        else
        {
            feature_text.text = feature_type + " - " + distance.ToString() + "m";

        }

    }



}
