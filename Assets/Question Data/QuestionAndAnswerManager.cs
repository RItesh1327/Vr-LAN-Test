using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace MCQQuiz
{
    public class QuestionAndAnswerManager : MonoBehaviour
    {
        public static QuestionAndAnswerManager instance;
        public AudioSource aSource;
        public AudioClip rightAnsClip;
        public AudioClip wrongAnsClip;

        public GameObject questionPanel;
        public GameObject resultPanel;
        public GameObject warningPanel;

        private float score = 0f;
        private int QuestionIndex = 0;
        private int attemptsLeft;
        private bool isAnswered;

        public int initialAttempts;

        public Button nextButton;
        public TextMeshProUGUI score_txt;
        public TextMeshProUGUI questionNumber_txt;
        public TextMeshProUGUI attempts_txt;

        public MCQ_QuizData QuestionData;
        public TextMeshProUGUI questionTxt;
        public Button[] options;

        MCQ_Question currentQuestion;

        private void Start()
        {
            attemptsLeft = initialAttempts;

            if (QuestionData != null && QuestionData.questions.Count > 0)
            {
                DisplayQuestion();
            }
            UpdateUI();
        }

        private IEnumerator ShowWarningPanelCoroutine()
        {
            Debug.Log("Coroutine started: Warning panel should show now.");
            warningPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            warningPanel.SetActive(false);
        }

        void DisplayQuestion()
        {
            if (QuestionIndex < QuestionData.questions.Count)
            {
                currentQuestion = QuestionData.questions[QuestionIndex];
                questionTxt.text = currentQuestion.question;
                attemptsLeft = initialAttempts;
                isAnswered = false;

                for (int i = 0; i < options.Length; i++)
                {
                    int capturedIndex = i; // Fix for button click issue
                    options[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.options[i];

                    options[i].image.color = Color.white;
                    options[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;

                    options[i].onClick.RemoveAllListeners();
                    options[i].onClick.AddListener(() => CheckAnswer(capturedIndex));
                }
                UpdateUI();
            }
            else
            {
                ShowResult();
            }
        }

        void CheckAnswer(int selectedIndex)
        {
            if (isAnswered || attemptsLeft <= 0)
                return;

            Button selectedButton = options[selectedIndex];
            TextMeshProUGUI selectedText = selectedButton.GetComponentInChildren<TextMeshProUGUI>();
            Color originalButtonColor = selectedButton.image.color;
            Color originalTextColor = selectedText.color;

            if (selectedIndex == currentQuestion.answerIndex)
            {
                float questionScore = 0f;

                switch (attemptsLeft)
                {
                    case 3:
                        questionScore = 2f;
                        break;
                    case 2:
                        questionScore = 1f;
                        break;
                    case 1:
                        questionScore = 0.5f;
                        break;
                    default:
                        questionScore = 0f;
                        break;
                }

                score += questionScore;
                isAnswered = true;

                aSource.PlayOneShot(rightAnsClip);
                StartCoroutine(FlashButton(selectedButton, selectedText, Color.green, originalButtonColor, originalTextColor, true));
            }
            else
            {
                attemptsLeft--;

                aSource.PlayOneShot(wrongAnsClip);
                StartCoroutine(FlashButton(selectedButton, selectedText, Color.red, originalButtonColor, originalTextColor, false));

                if (attemptsLeft <= 0)
                {
                    isAnswered = true; // lock question after last attempt
                    StartCoroutine(FlashButton(selectedButton, selectedText, Color.red, originalButtonColor, originalTextColor, true));
                }
            }

            UpdateUI();
        }




        public void OnNextButtonClick()
        {
            if (!isAnswered)
            {
                ShowWarningPanel();
                return;
            }

            NextQuestion();
        }

        void NextQuestion()
        {
            QuestionIndex++;
            DisplayQuestion();
        }

        void ShowWarningPanel()
        {
            Debug.Log("ShowWarningPanel() called");
            StartCoroutine(ShowWarningPanelCoroutine());
        }

        void ShowResult()
        {
            questionPanel.SetActive(false);
            resultPanel.SetActive(true);
        }

        IEnumerator FlashButton(Button button, TextMeshProUGUI text, Color flashColor, Color originalButtonColor, Color originalTextColor, bool shouldProceed)
        {
            button.image.color = flashColor;
            text.color = Color.black;
            yield return new WaitForSeconds(1.5f);
            button.image.color = originalButtonColor;
            text.color = originalTextColor;

            if (shouldProceed)
            {
                NextQuestion();
            }
        }


        public void RestartQuiz()
        {
            score = 0f;
            QuestionIndex = 0;
            attemptsLeft = initialAttempts;
            isAnswered = false;

            resultPanel.SetActive(false);
            questionPanel.SetActive(true);

            DisplayQuestion(); // Show the first question
            UpdateUI();        // Refresh score, attempts, etc.
        }


        void UpdateUI()
        {
            score_txt.text = "Your Score: " + score.ToString("0.0");
            attempts_txt.text = "Attempts Left: " + attemptsLeft;
            questionNumber_txt.text = "Question: " + Mathf.Min(QuestionIndex + 1, QuestionData.questions.Count) + "/" + QuestionData.questions.Count;
        }
    }
}
