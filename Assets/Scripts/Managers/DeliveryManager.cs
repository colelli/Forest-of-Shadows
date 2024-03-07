using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private PropListSO propList;
    private int propCountToDeliver;
    private int deliveredProps;
    private List<PropSO> propsToDeliver;

    private void Awake() {
        //We check if there is already a Singleton of DeliveryManager
        if (Instance != null && Instance != this) {
            Destroy(this);
            throw new System.Exception($"[{this.name}] >>> An Instance of this Singleton already exists!");
        } else {
            //There are not instances
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        SetupDeliveryManager();
    }
    private void SetupDeliveryManager() {
        deliveredProps = 0;
        propsToDeliver = new List<PropSO>();
        CalculatePropCountToDeliver();
        PopulateDeliverablesList();
    }

    private void CalculatePropCountToDeliver() {
        //TO-DO write calculate method based on current day
        propCountToDeliver = 3;
    }

    private void PopulateDeliverablesList() {
        for(int i = 0; i < propCountToDeliver; i++) {
            propsToDeliver.Add(propList.propSOList[UnityEngine.Random.Range(0, propList.propSOList.Count)]);
        }
    }

    public List<PropSO> GetDeliverablesList() {
        return propsToDeliver;
    }

    public bool DeliverProp(PropBase prop) {

        for(int i = 0; i < propsToDeliver.Count; i++) {

            PropSO propSO = propsToDeliver[i];

            if (propSO == prop.GetPropSO()) {

                propsToDeliver.RemoveAt(i);
                prop.DestroySelf();
                deliveredProps++;

                return true;

            }

        }

        return false;

    }

}