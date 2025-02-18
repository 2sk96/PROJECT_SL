using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectSL
{
    public class MainHUDUI : UIBase
    {
        public Image healthBar;
        public Image staminaBar;
        public TextMeshProUGUI bulletText;
        
        public void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }

        public void UpdateStaminaBar(float currentStamina, float maxStamina)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }

        public void UpdateDisplayMag(float currentMagazine, float magazineSize)
        {
            bulletText.text = currentMagazine.ToString() + "/" + magazineSize.ToString();
        }
    }
}
