/*
Copyright 2019 Tony Wu

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapController : MonoBehaviour
{
    //private static readonly float PanSpeed = 20f;
    //private static readonly float ZoomSpeedTouch = 0.1f;

    //private Camera cam;
    //private Plane cameraPlane;

    //private GameObject[] floors = new GameObject[4];
    //private GameObject augmentLayer;

    //private GameObject indicatorPrefab;

    //private List<string> visibleObjectKeywords = new List<string>();

    //void Awake()
    //{
    //    cam = Camera.main;
    //}
    //// Start is called before the first frame update
    //void Start()
    //{
    //    indicatorPrefab = Resources.Load<GameObject>("Prefabs/MuseumObjectIndicator");
    //    augmentLayer = transform.Find("Augmentation").gameObject;

    //    for (int i = 0; i < 4; i++)
    //    {
    //        floors[i] = transform.Find("Environment/BerlinerSchlossFloors/F" + i.ToString()).gameObject;
    //    }

    //    PlaceMuseumObjects(CommandCenter.museumObjects.Values.ToList());
    //    ToggleMuseumObjects(false, CommandCenter.museumObjects.Values.ToList());
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
    //    //{
    //    //    HandleTouch();
    //    //}
    //    //else
    //    //{
    //    //    HandleMouse();
    //    //}
    //}

    //private void UpdateVisibleObjects()
    //{
    //    ToggleMuseumObjects(false, CommandCenter.museumObjects.Values.ToList());
    //    foreach (string keyword in visibleObjectKeywords)
    //    {
    //        string filter = keyword.Split('.')[0];
    //        string key = keyword.Split('.')[1];
    //        List<CommandCenter.MuseumObject> museumCollectionValues = CommandCenter.museumObjects.Values.ToList();
    //        List<CommandCenter.MuseumObject> listOfExhibits = new List<CommandCenter.MuseumObject>();
    //        switch (filter)
    //        {
    //            case "exhibitions":
    //                listOfExhibits = CommandCenter.museumObjects.Values.Where(e => e.exhibitions.Contains(key)).ToList();
    //                break;
    //            case "themes":
    //                listOfExhibits = CommandCenter.museumObjects.Values.Where(e => e.themes.Contains(key)).ToList();
    //                break;
    //        }
    //        ToggleMuseumObjects(true, listOfExhibits);
    //    }
    //}
    //public void EditVisibles(string keyword)
    //{
    //    Debug.Log(keyword);
    //    if (visibleObjectKeywords.Contains(keyword))
    //    {
    //        visibleObjectKeywords.Remove(keyword);
    //    } else {
    //        visibleObjectKeywords.Add(keyword);
    //    }
    //    UpdateVisibleObjects();
    //    Debug.Log(string.Join(" ", visibleObjectKeywords));
    //}

    //private void PlaceMuseumObjects(List<CommandCenter.MuseumObject> mObjects)
    //{
    //    foreach (CommandCenter.MuseumObject museumObject in mObjects)
    //    {
    //        GameObject indicator = Instantiate(indicatorPrefab);
    //        indicator.name = museumObject.id;
    //        indicator.transform.parent = augmentLayer.transform;
    //        GameObject floor = floors[(int)museumObject.location.y];
    //        indicator.transform.position = new Vector3(museumObject.location.x * floor.GetComponent<Renderer>().bounds.size.x,
    //                                                   floor.transform.position.y + 0.3f,
    //                                                   museumObject.location.z * floor.GetComponent<Renderer>().bounds.size.z);

    //        CommandCenter.ChangeColor(indicator, museumObject.color);
    //    }
    //}
    //private void ToggleMuseumObjects(bool visible, List<CommandCenter.MuseumObject> museumObjects)
    //{
    //    foreach (CommandCenter.MuseumObject museumObject in museumObjects)
    //    {
    //        augmentLayer.transform.Find(museumObject.id).gameObject.SetActive(visible);
    //    }
    //}


    //private void HandleTouch()
    //{
    //}

    //private void HandleMouse()
    //{ 
    //}
}
