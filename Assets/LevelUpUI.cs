using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KA
{
    public class LevelUpUI : MonoBehaviour
    {
        public PlayerManager playerManager;

        public Button confirmLevelUpButton;

        [Header("Player Level")]
        public int currentPlayerLevel;
        public int projectedPlayerLevel;
        public TMP_Text currentPlayerLevelText;
        public TMP_Text projectedPlayerLevelText;

        [Header("Souls")]
        public TMP_Text currentSoulsText;
        public TMP_Text soulsRequiredToLevelUpText;
        private int soulsRequiredToLevelUp;
        public int baseLevelUpCost;

        [Header("Health")]
        public Slider healthSlider;
        public TMP_Text currentHealthLevelText;
        public TMP_Text projectedHealthLevelText;

        [Header("Stamina")]
        public Slider staminaSlider;
        public TMP_Text currentStaminaLevelText;
        public TMP_Text projectedStaminaLevelText;

        [Header("Focus")]
        public Slider focusSlider;
        public TMP_Text currentFocusLevelText;
        public TMP_Text projectedFocusLevelText;

        [Header("Strength")]
        public Slider strengthSlider;
        public TMP_Text currentStrengthLevelText;
        public TMP_Text projectedStrengthLevelText;

        [Header("Dexterity")]
        public Slider dexteritySlider;
        public TMP_Text currentDexterityLevelText;
        public TMP_Text projectedDexterityLevelText;

        [Header("Faith")]
        public Slider faithSlider;
        public TMP_Text currentFaithLevelText;
        public TMP_Text projectedFaithLevelText;

        [Header("Poise")]
        public Slider poiseSlider;
        public TMP_Text currentPoiseLevelText;
        public TMP_Text projectedPoiseLevelText;

        [Header("Intelligence")]
        public Slider intelligenceSlider;
        public TMP_Text currentIntelligenceLevelText;
        public TMP_Text projectedIntelligenceLevelText;

        private void OnEnable()
        {
            currentPlayerLevel = playerManager.playerStats.playerLevel;
            currentPlayerLevelText.text = currentPlayerLevel.ToString();

            projectedPlayerLevel = playerManager.playerStats.playerLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            healthSlider.value = playerManager.playerStats.healthLevel;
            healthSlider.minValue = playerManager.playerStats.healthLevel;
            healthSlider.maxValue = 99;
            currentHealthLevelText.text = playerManager.playerStats.healthLevel.ToString();
            projectedHealthLevelText.text = playerManager.playerStats.healthLevel.ToString();

            staminaSlider.value = playerManager.playerStats.staminaLevel;
            staminaSlider.minValue = playerManager.playerStats.staminaLevel;
            staminaSlider.maxValue = 99;
            currentStaminaLevelText.text = playerManager.playerStats.staminaLevel.ToString();
            projectedStaminaLevelText.text = playerManager.playerStats.staminaLevel.ToString();

            focusSlider.value = playerManager.playerStats.focusLevel;
            focusSlider.minValue = playerManager.playerStats.focusLevel;
            focusSlider.maxValue = 99;
            currentFocusLevelText.text = playerManager.playerStats.focusLevel.ToString();
            projectedStaminaLevelText.text = playerManager.playerStats.focusLevel.ToString();

            poiseSlider.value = playerManager.playerStats.poiseLevel;
            poiseSlider.minValue = playerManager.playerStats.poiseLevel;
            poiseSlider.maxValue = 99;
            currentPoiseLevelText.text = playerManager.playerStats.poiseLevel.ToString();
            projectedStaminaLevelText.text = playerManager.playerStats.poiseLevel.ToString();

            strengthSlider.value = playerManager.playerStats.strengthLevel;
            strengthSlider.minValue = playerManager.playerStats.strengthLevel;
            strengthSlider.maxValue = 99;
            currentStrengthLevelText.text = playerManager.playerStats.strengthLevel.ToString();
            projectedStrengthLevelText.text = playerManager.playerStats.strengthLevel.ToString();

            dexteritySlider.value = playerManager.playerStats.dexterityLevel;
            dexteritySlider.minValue = playerManager.playerStats.dexterityLevel;
            dexteritySlider.maxValue = 99;
            currentDexterityLevelText.text = playerManager.playerStats.dexterityLevel.ToString();
            projectedDexterityLevelText.text = playerManager.playerStats.dexterityLevel.ToString();

            intelligenceSlider.value = playerManager.playerStats.intelligenceLevel;
            intelligenceSlider.minValue = playerManager.playerStats.intelligenceLevel;
            intelligenceSlider.maxValue = 99;
            currentIntelligenceLevelText.text = playerManager.playerStats.intelligenceLevel.ToString();
            projectedIntelligenceLevelText.text = playerManager.playerStats.intelligenceLevel.ToString();

            faithSlider.value = playerManager.playerStats.faithLevel;
            faithSlider.minValue = playerManager.playerStats.faithLevel;
            faithSlider.maxValue = 99;
            currentFaithLevelText.text = playerManager.playerStats.faithLevel.ToString();
            projectedFaithLevelText.text = playerManager.playerStats.faithLevel.ToString();

            currentSoulsText.text = playerManager.playerStats.soulCount.ToString();

            UpdateProjectedPlayerLevel();
        }

        public void ConfirmPlayerLevelUpStats()
        {
            playerManager.playerStats.playerLevel = projectedPlayerLevel;
            playerManager.playerStats.healthLevel = Mathf.RoundToInt(healthSlider.value);
            playerManager.playerStats.staminaLevel = Mathf.RoundToInt(staminaSlider.value);
            playerManager.playerStats.focusLevel = Mathf.RoundToInt(focusSlider.value);
            playerManager.playerStats.poiseLevel = Mathf.RoundToInt(poiseSlider.value);
            playerManager.playerStats.staminaLevel = Mathf.RoundToInt(staminaSlider.value);
            playerManager.playerStats.dexterityLevel = Mathf.RoundToInt(dexteritySlider.value);
            playerManager.playerStats.faithLevel = Mathf.RoundToInt(faithSlider.value);
            playerManager.playerStats.intelligenceLevel = Mathf.RoundToInt(intelligenceSlider.value);

            playerManager.playerStats.maxHealth = playerManager.playerStats.SetMaxHealthFromHealthLevel();
            playerManager.playerStats.maxStamina = playerManager.playerStats.SetMaxStaminaFromStaminaLevel();
            playerManager.playerStats.maxFocusPoints = playerManager.playerStats.SetMaxFocusPointsFromFocusLevel();

            playerManager.playerStats.soulCount = playerManager.playerStats.soulCount - soulsRequiredToLevelUp;
            playerManager.uIManager.soulCount.text = playerManager.playerStats.soulCount.ToString();

            gameObject.SetActive(false);
        }

        private void CalculateSoulCostToLevelUp()
        {
            for (int i = 0; i < projectedPlayerLevel; i++)
            {
                soulsRequiredToLevelUp = soulsRequiredToLevelUp + Mathf.RoundToInt((projectedPlayerLevel + baseLevelUpCost) * 1.5f);
            }
        }

        private void UpdateProjectedPlayerLevel()
        {
            soulsRequiredToLevelUp = 0;

            projectedPlayerLevel = currentPlayerLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(healthSlider.value) - playerManager.playerStats.healthLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(staminaSlider.value) - playerManager.playerStats.staminaLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(focusSlider.value) - playerManager.playerStats.focusLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(poiseSlider.value) - playerManager.playerStats.poiseLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(strengthSlider.value) - playerManager.playerStats.staminaLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(dexteritySlider.value) - playerManager.playerStats.dexterityLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(intelligenceSlider.value) - playerManager.playerStats.intelligenceLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(faithSlider.value) - playerManager.playerStats.faithLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            CalculateSoulCostToLevelUp();
            soulsRequiredToLevelUpText.text = soulsRequiredToLevelUp.ToString();

            if (playerManager.playerStats.soulCount < soulsRequiredToLevelUp)
            {
                confirmLevelUpButton.interactable = false;
            }
            else
            {
                confirmLevelUpButton.interactable = true;
            }
        }

        public void UpdateHealthLevelSlider()
        {
            projectedHealthLevelText.text = healthSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateStaminaLevelSlider()
        {
            projectedStaminaLevelText.text = staminaSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateFocusLevelSlider()
        {
            projectedFocusLevelText.text = focusSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdatePoiseLevelSlider()
        {
            projectedPoiseLevelText.text = poiseSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateStrengthLevelSlider()
        {
            projectedStrengthLevelText.text = strengthSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateDexterityLevelSlider()
        {
            projectedDexterityLevelText.text = dexteritySlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateFaithLevelSlider()
        {
            projectedFaithLevelText.text = faithSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateIntelligenceLevelSlider()
        {
            projectedIntelligenceLevelText.text = intelligenceSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
    }
}

