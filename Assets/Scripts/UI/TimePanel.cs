using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimePanel : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TimeManager timeManager;

    public void Update()
    {
        timeText.text = timeManager.gameTimeText;
    }
}
