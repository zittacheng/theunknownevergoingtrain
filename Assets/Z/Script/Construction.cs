﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Z
{
    public class Construction : MonoBehaviour {
        public float MaxProgression;
        public float Progression;
        public List<GameObject> MeshBases;
        [Tooltip("Exclusive")]
        public List<float> MaxValues;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangeProgression(float Value)
        {
            Progression += Value;
            if (Progression > MaxProgression)
                Progression = MaxProgression;
            ProgressionUpdate();
        }

        public void ProgressionUpdate()
        {
            float Rate = Progression / MaxProgression;
            for (int i = 0; i < MeshBases.Count; i++)
            {
                if (Rate >= MaxValues[i])
                    continue;
                SetMesh(i);
            }
        }

        public void SetMesh(int Index)
        {
            if (MeshBases[Index].activeSelf)
                return;
            for (int i = 0; i < MeshBases.Count; i++)
            {
                if (i == Index)
                    MeshBases[i].SetActive(true);
                else
                    MeshBases[i].SetActive(false);
            }
        }
    }
}