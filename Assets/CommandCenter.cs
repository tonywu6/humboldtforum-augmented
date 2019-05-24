/*
Copyright 2019 Tony Wu

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using UnityEngine.SceneManagement;

public class CommandCenter : MonoBehaviour
{

    public class MuseumObject
    {
        public string id;
        public Vector4 location;
        public string type;
        public string name;

        public List<string> themes;
        public List<string> exhibitions;
        public Color32 color = new Color32(1, 1, 1, 1);

        public List<string> exhibits;
        public int navHierarchy;

        public MuseumObject(JSONNode json)
        {
            id = json["indices"]["id"];
            location = DatabaseLocationToVectorLocation(json["indices"]["locationIdentifier"]);
            type = json["indices"]["type"];
            name = json["description"]["name"];

            switch (type)
            {
                case "exhibit":
                    JSONNode curatorialInfo = json["curatorial"];
                    themes = new List<string>();
                    exhibitions = new List<string>();
                    foreach (JSONNode theme in curatorialInfo["themes"].AsArray)
                    {
                        themes.Add(theme);
                    }
                    foreach (JSONNode exhibition in curatorialInfo["exhibitions"].AsArray)
                    {
                        exhibitions.Add(exhibition);
                    }
                    JSONArray color32 = curatorialInfo["colors"][0].AsArray;
                    color = new Color32((byte)color32[0].AsInt, (byte)color32[1].AsInt, (byte)color32[2].AsInt, 255);
                    break;
                case "place":
                    JSONNode relations = json["relations"];
                    exhibits = new List<string>();
                    foreach (JSONNode exhibit in relations["exhibits"].AsArray)
                    {
                        exhibits.Add(exhibit);
                    }
                    navHierarchy = relations["navigationalHierarchy"];
                    break;
            }
        }
    }

    private JSONObject museumCollectionJSON;

    private GameObject museumSpace;
    private GameObject arMuseumSpace;
    public Camera placeholderCam;

    public static Vector4 currentLocation;
    public static Dictionary<string, MuseumObject> museumObjects = new Dictionary<string, MuseumObject>();

    public static float museumLength = 183.811f;
    public static float museumWidth = 121.7801f;

    // Start is called before the first frame update
    void Start()
    {
        LoadMuseumCollection();
        Debug.Log(museumCollectionJSON["items"]["1994.245.135"]["indices"]["provider"]);

        currentLocation = new Vector4(0.2445209768f, 0, 0.4865384615f, 0);

        UnloadAllScenesExcept("Controller");
    }
    private void LoadMuseumCollection()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "objects.json");
        string jsonStr = File.ReadAllText(filePath);
        museumCollectionJSON = (JSONObject)JSON.Parse(jsonStr);

        foreach (JSONNode item in museumCollectionJSON["exhibits"])
        {
            museumObjects.Add(item["indices"]["id"], new MuseumObject(item));
        }
        foreach (JSONNode item in museumCollectionJSON["places"])
        {
            museumObjects.Add(item["indices"]["id"], new MuseumObject(item));
        }
    }

    private IEnumerator AddSceneAndSetActive(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        yield return 0;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
    }
    private void UnloadAllScenesExcept(string sceneName)
    {
        int c = SceneManager.sceneCount;
        for (int i = 0; i < c; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            print(scene.name);
            if (scene.name != sceneName && scene.name != "UnityARKitRemote")
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }

    // - Demonstration Controls
    public void MapPreview()
    {
        if (!SceneManager.GetSceneByName("3DMapScene").isLoaded)
        {
            UnloadAllScenesExcept("Controller");
            StartCoroutine("AddSceneAndSetActive", "3DMapScene");
        }
    }
    public void MapLocalizationPreview()
    {
        if (!SceneManager.GetSceneByName("3DMapScene").isLoaded)
        {
            UnloadAllScenesExcept("Controller");
            StartCoroutine("AddSceneAndSetActive", "3DMapScene");
        }
    }
    public void ARMapPreview()
    {
        UnloadAllScenesExcept("Controller");
        StartCoroutine("AddSceneAndSetActive", "ARScene");
        placeholderCam.enabled = false;
    }
    public void ResetScenes()
    {
        UnloadAllScenesExcept("Controller");
        placeholderCam.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!museumSpace)
        {
            museumSpace = GameObject.Find("Museum");
        }
        if (!arMuseumSpace)
        {
            museumSpace = GameObject.Find("ARMuseum");
        }

    }

    // Helpers
    public static void ChangeColor(GameObject GO, Color col)
    {
        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
        Renderer renderer = GO.GetComponent<Renderer>();
        renderer.GetPropertyBlock(propBlock);
        propBlock.SetColor("_Color", col);
        renderer.SetPropertyBlock(propBlock);
    }
    public static Vector4 DatabaseLocationToVectorLocation(JSONNode jsonLocation)
    {
        JSONArray locationIdentifier = jsonLocation.AsArray;
        return new Vector4(locationIdentifier[4] - 0.5f, locationIdentifier[0] / 100, -(locationIdentifier[5] - 0.5f), locationIdentifier[0]);
    }
}
