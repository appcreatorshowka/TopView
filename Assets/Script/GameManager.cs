using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI arrowText;
    public TextMeshProUGUI SilverKeyText;
    public TextMeshProUGUI lightText;
    public Slider LifeSlider;
    public Slider powerSlider;
    public Image mainImage;
    public GameObject retryButton;
    public Sprite gameOverSpr;
    public Sprite gameClearSpr;

    public void UpdateLife(float life)
    {
        if (life <= 0)
        {
            retryButton.SetActive(true);
            mainImage.gameObject.SetActive(true);
            mainImage.sprite = gameOverSpr;
        }
        LifeSlider.value = life;
    }

    public void UpdatePower(float power)
    {
        powerSlider.value = power;
    }

    public void UpdateItemCount(ItemType itemtype, int count)
    {

        if (itemtype == ItemType.Arrow)
        {
            arrowText.text = count.ToString();
        }
        else if (itemtype == ItemType.SilverKey)
        {
            SilverKeyText.text = count.ToString();
        }
        else if (itemtype == ItemType.Light)
        {
            lightText.text = count.ToString();
        }
    }

    public void Retry()
    {
        PlayerController.life = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void InactiveImage()
    {
        mainImage.gameObject.SetActive(false);

    }

    void Start()
    {
        Invoke("InactiveImage", 1.0f);
        retryButton.SetActive(false);
    }

}
