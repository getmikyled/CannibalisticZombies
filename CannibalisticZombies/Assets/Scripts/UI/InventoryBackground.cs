using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class InventoryBackground : MonoBehaviour
{
    public GameObject InventoryUIText;
    
    // Start is called before the first frame update
    void Start()
    {
        this.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
