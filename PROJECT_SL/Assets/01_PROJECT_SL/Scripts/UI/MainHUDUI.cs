using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectSL
{
    public class MainHUDUI : UIBase
    {
        public Image healthBar;
        public Image staminaBar;
        
        public void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }

        public void UpdateStaminaBar(float currentStamina, float maxStamina)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }
    }
}
