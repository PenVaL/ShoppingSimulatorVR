using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StocksManager : MonoBehaviour
{
    public GameObject goodsPrefab; // Assign the goods prefab in the Inspector
    public Material goodsMaterial; // Assign the material for the goods prefab in the Inspector

    public void RestoreGoods() {
        DestroyAllChildGoods();

        for (int i = 0; i < transform.childCount; i++) {
            // Instantiate the goods prefab at the correct position and rotation
            GameObject newGoods = Instantiate(goodsPrefab, transform.GetChild(i).position, transform.GetChild(i).rotation, transform.GetChild(i));

            // Check if a material is assigned before applying it to the new goods object
            if (goodsMaterial != null) {
                newGoods.GetComponent<Renderer>().material = goodsMaterial;
            }
        }
    }

    public void DestroyAllChildGoods() {
        // Iterate through all the child objects of the StockPlaceHolders GameObject
        for (int i = 0; i < transform.childCount; i++) {
            // Check if the current child object has a child object
            if (transform.GetChild(i).childCount > 0) {
                // Destroy the first child object of the current child object
                Destroy(transform.GetChild(i).GetChild(0).gameObject);
            }
        }
    }
}