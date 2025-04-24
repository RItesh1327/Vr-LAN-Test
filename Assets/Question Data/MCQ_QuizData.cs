using System.Collections.Generic;
using UnityEngine;

namespace MCQQuiz
{
    [CreateAssetMenu(fileName = "NewQuizData", menuName = "MCQ Quiz/Quiz Data")]
    public class MCQ_QuizData : ScriptableObject
    {
        public List<MCQ_Question> questions;
    }
}
