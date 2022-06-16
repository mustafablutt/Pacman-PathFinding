using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletFiller : MonoBehaviour
{
    public int hCells;
    public int vCells;

    public GameObject bottomLeft, topRight;

    public GameObject prefabPellet;

    public GameObject pelletHolder;

    public bool active;

    // public void FillField()
    // {
    //     if (active)
    //     {
    //         return;
    //     }
    //
    //     if (!active)
    //     {
    //         active = true;
    //         hCells = (int) Vector3.Distance(new Vector3(topRight.transform.position.x, 0, 0),
    //             new Vector3(bottomLeft.transform.position.x, 0, 0));
    //         vCells = (int) Vector3.Distance(new Vector3(0, 0, topRight.transform.position.z),
    //             new Vector3(0, 0, bottomLeft.transform.position.z));
    //
    //         for (int i = 0; i < hCells; i++)
    //         {
    //             for (int j = 0; j < vCells; j++)
    //             {
    //                 if (!Physics.CheckSphere(new Vector3(bottomLeft.transform.position.x + i,
    //                     bottomLeft.transform.position.y, bottomLeft.transform.position.z + j), 0.4f))
    //                 {
    //                     GameObject pellet = Instantiate(prefabPellet, new Vector3(bottomLeft.transform.position.x + i,
    //                             bottomLeft.transform.position.y, bottomLeft.transform.position.z + j),
    //                         Quaternion.identity,
    //                         pelletHolder.transform) as GameObject;
    //                 }
    //             }
    //         }
    //
    //         active = false;
    //     }
    // }
}