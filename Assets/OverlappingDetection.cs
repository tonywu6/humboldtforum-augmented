﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlappingDetection : MonoBehaviour
{
    internal int overlappingCount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) overlappingCount++;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10) overlappingCount--;
    }
    private void OnDisable()
    {
        overlappingCount = 0;
    }
}