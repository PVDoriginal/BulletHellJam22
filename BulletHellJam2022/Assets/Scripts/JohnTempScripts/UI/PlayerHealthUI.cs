using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] TMP_Text playerHealthUI;
    [SerializeField] PlayerHealth playerHealth;

    private void Update()
    {
        playerHealthUI.text = "Health: " + playerHealth.GetHealth();
    }
}
