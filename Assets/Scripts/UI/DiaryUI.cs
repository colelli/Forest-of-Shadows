using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiaryUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI _pageText;
    [SerializeField] private Button _nextBtn;
    [SerializeField] private Button _prevBtn;

    private void Awake() {
        _nextBtn.onClick.AddListener(() => {
            _pageText.text += "1";
        });
        _prevBtn.onClick.AddListener(() => {
            _pageText.text = "1";
        });
    }

}
