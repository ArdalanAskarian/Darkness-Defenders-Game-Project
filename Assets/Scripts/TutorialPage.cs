using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialManager : MonoBehaviour
{
    public Button nextButton;
    public Button prevButton;
    public Text tutorialText;
    public Text pageNum;
    public RawImage gifImage; // Reference to the Raw Image
    public Texture Image1;
    public Texture Image2;
    public Texture Image3;
    public Texture Image4;
    public Texture Image5;
    public Button close;
    public GameObject tutorialmenu;

    private int currentPage = 0;
    private int totalPages = 5; // Adjust this based on the number of tutorial pages

    private void Start()
    {
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);
        close.onClick.AddListener(CloseMenu);
        UpdateButtons();
        UpdateTutorialText();
    }

    private void NextPage()
    {
        currentPage = Mathf.Min(currentPage + 1, totalPages - 1);
        UpdateButtons();
        UpdateTutorialText();
    }

    private void PrevPage()
    {
        currentPage = Mathf.Max(currentPage - 1, 0);
        UpdateButtons();
        UpdateTutorialText();
    }

    private void UpdateTutorialText()
    {
        if (currentPage == 0)
        {
            tutorialText.text = "They are after you, but they should be running. Use WASD to hunt them down. Space to blast them with evil. There's no need to fear destroying this body, your soul is tied to the source of your powers and you will be reborn at your dark fortress. ";
            pageNum.text = "Page 1/5 of Darkness";
            // Load and assign the image for page 1
            gifImage.texture = Image1;
        }
        else if (currentPage == 1)
        {
            tutorialText.text = "Your powers are not enough, You will need to demolish some trees, and install some dark towers. Click on the tower foundations to increase your evil power.";
            pageNum.text = "Page 2/5 of Darkness";
            // Load and assign the image for page 2
            gifImage.texture = Image2;
        }
        // Add more cases for additional pages if needed
        else if (currentPage == 2)
        {
            tutorialText.text = "Click on your towers to open the upgrade menu, and spend gold pillaged from innocent victims to strengthen your towers.";
            pageNum.text = "Page 3/5 of Darkness";
            // Load and assign the image for page 3
            gifImage.texture = Image3;
        }
        else if (currentPage == 3)
        {
            tutorialText.text = "Click on your castle to increase your powers with the Bones of the Forsaken and summon the Dragon of Spiky Scales (his name is Timmy). Timmy's diet consists of gold, so he will consume your money until you dismiss him.";
            pageNum.text = "Page 4/5 of Darkness";
            // Load and assign the image for page 4
            gifImage.texture = Image4;
        }
        else if (currentPage == 4)
        {
            tutorialText.text = "There are heroes lost in forest looking for you. To guide them to your castle press the start raid button. If their numbers get to be too much you can seek sanctuary in your castle.";
            pageNum.text = "Page 5/5 of Darkness";
            // Load and assign the image for page 5
            gifImage.texture = Image5;
        }
    }

    private void UpdateButtons()
    {
        nextButton.interactable = currentPage < totalPages - 1;
        prevButton.interactable = currentPage > 0;
    }
    private void CloseMenu()
    {
        tutorialmenu.SetActive(false);
    }
}
