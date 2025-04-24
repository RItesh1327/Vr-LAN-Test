using Fusion;
using MCQQuiz;
using TMPro;
using UnityEngine;

public class QuizHandler : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] GameObject[] answers;
    string question;
    string[] answersText;

    public string Question
    {
        get => question;
        set => question = value;
    }

    public string[] AnswersText
    {
        get => answersText;
        set => answersText = value;
    }
    [SerializeField] MCQ_QuizData questionData;
    int currentQuestionindex = 0;
    public int CurrentQuestionIndex
    {
        get => currentQuestionindex;
        set => currentQuestionindex = value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    NetworkObject networkObject = GetComponent<NetworkObject>();
    //    if (networkObject.HasInputAuthority)
    //    {
    //        DispatchQuestion();
    //    }
    //    else
    //    {
    //        Debug.LogError("We dont have state authority here");
    //    }
    //}

    public override void Spawned()
    {
        base.Spawned();
        if (HasStateAuthority)
        {
            DispatchQuestion();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ValidateAnswer(int index)
    {
        bool isCorrect = index == questionData.questions[currentQuestionindex].answerIndex;
        if (index == questionData.questions[currentQuestionindex].answerIndex)
        {
            Debug.Log("Correct Answer");
            isCorrect = true;
        }
        else
        {
            Debug.Log("Wrong Answer");
            isCorrect = false;
        }
        if (HasStateAuthority)
        {
            RPC_ValidateAnswer(index, isCorrect);
            if (isCorrect)
                NextQuestion();
        }
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ValidateAnswer(int index, bool isCorrect)
    {
        // Do your stuff here. 
    }

    public void NextQuestion()
    {
        currentQuestionindex++;
        DispatchQuestion();
    }
    public void DispatchQuestion()
    {
        
        currentQuestionindex = Mathf.Clamp(currentQuestionindex, 0, questionData.questions.Count - 1);
        string q = questionData.questions[currentQuestionindex].question;
        string[] answers = questionData.questions[currentQuestionindex].options;
        if(HasStateAuthority)
        {
            RPC_SetQuesion(q, answers, currentQuestionindex);
        }
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetQuesion(string question, string[] answersText, int currentQuestionIndex)
    {
        Question = question;
        AnswersText = answersText;
        CurrentQuestionIndex = currentQuestionIndex;
        questionText.text = Question;
        for (int i = 0; i < answers.Length; i++)
        {
            answers[i].SetActive(i < AnswersText.Length);
            if (i < AnswersText.Length)
            {
                answers[i].GetComponentInChildren<TextMeshProUGUI>().text = AnswersText[i];
            }
        }
    }
}
