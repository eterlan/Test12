using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Init();
            Test1();
        }
    }

    private Exam                      exam;
    private Dictionary<string, Exam.ItemData> allItem = new();
    
    private void Init()
    {
        exam = new Exam();
        allItem = new Dictionary<string, Exam.ItemData>{
            {"A", new Exam.ItemData{id = 1, costGold = 26, count = 0}}, 
            {"B", new Exam.ItemData{id = 2, costGold = 53, count = 0}}, 
            {"C", new Exam.ItemData{id = 3, costGold = 0, count = 2}},
            {"D", new Exam.ItemData{id = 4, costGold = 58, count = 0}},
            {"E", new Exam.ItemData{id = 5, costGold = 0, count = 14}},
            {"F", new Exam.ItemData{id = 6, costGold = 0, count = 70}},
            {"G", new Exam.ItemData{id = 7, costGold = 0, count = 72}},
        };
        allItem["A"].materialList = new List<Exam.MaterialData>{
            new(){ item = allItem["B"], count = 3},
            new(){ item = allItem["C"], count = 1 },
            new(){ item = allItem["D"], count = 4 }
        };
        allItem["B"].materialList = new List<Exam.MaterialData>{
            new(){ item = allItem["E"], count = 1},
            new(){ item = allItem["F"], count = 5 },
        };
        allItem["D"].materialList = new List<Exam.MaterialData>{
            new(){ item = allItem["G"], count = 9},
            new(){ item = allItem["B"], count = 1 },
        };
    }

    private void Test1()
    {
        var result = exam.Run(allItem["A"], 1300);
        Debug.Log(result);
    }
}


