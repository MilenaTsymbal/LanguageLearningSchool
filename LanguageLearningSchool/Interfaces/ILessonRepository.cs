﻿using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Interfaces
{
    public interface ILessonRepository
    {
        List<Lesson> GetAll();
        Lesson GetById(int id);
        bool Add(Lesson course);
        bool Delete(Lesson course);
        bool DeleteRange(List<Lesson> lessons);
        bool Update(Lesson course);
        bool Save();
    }
}
