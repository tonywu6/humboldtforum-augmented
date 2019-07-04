/*
Copyright 2019 Tony Wu

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using TMPro;

public class ARController_ : MonoBehaviour
{

    //private GameObject camera;

    //private GameObject origin;
    //private GameObject[] floors = new GameObject[4];

    //private string currentRoom;

    //private GameObject indicatorPrefab;
    //private GameObject signPrefab;
    //private GameObject envLayer;
    //private GameObject objLayer;
    //private GameObject augLayer;
    //private GameObject augField;
    //private GameObject canvas;

    //private string[] filters = { "Floors", "Highlights", "CreaturesInCulture", "Power", "TechnoCulture" };
    //private int filterPos = 0;

    //private Dictionary<string, GameObject> museumObjectsGO = new Dictionary<string, GameObject>();
    //private Dictionary<string, GameObject> museumSignsGO = new Dictionary<string, GameObject>();

    //// Start is called before the first frame update
    //void Start()
    //{
    //    origin = transform.Find("Origin").gameObject;

    //    origin.transform.position = CommandCenter.currentLocation;
    //    RelocalizeMuseum(origin.transform);

    //    indicatorPrefab = Resources.Load<GameObject>("Prefabs/MuseumObjectIndicator");
    //    signPrefab = Resources.Load<GameObject>("Prefabs/MuseumObjectSign");

    //    envLayer = transform.Find("Environment").gameObject;
    //    objLayer = transform.Find("Objects").gameObject;
    //    augLayer = transform.Find("Augmentation").gameObject;
    //    canvas = transform.Find("Canvas").gameObject;

    //    augField = Instantiate(Resources.Load<GameObject>("Prefabs/AugmentationField"));
    //    augField.name = "AugmentationField";
    //    augField.transform.parent = augLayer.transform;

    //    for (int i = 0; i < 4; i++)
    //    {
    //        floors[i] = transform.Find("Environment/BerlinerSchlossFloors/F" + i.ToString()).gameObject;
    //    }

    //    camera = transform.Find("CameraParent/Main Camera").gameObject;

    //    PlaceMuseumObjects(CommandCenter.museumObjects);

    //    envLayer.SetActive(false);
    //    objLayer.SetActive(false);
    //    augLayer.SetActive(false);

    //    StartCoroutine(DemonstrationInitialScreen());
    //}

    //private IEnumerator DemonstrationInitialScreen()
    //{
    //    canvas.transform.Find("Status/Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Acquiring location...";
    //    canvas.transform.Find("Topic").gameObject.SetActive(false);
    //    canvas.transform.Find("Alert/Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Please slowly move your phone around...";
    //    canvas.transform.Find("Controls").gameObject.SetActive(false);
    //    yield return new WaitForSeconds(5);
    //    canvas.transform.Find("Status/Text").gameObject.GetComponent<TextMeshProUGUI>().text = "First floor, Foyer";
    //    canvas.transform.Find("Topic").gameObject.SetActive(true);
    //    //canvas.transform.Find("Alert").gameObject.SetActive(false);
    //    canvas.transform.Find("Controls").gameObject.SetActive(true);
    //    augLayer.SetActive(true);
    //    canvas.transform.Find("Alert/Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Please scan around with your phone";
    //    yield return new WaitForSeconds(3);
    //    canvas.transform.Find("Alert").gameObject.SetActive(false);
    //}

    //public void DemonstrationRelocalize(string place)
    //{
    //    if (currentRoom == place) { return; }
    //    Vector4 newMuseumLocation = CommandCenter.museumObjects[place].location;
    //    CommandCenter.currentLocation = newMuseumLocation;

    //    Vector3 reverseTranslation = camera.transform.position - objLayer.transform.Find(place).position;
    //    //origin.transform.position = objLayer.transform.Find(place).position;
    //    //RelocalizeMuseum(origin.transform);
    //    origin.transform.Translate(reverseTranslation, Space.World);
    //    envLayer.transform.Translate(reverseTranslation, Space.World);
    //    objLayer.transform.Translate(reverseTranslation, Space.World);
    //    augLayer.transform.Translate(reverseTranslation, Space.World);
    //    currentRoom = place;

    //    switch (place)
    //    {
    //        case "room.0":
    //            canvas.transform.Find("Status/Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Foyer";
    //            break;
    //        case "room.114":
    //            canvas.transform.Find("Status/Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Humboldt Forum Academy";
    //            break;
    //        case "room.206":
    //            canvas.transform.Find("Status/Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Ethnologisches Museum - America";
    //            break;
    //        case "room.300":
    //            canvas.transform.Find("Status/Text").gameObject.GetComponent<TextMeshProUGUI>().text = "3rd Floor";
    //            break;
    //    }
    //}
    //public void DemonstrationChangeTopic(int step)
    //{
    //    filterPos += step;
    //    if (filterPos == filters.Length) { filterPos = 0; return; }
    //    if (filterPos == -1) { filterPos = filters.Length - 1; return; }
    //}
    //private void RelocalizeMuseum(Transform newTransform)
    //{
    //    UnityARSessionNativeInterface.GetARSessionNativeInterface().SetWorldOrigin(newTransform);
    //}
    //private void PlaceMuseumObjects(Dictionary<string, CommandCenter.MuseumObject> museumObjs)
    //{
    //    foreach (CommandCenter.MuseumObject museumObject in museumObjs.Values)
    //    {
    //        GameObject indicator = Instantiate(indicatorPrefab);
    //        indicator.name = museumObject.id;
    //        indicator.transform.parent = objLayer.transform;
    //        GameObject floor = floors[(int)museumObject.location.y];
    //        indicator.transform.position = new Vector3(museumObject.location.x * floor.GetComponent<Renderer>().bounds.size.x * 10,
    //                                                   floor.transform.position.y + 0.3f,
    //                                                   -museumObject.location.z * floor.GetComponent<Renderer>().bounds.size.z * 10);

    //        indicator.transform.localScale = new Vector3(3, 3, 3);
    //        CommandCenter.ChangeColor(indicator, museumObject.color);

    //        museumObjectsGO.Add(museumObject.id, indicator);

    //        GameObject sign = Instantiate(signPrefab);
    //        sign.name = museumObject.id;
    //        sign.transform.parent = augLayer.transform;

    //        GameObject largeCylinder = sign.transform.Find("LCylinder").gameObject;
    //        GameObject billboard = sign.transform.Find("LabelBoard").gameObject;
    //        TextMeshPro label = sign.transform.Find("Label").GetComponent<TextMeshPro>();

    //        Vector3 oldScale = billboard.gameObject.transform.localScale;

    //        label.text = museumObject.name;
    //        label.ForceMeshUpdate();

    //        CommandCenter.ChangeColor(largeCylinder, museumObject.color);
    //        CommandCenter.ChangeColor(billboard, museumObject.color);
    //        billboard.transform.localScale = new Vector3(label.GetRenderedValues().x / 6 + 0.25f, oldScale.y, oldScale.z);
    //        billboard.transform.position = largeCylinder.transform.position;
    //        billboard.transform.Translate(billboard.transform.localScale.x / 2 + 0.1f, 0, 0);

    //        museumSignsGO.Add(museumObject.id, sign);
    //    }
    //}

    //public void RandomizeLocations()
    //{
    //    foreach (KeyValuePair<string, GameObject> museumObj in museumObjectsGO)
    //    {
    //        if (CommandCenter.museumObjects[museumObj.Key].type != "place")
    //        {
    //            museumObj.Value.transform.position = new Vector3(CommandCenter.museumLength * Random.Range(0f, 1f), 5 * Random.Range(0, 4), CommandCenter.museumWidth * Random.Range(0f, 1f));
    //        }
    //    }
    //}

    //private void RepositionAndFaceCamera(GameObject GO, Vector3 position)
    //{
    //    GO.transform.position = position;
    //    GO.transform.LookAt(camera.transform);
    //    GO.transform.Rotate(Vector3.right, -90 - GO.transform.rotation.eulerAngles.x);
    //    GO.transform.Rotate(Vector3.up, 180);
    //}
    //// Update is called once per frame
    //void Update()
    //{
    //    augField.transform.position = camera.transform.position;

    //    canvas.transform.Find("Topic/Text").gameObject.GetComponent<TextMeshProUGUI>().text = filters[filterPos];
    //    int r = 0;
    //    foreach (KeyValuePair<string, GameObject> museumObj in museumObjectsGO)
    //    {
    //        string mKey = museumObj.Key;
    //        GameObject mObjGO = museumObj.Value;
    //        GameObject mSignGO = museumSignsGO[mKey];

    //        switch (filters[filterPos])
    //        {
    //            case "Floors":
    //                if (CommandCenter.museumObjects[mKey].type != "place") { mSignGO.SetActive(false); continue; }
    //                break;
    //            case "Highlights":
    //                if (CommandCenter.museumObjects[mKey].exhibitions == null || !CommandCenter.museumObjects[mKey].exhibitions.Contains("HFHighlights")) { mSignGO.SetActive(false); continue; }
    //                break;
    //            case "CreaturesInCulture":
    //            case "Power":
    //            case "TechnoCulture":
    //                if (CommandCenter.museumObjects[mKey].themes == null || !CommandCenter.museumObjects[mKey].themes.Contains(filters[filterPos])) { mSignGO.SetActive(false); continue; }
    //                break;
    //        }
    //        r++;

    //        float mObjFloor = CommandCenter.museumObjects[museumObj.Key].location.y;
    //        float mObjRoom = CommandCenter.museumObjects[museumObj.Key].location.w;
    //        float currentFloor = CommandCenter.currentLocation.y;
    //        float currentRoomNum = CommandCenter.currentLocation.w;
    //        int heightModifier;
    //        if ((int)currentFloor == (int)mObjFloor)
    //        {
    //            heightModifier = 0;
    //        }
    //        else if (currentFloor > mObjFloor)
    //        {
    //            heightModifier = 1;
    //        }
    //        else
    //        {
    //            heightModifier = -1;
    //        }

    //        if (Vector3.Distance(camera.transform.position, mObjGO.transform.position) < 0.1)
    //        {
    //            mSignGO.SetActive(false);
    //            continue;
    //        }
    //        mSignGO.SetActive(true);
    //        if (Vector3.Distance(camera.transform.position, new Vector3(mObjGO.transform.position.x, camera.transform.position.y, mObjGO.transform.position.z)) < 30)
    //        {
    //            if ((int) currentFloor == (int)mObjFloor) {
    //                mSignGO.transform.Find("LCylinder").gameObject.SetActive(true);
    //                mSignGO.transform.Find("SCylinder").gameObject.SetActive(true);
    //                RepositionAndFaceCamera(mSignGO, mObjGO.transform.position);
    //                mSignGO.transform.localScale = new Vector3(0.6f, 1, 0.6f);
    //            } else
    //            {
    //                RepositionAndFaceCamera(mSignGO, new Vector3(mObjGO.transform.position.x, camera.transform.position.y + -7.5f * heightModifier + -1.5f * (r % 4), mObjGO.transform.position.z));
    //            }
    //        }
    //        else if (Physics.Linecast(mObjGO.transform.position, camera.transform.position, out RaycastHit hitInfo) && hitInfo.collider.transform.parent.name == "AugmentationField")
    //        {
    //            RepositionAndFaceCamera(mSignGO, new Vector3(hitInfo.point.x, camera.transform.position.y + -7.5f * heightModifier + -1.5f * (r % 4), hitInfo.point.z));
    //            //if (hitInfo.distance > 40)
    //            //{
    //            //    mSignGO.transform.Translate(0, 10, 0);
    //            //} else if (hitInfo.distance > 20)
    //            //{
    //            //    mSignGO.transform.Translate(0, (hitInfo.distance - 20) / 20 * 10, 0);
    //            //}
    //            mSignGO.transform.localScale = new Vector3(2, 1, 2);
    //            mSignGO.transform.Find("LCylinder").gameObject.SetActive(false);
    //            mSignGO.transform.Find("SCylinder").gameObject.SetActive(false);
    //        }

    //        switch (heightModifier)
    //        {
    //            case 1:
    //                mSignGO.transform.Find("ArrowUp").gameObject.SetActive(false);
    //                mSignGO.transform.Find("ArrowDown").gameObject.SetActive(true);
    //                break;
    //            case -1:
    //                mSignGO.transform.Find("ArrowUp").gameObject.SetActive(true);
    //                mSignGO.transform.Find("ArrowDown").gameObject.SetActive(false);
    //                break;
    //            case 0:
    //                mSignGO.transform.Find("ArrowUp").gameObject.SetActive(false);
    //                mSignGO.transform.Find("ArrowDown").gameObject.SetActive(false);
    //                break;
    //        }
    //    }

    //    //Transform foundFriedrichIII = transform.Find("FriedrichIII");

    //    //if (foundFriedrichIII)
    //    //{
    //    //    currentRoom = "room.300";
    //    //    DemonstrationRelocalize("room.300");
    //    //    transform.Find("Augmentation/1961.1.1").Translate(0, 1, 0);
    //    //}

    //}
}
