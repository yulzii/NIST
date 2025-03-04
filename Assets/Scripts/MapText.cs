
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.Networking;

public class MapText : MonoBehaviour
{
    public string baseURL = "http://halo05.wings.cs.wisc.edu:5000/";
    public string location_id = "";

    public EasyVizARHeadsetManager manager;


    [SerializeField]
    public TextMeshPro map_text;
    // Start is called before the first frame update
    /*
    [System.Serializable]
    public class MapField
    {
        public string id;
        public string name;
    }
    [System.Serializable]
    public class Location
    {
        public MapField[] loc;
    }
    */
    void Start()
    {
        map_text = GetComponent<TextMeshPro>();
        map_text.color = new Color32(191, 131, 6, 255);
        // TODO: ask about how to fix the hard code problem
        //StartCoroutine(GetLocationInfo(baseURL + "/locations/"));//this is the one USED


        //StartCoroutine(GetMapName("http://halo05.wings.cs.wisc.edu:5000/locations/66a4e9f2-e978-4405-988e-e168a9429030/"));
        Debug.Log("manager ID: " + manager.LocationID);
        StartCoroutine(GetLocationInfo(baseURL + "/locations/" + manager.LocationID));


        //GetMapName(baseURL + "/locations/" + manager.LocationID + "/");

        //added -> delete later
        //location_id = EasyVizAR.Headset.location_id;// ("hi", "hi", "hi", GetLocationInfo);
        // location_id = EasyVizARHeadset._locationID;
        //Debug.Log("the location here is: " + location_id);

    }

    
    IEnumerator GetLocationInfo(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();


        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            var txt = www.downloadHandler.text;
            EasyVizAR.MapField myLocation = new EasyVizAR.MapField();
            myLocation = JsonUtility.FromJson<EasyVizAR.MapField>(txt);

            map_text.text = myLocation.name;
            Debug.Log("the location id at GetLocationID: " + location_id);
        }
    }
    

    //testing --> TODO: why this isn't working?
    public void GetMapName(string url)
    {
        EasyVizARServer.Instance.Get(url, EasyVizARServer.JSON_TYPE, GetMapCallback);

    }

    public void GetMapCallback(string result)
    {
        var resultJSON = JsonUtility.FromJson<EasyVizAR.MapField>(result); // TODO: why is this line causing error? will look into this in the future

        if (result != "error")
        {
            Debug.Log("SUCCESS: " + result);

            map_text.text = resultJSON.name;
        }
        else
        {
            Debug.Log("ERROR: " + result);
        }


    }


    // below are the past stuff used for future reference

    /*
IEnumerator GetLocationInfo(string url)
{
    UnityWebRequest www = UnityWebRequest.Get(url);

    yield return www.SendWebRequest();


    if (www.result != UnityWebRequest.Result.Success)
    {
        Debug.Log(www.error);
    }
    else
    {
        var txt = www.downloadHandler.text;
        Location myLocation = new Location();
        myLocation = JsonUtility.FromJson<Location>("{\"loc\":" + txt + "}");

        this.location_id = myLocation.loc[0].id; //TODO: will have to fix this after (ask Ross about how he is setting up location id) 
        map_text.text = myLocation.loc[0].name;
        Debug.Log("the location id at GetLocationID: " + location_id);
    }
}
*/



}