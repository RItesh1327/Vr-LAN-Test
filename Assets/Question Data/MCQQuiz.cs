using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MCQQuiz
{
    [System.Serializable]
    public class MCQ_Question
    {
        public int questionNumber; // Add question index
        public string question;
        public string[] options = new string[3];
        public int answerIndex;
        //public int scoreValue;
    }
}



