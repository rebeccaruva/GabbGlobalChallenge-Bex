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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanges : MonoBehaviour
{
    public GameController quiz;

    //button to welcome scene
    public void goToWelcomeScene() {
        SceneManager.LoadScene("1-Willkomen");
    }

    //button to first globe scene
    public void goToGlobeScene() {
        SceneManager.LoadScene("2-Globe");
    }

    //button to europe info scene
    public void goToEuropeInfoScene() {
        SceneManager.LoadScene("3-Europe");
    }

    //button for first flag names quiz
    public void answerWrongorRight(Button button) {
        if(button.gameObject.tag == "correct") {
            //continue through quiz
            quiz = this.gameObject.GetComponent<GameController>();
            quiz.flagNamesQuiz();
        } else {
            //disable the incorrect answer
            button.interactable = false;
        }
    }

    //button to second globe scene
    public void goToGlobeSceneAgain() {
        SceneManager.LoadScene("5-Globe");
    }

    //button to names and colors assignment quiz scene
    public void goToArrangeScene() {
        SceneManager.LoadScene("6-NamesAndColors");
    }

    //button for assignment flag names quiz
    public void answerCountryNames(Button button) {
        if(button.gameObject.tag == "correct") {
            //continue through assigning countries
            quiz = this.gameObject.GetComponent<GameController>();
            quiz.countriesAssignQuiz();
        } else {
            //disable the incorrect answer
            button.interactable = false;
        }
    }

    //button for assignment flag colors quiz
    public void answerColors(Button button) {
        if(button.gameObject.tag == "correct") {
            //continue through assigning countries
            quiz = this.gameObject.GetComponent<GameController>();
            quiz.colorsAssignQuiz();
        } else {
            //disable the incorrect answer
            button.interactable = false;
        }
    }

    //button to final namibia information scene
    public void goToNamibiaScene() {
        SceneManager.LoadScene("7-Namibia");
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
