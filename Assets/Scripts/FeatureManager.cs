using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureManager : MonoBehaviour
{

    public EasyVizARHeadsetManager manager;

    // key as id, value as the GameObject (the marker placed)
    public Dictionary<int, EasyVizAR.Feature> feature_dictionary = new Dictionary<int, EasyVizAR.Feature>();
    public Dictionary<int, GameObject> feature_gameobj_dictionary = new Dictionary<int, GameObject>(); // a seperate dictionary for keeping track of Gameobject in the scene
    public Dictionary<int, string> feature_type_dictionary = new Dictionary<int, string>();
    public EasyVizAR.FeatureList feature_list = new EasyVizAR.FeatureList();
    public EasyVizAR.Feature featureHolder = null;
    public GameObject markerHolder = null;
    public int featureID = 32; // also a temporary holder
    public string color = "";
    public string name = "";
  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    public void GetFeatureType()
    {
        EasyVizARServer.Instance.Get("locations/" + manager.LocationID + "/features/" + id, EasyVizARServer.JSON_TYPE, GetFeatureTypeCallback); // change path later

    }

    void GetFeatureTypeCallback()
    {
        var resultJSON = JsonUtility.FromJson<List<string>>(result);

        if (result != "error")
        {
            Debug.Log("SUCCESS: " + result);
            for (int i = 0; i < resultJSON.Count; i++)
            {
                feature_type_dictionary.Add(i, resultJSON[i]);

            }


        }
        else
        {
            Debug.Log("ERROR: " + result);
        }

    }
    */

    // a callback function
    void PostFeature(string result)
    {
        var resultJSON = JsonUtility.FromJson<EasyVizAR.Feature>(result);
        
        if (result != "error")
        { 
            Debug.Log("SUCCESS: " + result);

        }
        else
        {
            Debug.Log("ERROR: " + result);
        }
        Debug.Log("new ID added: " + resultJSON.id);
        feature_dictionary.Add(resultJSON.id, featureHolder);
        feature_gameobj_dictionary.Add(resultJSON.id, markerHolder);
        Debug.Log("post contain id?: " + feature_dictionary.ContainsKey(resultJSON.id));


    }

    void GetFeatureCallBack(string result)
    {
        var resultJSON = JsonUtility.FromJson<EasyVizAR.Feature>(result);

        if (result != "error")
        {
            Debug.Log("SUCCESS: " + result);

        }
        else
        {
            Debug.Log("ERROR: " + result);
        }


    }

    // POST 
    public void CreateNewFeature(int feature_type, GameObject marker)
    {

        EasyVizAR.Feature feature_to_post = new EasyVizAR.Feature();

      
        feature_to_post.createdBy = manager.LocationID;
       
        
        feature_to_post.position = marker.transform.position;
        switch (feature_type) {
            case 0: 
                {
                    feature_to_post.type = "ambulance";
                    feature_to_post.name = "ambulance";
                    break;
                };
            case 1:
                {
                    feature_to_post.type = "door";
                    feature_to_post.name = "door";
                    break;
                };
            case 2:
                {
                    feature_to_post.type = "elevator";
                    feature_to_post.name = "elevator";
                    break;
                };
            case 3:
                {
                    feature_to_post.type = "extinguisher";
                    feature_to_post.name = "extinguisher";
                    break;
                };
            case 4:
                {
                    feature_to_post.type = "fire";
                    feature_to_post.name = "fire";
                    break;
                };
            case 5:
                {
                    feature_to_post.type = "headset";
                    feature_to_post.name = "headset";
                    break;
                };
            case 6:
                {
                    feature_to_post.type = "injury";
                    feature_to_post.name = "injury";
                    break;
                };
            case 7:
                {
                    feature_to_post.type = "message";
                    feature_to_post.name = "message";
                    break;
                };
            case 8:
                {
                    feature_to_post.type = "object";
                    feature_to_post.name = "object";
                    break;
                };
            case 9:
                {
                    feature_to_post.type = "stairs";
                    feature_to_post.name = "stairs";
                    break;
                };
            case 10:
                {
                    feature_to_post.type = "user";
                    feature_to_post.name = "user";
                    break;
                };
            case 11:
                {
                    feature_to_post.type = "warning";
                    feature_to_post.name = "warning";
                    break;
                };
        }

       

        //   feature_to_post.updated = ((float)System.DateTime.Now.Hour + ((float)System.DateTime.Now.Minute * 0.01f));
        EasyVizAR.FeatureDisplayStyle style = new EasyVizAR.FeatureDisplayStyle();
        style.placement = "point";
        feature_to_post.style = style;
        
        //Serialize the feature into JSON
        var data = JsonUtility.ToJson(feature_to_post);
        Debug.Log("Json utility: " + feature_to_post.id);
        Debug.Log("locations/" + manager.LocationID + "/features");
        //for sending stuff to the server 
        
        EasyVizARServer.Instance.Post("locations/" + manager.LocationID + "/features", EasyVizARServer.JSON_TYPE, data, PostFeature);
       
        
        featureHolder = feature_to_post;
        markerHolder = marker;
        //append to list 
        
    }
    [ContextMenu("GetFeature")]
    public void GetFeature() //takes in id as parameter, for some reason Unity doesn't accept that convention
    {
       var id = featureID;
        EasyVizARServer.Instance.Get("locations/" + manager.LocationID + "/features/" + id, EasyVizARServer.JSON_TYPE, GetFeatureCallBack);


    }
    [ContextMenu("ListFeatures")]
    public void ListFeatures()
    {
        EasyVizARServer.Instance.Get("locations/" + manager.LocationID + "/features", EasyVizARServer.JSON_TYPE, ListFeatureCallBack);
    }

    void ListFeatureCallBack(string result) {
        if (result != "error")
        {
            Debug.Log("SUCCESS: " + result);
            feature_list = JsonUtility.FromJson<EasyVizAR.FeatureList> ("{\"features\":" + result + "}"); 

        }
        else
        {
            Debug.Log("ERROR: " + result);
        }

    }

    [ContextMenu("UpateFeature")]
    public void UpateFeature() //    public void UpateFeature(int id, GameObject new_feature)
    {
        Debug.Log("reached update method");
        Debug.Log("contain id?: " + feature_dictionary.ContainsKey(featureID));


        var id = featureID; // parameter 
        var new_feature = markerHolder; // parameter
        // the following feature will be specified by user
        
        if (feature_dictionary.ContainsKey(id))
        {
            //creating new feature
            EasyVizAR.Feature feature_to_patch = feature_dictionary[id];
            if (color.Length != 0)
            {
                feature_to_patch.color = color;
            }

            if (name.Length != 0)
            {
                feature_to_patch.name = name;
                feature_to_patch.type = name;
            }



            Debug.Log("feature name: " + feature_to_patch.name);
           //eature_to_patch.name = "Updated name: ";
            // Main: updating the position
            feature_to_patch.position = new_feature.transform.position;


            // TODO: might want to modify the style?
            //feature_to_patch.style.placement = "point";

            //Serialize the feature into JSON
            var data = JsonUtility.ToJson(feature_to_patch);


            // updates the dictionary
            featureHolder = feature_to_patch;
            featureID = id;
            EasyVizARServer.Instance.Patch("locations/" + manager.LocationID + "/features/" + id, EasyVizARServer.JSON_TYPE, data, UpdateFeatureCallback);

        }

    }

    void UpdateFeatureCallback(string result)
    {
        Debug.Log("reached update callback method");
        if (result != "error")
        {
            Debug.Log("SUCCESS: " + result);
            // updates the dictionary
            feature_dictionary[featureID] = featureHolder;

        }
        else
        {
            Debug.Log("ERROR: " + result);
        }

    }


    // implement later
    public void ReplaceFeature()
    {

    }

    void ReplaceFeatureCallback()
    {

    }



    [ContextMenu("DeleteFeature")]
    public void DeleteFeature()
    {
        var id = featureID; //parameter 

        if (feature_dictionary.ContainsKey(id))
        {
            EasyVizAR.Feature delete_feature = feature_dictionary[id];
            var data = JsonUtility.ToJson(delete_feature);
            EasyVizARServer.Instance.Delete("locations/" + manager.LocationID + "/features/" + id, EasyVizARServer.JSON_TYPE, data, DeleteFeatureCallBack);

        }

        /*
         foreach (EasyVizAR.Feature feature in feature_list)
         {
             if (feature.id == id)
             {
                 feature_list.Remove(feature);
                 break;
             }
         }
        */

    }

    void DeleteFeatureCallBack(string result)
    {
        if (result != "error")
        {
            Debug.Log("SUCCESS: " + result);
            // update the respective dictionaries
            feature_dictionary.Remove(featureID);
            feature_gameobj_dictionary.Remove(featureID);
           

        }
        else
        {
            Debug.Log("ERROR: " + result);
        }

    }
}
