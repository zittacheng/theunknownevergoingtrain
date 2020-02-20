﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Z
{
    public class ConstructionControl : MonoBehaviour {
        public GameObject RayPoint;
        public GameObject CharacterPoint;
        public GameObject TempObject;
        public Beacon CurrentBeacon;
        [Space]
        public Ray BeaconRay;
        public float MaxDistance;
        public LayerMask BeaconRayMask;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (CurrentBeacon)
                BeaconPositionUpdate();
        }

        public void BeaconPositionUpdate()
        {
            BeaconRay.origin = RayPoint.transform.position;
            BeaconRay.direction = RayPoint.transform.forward;
            RaycastHit[] Hs = Physics.RaycastAll(BeaconRay, MaxDistance, BeaconRayMask);
            List<RaycastHit> Hits = new List<RaycastHit>(Hs);
            if (Hits.Count <= 0)
            {
                CurrentBeacon.SetActive(false);
                return;
            }

            Physics.Raycast(BeaconRay, out RaycastHit RH, 999f, BeaconRayMask);
            CurrentBeacon.SetActive(true);
            Vector3 TempPosition = new Vector3();
            Vector3 TempRotation = new Vector3();
            TempPosition = RH.point;
            TempObject.transform.position = TempPosition;
            if (RH.normal.y > -0.1f && RH.normal.y < 0.1f)
            {
                TempObject.transform.forward = RH.normal;
                TempRotation = new Vector3(0, TempObject.transform.eulerAngles.y, 0);
            }
            else
            {
                if (RH.point.y <= RayPoint.transform.position.y)
                    BeaconRay.origin = RH.point + new Vector3(0, 0.1f, 0);
                else
                    BeaconRay.origin = RH.point - new Vector3(0, 0.1f, 0);
                BeaconRay.direction = new Vector3(BeaconRay.origin.x, 0, BeaconRay.origin.z) - new Vector3(RayPoint.transform.position.x, 0, RayPoint.transform.position.z);
                Physics.Raycast(BeaconRay, out RaycastHit RHII, 999f, BeaconRayMask);
                if (RHII.normal.y > -0.1f && RHII.normal.y < 0.1f)
                {
                    TempPosition = RHII.point;
                    TempObject.transform.position = TempPosition;
                    TempObject.transform.forward = RHII.normal;
                    TempRotation = new Vector3(0, TempObject.transform.eulerAngles.y, 0);
                }
                else
                {
                    TempObject.transform.forward = -RayPoint.transform.forward;
                    TempRotation = new Vector3(0, TempObject.transform.eulerAngles.y, 0);
                    CurrentBeacon.SetActive(false);
                    return;
                }
            }
            Debug.DrawRay(BeaconRay.origin, BeaconRay.direction * MaxDistance, Color.green);

            Ray X = new Ray(TempObject.transform.position - TempObject.transform.right * 0.01f, TempObject.transform.right);
            Ray XII = new Ray(TempObject.transform.position + TempObject.transform.right * 0.01f, -TempObject.transform.right);
            Ray Y = new Ray(TempObject.transform.position + TempObject.transform.up * 0.01f, -TempObject.transform.up);
            Ray Z = new Ray(TempObject.transform.position + TempObject.transform.forward * 0.01f, -TempObject.transform.forward);
            if (Physics.Raycast(Y, out RaycastHit YH, 999f))
            {
                TempPosition = YH.point + new Vector3(0, CurrentBeacon.Size.y, 0);
                TempObject.transform.position = TempPosition;
            }
            else
                CurrentBeacon.SetActive(false);
            if (Physics.Raycast(Z, out RaycastHit ZH, CurrentBeacon.Size.z))
            {
                float z = ZH.distance;
                TempPosition += (CurrentBeacon.Size.z - ZH.distance) * TempObject.transform.forward;
                TempObject.transform.position = TempPosition;
            }
            if (Physics.Raycast(X, out RaycastHit XH, CurrentBeacon.Size.x))
            {
                float x = XH.distance;
                TempPosition -= (CurrentBeacon.Size.x - XH.distance) * TempObject.transform.right;
                TempObject.transform.position = TempPosition;
                if (Physics.Raycast(XII, CurrentBeacon.Size.x))
                {
                    CurrentBeacon.SetActive(false);
                    return;
                }
            }
            else if (Physics.Raycast(XII, out RaycastHit XHII, CurrentBeacon.Size.x))
            {
                float x = XHII.distance;
                TempPosition += (CurrentBeacon.Size.x - XHII.distance) * TempObject.transform.right;
                TempObject.transform.position = TempPosition;
                if (Physics.Raycast(X, CurrentBeacon.Size.x))
                {
                    CurrentBeacon.SetActive(false);
                    return;
                }
            }

            if ((TempPosition - CharacterPoint.transform.position).magnitude > MaxDistance)
            {
                CurrentBeacon.SetActive(false);
                return;
            }

            CurrentBeacon.SetPosition(TempPosition);
            CurrentBeacon.SetRotation(TempRotation);
        }
    }
}