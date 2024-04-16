using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropTemplateUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Image propSprite;
    [SerializeField] private GameObject doneUI;
    private bool isDone = false;

    public void UpdateContent(KeyValuePair<PropSO, int> entry) {
        PropSO propSO = entry.Key;
        Dictionary<PropSO, int> deliveredProps = DeliveryManager.Instance.GetDeliveredList();
        SetPropSprite(propSO.propIcon);
        int currentCount = 0;
        if (deliveredProps.ContainsKey(propSO)) {
            currentCount = DeliveryManager.Instance.GetDeliveredList()[propSO];
        }
        SetProgressText(currentCount, entry.Value);

        if (currentCount == entry.Value) {
            isDone = true;
        }
        ShowDoneUI(isDone);
    }

    private void ShowDoneUI(bool value) {
        doneUI.SetActive(value);
    }

    private void SetProgressText(int currentCount, int countToDeliver) {
        progressText.text = $"{currentCount}/{countToDeliver}";
    }

    private void SetPropSprite(Sprite sprite) {
        propSprite.sprite = sprite;
    }

}
