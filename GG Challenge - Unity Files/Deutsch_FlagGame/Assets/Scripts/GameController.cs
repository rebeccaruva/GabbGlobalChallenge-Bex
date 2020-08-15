/////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////
// 
// Code by Bex Ruvalcaba
// @bexthehexx
// bexthehexx.graphics
//
/////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    ///////////
    //privates
    ///////////

    //first globe scene vars
    private bool zoomed = false;
    private Color blackColor = new Color(0f, 0f, 0f, 1f);

    //europe info scene vars
    private int infoCount = 0;

    //flag names quiz vars
    private int flagNamesCount = 0;
    private string[] wrongNames = {"Deutschland", "Liechtenstein", "Belgien", "Die Schweiz", "Österreich", "Luxembourg"};

    //second globe colors scene vars
    private Hashtable colorNames = new Hashtable();

    //assign flag names and colors quiz vars
    private int countriesCount = 0;
    private bool assignEnd = false;

    ///////////
    //publics
    ///////////

    //first globe scene vars
    public List<Sprite> flags = new List<Sprite>(6);
    public GameObject prompt;
    public GameObject globe;
    public GameObject dots;
    public Text promptText;
    public GameObject flag; 
    public GameObject nextButton;
    public List<GameObject> dotsChildren = new List<GameObject>(6);

    //europe info scene vars
    public List<GameObject> infosEur = new List<GameObject>(3);

    //flag names quiz vars
    public Text a1Text;
    public Text a2Text;
    public GameObject answer1;
    public GameObject answer2;
    public GameObject popup;

    //second globe scene vars
    public Text colorText;

    //assign flag names and colors quiz vars
    public List<GameObject> countriesButtons = new List<GameObject>(6);
    public List<GameObject> colorsButtons = new List<GameObject>(7);
    public GameObject colorsToActivate;
    public GameObject countriesToDeactivate;
    public Text namesColorsTitle;

    //namibia info scene var
    public List<GameObject> infosNam = new List<GameObject>(7);


    void Start() {
        ////////////////////////////////////
        //
        // only for scene 4 - flag names quiz
        //
        ////////////////////////////////////

        //go through flags and answers in flag name quiz
        if(SceneManager.GetActiveScene().name == "4-FlagsName") {
            flagNamesQuiz();
        }

        ////////////////////////////////////
        //
        // only for second globe scene and assign names and colors quiz
        //
        ////////////////////////////////////

        // add to hashtable, only relevant for colors globe scene
        if(SceneManager.GetActiveScene().name == "5-Globe" || SceneManager.GetActiveScene().name == "6-NamesAndColors") {
            colorNames.Add("Deutschland", "schwarz, rot, gelb");
            colorNames.Add("Liechtenstein", "dunkelblau, rot");
            colorNames.Add("Belgien", "schwarz, gold, rot");
            colorNames.Add("Die Schweiz", "rot, weiß");
            colorNames.Add("Österreich", "rot, weiß, rot");
            colorNames.Add("Luxembourg", "rot, weiß, hellblau");  
        }

        ////////////////////////////////////
        //
        // only for assign names and colors quiz
        //
        ////////////////////////////////////

        //go through flag names in assigning game
        if(SceneManager.GetActiveScene().name == "6-NamesAndColors") {
            countriesAssignQuiz();
        } 
    }

    void Update() {

        ////////////////////////////////////
        //
        // get raycast of button
        // use touches instead when going to mobile
        //
        ////////////////////////////////////

        if (Input.GetMouseButtonDown(0)) {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            
            if (hit.collider != null) {

                ////////////////////////////////////
                //
                // only for first globe scene
                //
                ////////////////////////////////////

                // zoom and select countries in globe scene
                if(SceneManager.GetActiveScene().name == "2-Globe") {

                    if(!zoomed) {
                        //check if first time zoom, if yes
                        if (hit.collider.gameObject.tag == "Zoom") {
                            zoomView(); //go to zoom view
                            Destroy(hit.collider.gameObject); //destroy empty zoom collider
                        }
                    } else { //if already zoomed
                        // check if user selects a country
                        if (hit.collider.gameObject.tag == "country") {
                            //show country info
                            selectCountry(hit.collider.name, false);
                        }
                    }
                }

                ////////////////////////////////////
                //
                // only for europe info scene
                //
                ////////////////////////////////////

                // continue through info in europe info scene
                if(SceneManager.GetActiveScene().name == "3-Europe") {
                    if(hit.collider.gameObject.tag == "EuropeNext") {
                        //if next button hit increase count
                        infoCount++;
                        if (infoCount < 3) {
                            //update shown info based on count
                            infosEur[infoCount-1].SetActive(false);
                            infosEur[infoCount].SetActive(true);
                        } else {
                            //once done change scenes
                            SceneManager.LoadScene("4-FlagsName");
                        }
                    }
                }

                ////////////////////////////////////
                //
                // only for second globe scene
                //
                ////////////////////////////////////

                // zoom into map once more
                if(SceneManager.GetActiveScene().name == "5-Globe") {

                    if(!zoomed) {
                        //check if first time zoom
                        if (hit.collider.gameObject.tag == "Zoom") {
                            zoomView();
                            Destroy(hit.collider.gameObject); //destroy empty zoom collider
                        }
                    } else {//if already zoomed
                         if (hit.collider.gameObject.tag == "country") {
                             //show country info
                            selectCountry(hit.collider.name, true);
                        }
                    }
                }

                ////////////////////////////////////
                //
                // only for namibia info scene
                //
                ////////////////////////////////////

                // continue through namibia history info scene
                if(SceneManager.GetActiveScene().name == "7-Namibia") {
                    if(hit.collider.gameObject.tag == "NamibiaNext") {
                        //if next button hit increase count
                        infoCount++;
                        if (infoCount < 7) {
                            //update shown info based on count
                            infosNam[infoCount-1].SetActive(false);
                            infosNam[infoCount].SetActive(true);
                        } else {
                            //once done change scenes, aka go back to start
                            SceneManager.LoadScene("1-Willkomen");
                        }
                    }
                }
            }
        }
    }

    ////////////////////////////////////
    //
    // "zoom" in for globe scenes
    //
    ////////////////////////////////////

    void zoomView() {
        zoomed = true;

        //"zoom" in on globe
        globe.transform.localPosition = new Vector3(640, -500, 0);
        globe.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(3500, 3500);
        
        //"zoom" in on dots
        dots.transform.localScale = new Vector2(5, 5); //make each dot 50 by 50 in same formation
        dots.transform.localPosition = new Vector3 (60, 165, 0);
    }

    ////////////////////////////////////
    //
    // show country info on globe scenes
    //
    ////////////////////////////////////

    void selectCountry(string country, bool colors) {
        //update prompt to show country name
        promptText.text = country;

        if (colors) {
            //if in second globe, show colors text as well
            colorText.text = (string)colorNames[country];
        }

        // recolor to "selected" state
        for (int i = 0; i < dotsChildren.Count; i++) {
            if(dotsChildren[i].name == country) {
                dotsChildren[i].GetComponent<Image>().color = blackColor;
            }
        }

        //show the flag
        displayFlag(country);

        // check if all have been selected
        int selectedCount = 0;
        for (int i = 0; i < dotsChildren.Count; i++) {
            if(dotsChildren[i].GetComponent<Image>().color == blackColor) {
                selectedCount++;
            }
            if (selectedCount == 6) {
                // if yes, show the next button
                nextButton.SetActive(true);
            }
        }

    }

    ////////////////////////////////////
    //
    // flag names quiz
    //
    ////////////////////////////////////

    public void flagNamesQuiz() {
        //reset interactivity with button
        answer1.GetComponent<Button>().interactable = true;
        answer2.GetComponent<Button>().interactable = true;

        //if still going through questions
        if(flagNamesCount < 6) {
            //show the flag in question
            displayFlag(flags[flagNamesCount].name);

            //randomly display correct answer on screen
            int correctAnswer = Random.Range(1, 3);
            if (correctAnswer == 1) {
                a1Text.text = flags[flagNamesCount].name;
                answer1.tag = "correct";
                a2Text.text = wrongNames[flagNamesCount];
                answer2.tag = "wrong";
            } else {
                a1Text.text = wrongNames[flagNamesCount];
                answer1.tag = "wrong";
                a2Text.text = flags[flagNamesCount].name;
                answer2.tag = "correct";
            }
            //update count
            flagNamesCount++;
        } else {
            //if done, show pop up
            finishPopUp();
        }
    }

    ////////////////////////////////////
    //
    // assign country/flag names quiz
    //
    ////////////////////////////////////

    public void countriesAssignQuiz() {
        // go through each button and reset their interactivity
        for(int i = 0; i<countriesButtons.Count; i++) {
            countriesButtons[i].GetComponent<Button>().interactable = true;
        }

        //if still going through questions
        if(countriesCount < 6) {
            //show the flag in question
            displayFlag(flags[countriesCount].name);

            //go through each button and assign correct/wrong answers
            for(int i = 0; i < countriesButtons.Count; i++) {
                if(flags[countriesCount].name == countriesButtons[i].name) {
                    countriesButtons[i].tag = "correct";
                } else {
                    countriesButtons[i].tag = "wrong";
                }
            }
            //update count
            countriesCount++;
        } else {
            //go to popup, which will redirect to colors after resetting some vars
            finishPopUp();
        }
    }

    ////////////////////////////////////
    //
    // assign flag colors quiz
    //
    ////////////////////////////////////

    public void colorsAssignQuiz() {
        //go through each button and reset their interactivity
        for(int i = 0; i<colorsButtons.Count; i++) {
            colorsButtons[i].GetComponent<Button>().interactable = true;
        }

        //if still going through questions
        if(countriesCount < 6) {

            //get flag name and colors in flag from dictionary
            string flagName = flags[countriesCount].name;
            string colorsInFlag = (string)colorNames[flagName];
       
            //show the flag in question
            displayFlag(flagName);

            //go through each button and assign correct/wrong answers
            for(int i = 0; i < colorsButtons.Count; i++) {
                //get the colors of buttons
                string colorButton = colorsButtons[i].name;
                
                // if color is within flag then it will be correct
                if(colorsInFlag.Contains(colorButton)) {
                    colorsButtons[i].tag = "correct";
                } else {
                    colorsButtons[i].tag = "wrong";
                }
            }
            //update count
            countriesCount++;
        } else {
            //set assignEnd to true so popup can show
            assignEnd = true;
            finishPopUp();
        }
    }

    ////////////////////////////////////
    //
    // popup for various quiz scenes
    //
    ////////////////////////////////////

    void finishPopUp() {

        //if need to finish colors
        if(SceneManager.GetActiveScene().name == "6-NamesAndColors" && !assignEnd) {

            //reset vars and send to colors

            countriesCount = 0;
            namesColorsTitle.text = "Ordnet den Flaggen die Farben zu...";
            countriesToDeactivate.SetActive(false);
            colorsToActivate.SetActive(true);
            colorsAssignQuiz();
        } else {
            //popup and next button of scene display
            nextButton.SetActive(true);
            popup.SetActive(true);
        }
    }

    ////////////////////////////////////
    //
    // display the flag with proper dimensions, used in various scenes
    //
    ////////////////////////////////////

    void displayFlag(string country) {
        //flag
        flag.SetActive(true);

        //make the flag the correct size
        if (country == "Die Schweiz") {
            flag.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(300, 300);
        } else if (country == "Belgien") {
            flag.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(350, 300);
        } else {
            flag.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(450, 300);
        }

        //get the correct country
        for (int i = 0; i < flags.Count; i++) {
            if(flags[i].name == country) {
                flag.GetComponent<Image>().sprite = flags[i];
            }
        } 
    }
}

/////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////
// 
// Code by Bex Ruvalcaba
// @bexthehexx
// bexthehexx.graphics
//
/////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////

