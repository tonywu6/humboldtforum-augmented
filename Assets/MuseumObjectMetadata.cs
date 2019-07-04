﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class MuseumObjectMetadata
{
    public string id;
    public string name;
    public string type;
    public string desc;
    public GameObject GO;

    public JSONObject geometry;
    public JSONObject curatorial;
    public JSONObject relationship;

    public MuseumObjectMetadata(JSONNode metadataJSON)
    {
        id = metadataJSON["id"];
        type = metadataJSON["type"];
        name = metadataJSON["description"]["name"];
        desc = metadataJSON["description"]["desc"];

        geometry = metadataJSON["geometry"].AsObject;
        curatorial = metadataJSON["curatorial"].AsObject;
        relationship = metadataJSON["relationship"].AsObject;
    }

    internal void InstantiateGO(Transform parent)
    {
        GO = new GameObject(id);
        GO.transform.SetParent(parent);
        JSONArray positionArray = geometry["position"].AsArray;
        GO.transform.position = CommandCenter.DenormalizedMuseumVectors(new Vector3(positionArray[0], positionArray[1], positionArray[2]), moveAnchor: true);
        switch (type)
        {
            case "exhibit":
                var controller = GO.AddComponent<MuseumObjectController>();
                controller.metadata = this;
                break;
            case "space":
                JSONArray boxSizeArray = geometry["boxSize"].AsArray;

                var collider = GO.AddComponent<BoxCollider>();
                collider.size = CommandCenter.DenormalizedMuseumVectors(new Vector3(boxSizeArray[0], boxSizeArray[1], boxSizeArray[2]));
                collider.center = Vector3.Scale(collider.size, new Vector3(.5f, .5f, -.5f));
                collider.isTrigger = true;
                break;
        }
    }
}