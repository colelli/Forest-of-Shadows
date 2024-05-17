using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {

    public event EventHandler OnPropsListGenerated;
    public event EventHandler OnPropsDelivered;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private PropListSO propList;
    private int propCountToDeliver;
    private Dictionary<PropSO, int> deliveredProps;
    private Dictionary<PropSO, int> propsToDeliver;

    private void Awake() {
        //We check if there is already a Singleton of DeliveryManager
        if (Instance != null && Instance != this) {
            Destroy(this);
            throw new System.Exception($"[{this.name}] >>> An Instance of this Singleton already exists!");
        } else {
            //There are not instances
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        SetupDeliveryManager();
    }
    private void SetupDeliveryManager() {
        deliveredProps = new Dictionary<PropSO, int>();
        propsToDeliver = new Dictionary<PropSO, int>();
        CalculatePropCountToDeliver();
        PopulateDeliverablesList();
    }

    private void CalculatePropCountToDeliver() {
        int deliverAmount = GameManager.Instance.GetCurrentDifficultyData().GetObjToDeliver();
        int maxDeliverAmount = GameManager.Instance.GetCurrentDifficultyData().GetMaxObjToDeliver();
        int currentLevel = 1;
        if(SaveManager.TryReadSavedData<GameSaveData>("game_data", out GameSaveData gameData)){
            currentLevel = gameData.level;
        }
        float multiplier = GameManager.Instance.GetCurrentDifficultyData().GetLevelObjToDeliverMultiplier();

        // calculate correct total based on current level & difficulty
        propCountToDeliver = deliverAmount;
        propCountToDeliver += Mathf.Clamp(Mathf.RoundToInt(deliverAmount * (currentLevel - 1) * multiplier) - deliverAmount, 0, maxDeliverAmount);
    }

    private void PopulateDeliverablesList() {
        int currentPropCounter = 0;
        while(currentPropCounter < propCountToDeliver) {
            // Get a random prop from the list and generate a random quantity
            PropSO propToDeliver = propList.propSOList[UnityEngine.Random.Range(0, propList.propSOList.Count)];
            int quantity = UnityEngine.Random.Range(1, propCountToDeliver - currentPropCounter);

            if (propsToDeliver.ContainsKey(propToDeliver)) {
                // Update current key value
                propsToDeliver[propToDeliver] += quantity;
            } else {
                // Add new key
                propsToDeliver.Add(propToDeliver, quantity);
            }

            currentPropCounter += quantity;
        }
        foreach(KeyValuePair<PropSO, int> entry in propsToDeliver) {
            Debug.Log($"Prop: {entry.Key} | Count: {entry.Value}");
        }
        OnPropsListGenerated?.Invoke(this, EventArgs.Empty);
    }

    public bool DeliverProp(PropBase prop) {

        // Get delivered propSO
        PropSO deliveredPropSO = prop.GetPropSO();

        if (propsToDeliver.ContainsKey(deliveredPropSO)) {
            if (deliveredProps.ContainsKey(deliveredPropSO)){
                // Update count
                deliveredProps[deliveredPropSO]++;
            } else {
                // Add new key
                deliveredProps.Add(deliveredPropSO, 1);
            }
            OnPropsDelivered?.Invoke(this, EventArgs.Empty);
            return true;
        }

        return false;

    }

    public bool CanReturnToLobby() {
        return deliveredProps.Count >= Mathf.Ceil(propCountToDeliver / 2);
    }

    public Dictionary<PropSO, int> GetDeliverablesList() {
        return propsToDeliver;
    }

    public Dictionary<PropSO, int> GetDeliveredList() {
        return deliveredProps;
    }

}