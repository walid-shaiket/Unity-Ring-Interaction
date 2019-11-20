﻿//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using TMPro; // Add the TextMesh Pro namespace to access the various functions.

public class DataPlotter : MonoBehaviour
{

    // Name of the input file, no extension
    public string filename;

    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;

    // Indices for columns to be assigned
    public int column1 = 0;
    public int column2 = 1;
    public int column3 = 2;
    public int column4 = 3;
    public int column5 = 4;
   
    // Full column names
    public string xName;
    public string yName;
    public string zName;
    public string pName;
    public string qName;

    public float plotScale = 10;

    // The prefab for the data points that will be instantiated
    public GameObject PointPrefab;

    // Object which will contain instantiated prefabs in hiearchy
    public GameObject PointHolder;


    //private TextMeshPro m_Text;
    //private TextContainer m_TextContainer;

    // Use this for initialization
    void Start()
    {
        /*
        // Get a reference to an existing TextMeshPro component or Add one if needed.
        m_Text = GetComponent<TextMeshPro>(); //?? gameObject.AddComponent<TextMeshPro>();

        // Get a reference to the text container. Alternatively, you can now use the RectTransform on the text object instead.
        m_TextContainer = GetComponent<TextContainer>();
        m_TextContainer.width = 25f;
        m_TextContainer.height = 3f;

        // Set the point size
        m_Text.fontSize = 24;

        // Set the text
        m_Text.text = "A simple line of text.";

        // Set the text of the attached Text mesh
        //GetComponent<TextMeshPro>().text = "scatter plot of iris data";
        */
        // Set pointlist to results of function Reader with argument inputfile
        pointList = CSVReader.Read(filename);

        //Log to console
        Debug.Log(pointList);

        // Declare list of strings, fill with keys (column names)
        List<string> columnList = new List<string>(pointList[1].Keys);

        // Print number of keys (using .count)
        Debug.Log("There are " + columnList.Count + " columns in the CSV");

        foreach (string key in columnList)
            Debug.Log("Column name is " + key);

        // Assign column name from columnList to Name variables
        xName = columnList[column1];
        yName = columnList[column2];
        zName = columnList[column3];
        pName = columnList[column4];
        qName = columnList[column5];

        // Get maxes of each axis
        float xMax = FindMaxValue(xName);
        float yMax = FindMaxValue(yName);
        float zMax = FindMaxValue(zName);
        //float pMax = FindMaxValue(pName);
        //float qMax = FindMaxValue(qName);

        // Get minimums of each axis
        float xMin = FindMinValue(xName);
        float yMin = FindMinValue(yName);
        float zMin = FindMinValue(zName);
       //float pMin = FindMinValue(pName);
        //float qMin = FindMinValue(qName);


        //Loop through Pointlist
        for (var i = 0; i < pointList.Count; i++)
        {
            // Get value in poinList at ith "row", in "column" Name, normalize
            float x =
                (System.Convert.ToSingle(pointList[i][xName]) - xMin)
                / (xMax - xMin);

            float y =
                (System.Convert.ToSingle(pointList[i][yName]) - yMin)
                / (yMax - yMin);

            float z =
               (System.Convert.ToSingle(pointList[i][zName]) - zMin)
               / (zMax - zMin);

            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    PointPrefab,
                    new Vector3(x, y, z) * plotScale,
                    Quaternion.identity);

            // Make child of PointHolder object, to keep points within container in hiearchy
            dataPoint.transform.parent = PointHolder.transform;

            // Assigns original values to dataPointName

            string dataPointName =
                pointList[i][xName] + " "
                + pointList[i][yName] + " "
                + pointList[i][zName] + " "
                + pointList[i][pName] + " "
                + pointList[i][qName] + " ";

            // Assigns name to the prefab
            dataPoint.transform.name = dataPointName;

            // Gets material color and sets it to a new RGB color we define
            dataPoint.GetComponent<Renderer>().material.color =
                new Color(x, y, z, 1.0f);
        }

    }

    private float FindMaxValue(string columnName)
    {
        //set initial value to first value
        float maxValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing maxValue if new value is larger
        for (var i = 0; i < pointList.Count; i++)
        {
            if (maxValue < Convert.ToSingle(pointList[i][columnName]))
                maxValue = Convert.ToSingle(pointList[i][columnName]);
        }

        //Spit out the max value
        return maxValue;
    }

    private float FindMinValue(string columnName)
    {

        float minValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing minValue if new value is smaller
        for (var i = 0; i < pointList.Count; i++)
        {
            if (Convert.ToSingle(pointList[i][columnName]) < minValue)
                minValue = Convert.ToSingle(pointList[i][columnName]);
        }

        return minValue;
    }
      
}