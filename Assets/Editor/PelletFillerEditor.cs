using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PelletFiller))]
[CanEditMultipleObjects]
public class PelletFillerEditor : Editor
{
    public GameObject myPrefab;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PelletFiller filler = (PelletFiller) target;

        myPrefab = filler.prefabPellet;

        if (GUILayout.Button("Fill Field"))
        {
            //filler.FillField();

            if (filler.active)
            {
                return;
            }

            if (!filler.active)
            {
                filler.active = true;
                filler.hCells = (int) Vector3.Distance(new Vector3(filler.topRight.transform.position.x, 0, 0),
                    new Vector3(filler.bottomLeft.transform.position.x, 0, 0));
                filler.vCells = (int) Vector3.Distance(new Vector3(0, 0, filler.topRight.transform.position.z),
                    new Vector3(0, 0, filler.bottomLeft.transform.position.z));

                for (int i = 0; i < filler.hCells; i++)
                {
                    for (int j = 0; j < filler.vCells; j++)
                    {
                        if (!Physics.CheckSphere(new Vector3(filler.bottomLeft.transform.position.x + i,
                            filler.bottomLeft.transform.position.y, filler.bottomLeft.transform.position.z + j), 0.4f))
                        {
                            // GameObject pellet = Instantiate(filler.prefabPellet, new Vector3(filler.bottomLeft.transform.position.x + i,
                            //         filler.bottomLeft.transform.position.y, filler.bottomLeft.transform.position.z + j),
                            //     Quaternion.identity,
                            //     filler.pelletHolder.transform) as GameObject;
                            GameObject pellet = PrefabUtility.InstantiatePrefab(myPrefab) as GameObject;
                            pellet.transform.position = new Vector3(filler.bottomLeft.transform.position.x + i,
                                filler.bottomLeft.transform.position.y, filler.bottomLeft.transform.position.z + j);
                            pellet.transform.parent = filler.pelletHolder.transform;
                        }
                    }
                }

                filler.active = false;
            }

        }
    }
}