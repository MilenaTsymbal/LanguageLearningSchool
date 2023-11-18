﻿using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Interfaces
{
    public interface IAnswerRepository
    {
        List<Answer> GetAll(int taskId);
        List<Answer> GetAllAnswersOfLesson(int lessonId);
        Answer GetById(int id);
        bool Add(Answer course);
        bool Delete(Answer course);
        bool Update(Answer course);
        bool Save();
    }
}
