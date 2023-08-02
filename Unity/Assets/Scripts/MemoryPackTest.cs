using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MemoryPack;


public class MemoryPackTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var v = new Person { Age = 40, Name = "Johncccccccccccccccccccccccc" };

        var bin = MemoryPackSerializer.Serialize(v);
        Debug.Log(string.Join(",",bin));
        var val = MemoryPackSerializer.Deserialize<Person>(bin);
        Debug.Log(JsonHelper.ToJson(val));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


[MemoryPackable]
public partial class Person
{
    public int Age { get; set; }
    public string Name { get; set; }
}