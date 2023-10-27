using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    private Canvas canvas;
    private Slider slider;
    public float hideDelay = 3.0f;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        slider = GetComponent<Slider>();
        slider.value = 1;
        HideHealthBar();
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth / maxHealth;
        ShowHealthBar();
        StartCoroutine(HideHealthBarAfterDelay());
    }

    private void ShowHealthBar()
    {
        canvas.gameObject.SetActive(true);
    }

    private void HideHealthBar()
    {
        canvas.gameObject.SetActive(false);
    }

    private IEnumerator HideHealthBarAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        HideHealthBar();
    }
}
