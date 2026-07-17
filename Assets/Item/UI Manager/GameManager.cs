using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI ArrowText;
    public TextMeshProUGUI SilverKeyText;
    public Slider LifeSlider;
    public Slider PowerSlider;
    public Image mainImage;
    public GameObject retryButton;
    public Sprite gameOverSpr;
    public Sprite gameClearSpr;

    public void UpdateItemCount(ItemType itemtype, int count)
    {
        if (itemtype == ItemType.Arrow)
        {
            ArrowText.text = count.ToString();
        }
        else if (itemtype == ItemType.SilverKey)
        {
            SilverKeyText.text = count.ToString();
        }
    
    }

    public void UpdateLife(float life)
    {
        if (life <= 0)
        {
            retryButton.SetActive(true);
            mainImage.gameObject.SetActive(true) ;
            mainImage.sprite = gameOverSpr; 
        }
        LifeSlider.value = life;
    }

    public void UpdatePower(float power)
    {
        PowerSlider.value = power;
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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("InactiveImage", 1.0f);
        retryButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
