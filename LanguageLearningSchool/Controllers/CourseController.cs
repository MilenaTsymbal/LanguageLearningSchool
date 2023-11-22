using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using LanguageLearningSchool.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using QuestPDF.Elements;
using System.Data;
using System.Drawing;
using System.Security.Claims;
using QuestPDF;
using LanguageLearningSchool.Repositories;

namespace LanguageLearningSchool.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        public readonly IUserRepository _userRepository;
        private readonly IUserAndCourseRepository _userAndCourseRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILessonRepository _lessonRepository;
        private readonly IUserAndLessonRepository _userAndLessonRepository;
        private readonly ILessonTaskRepository _lessonTaskRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IUserAndTaskRepository _userAndTaskRepository;
        public CourseController(ICourseRepository courseRepository, IUserRepository userRepository,
            IUserAndCourseRepository userAndCourseRepository, UserManager<User> userManager,
            ILessonRepository lessonRepository, IUserAndLessonRepository userAndLessonRepository,
            ILessonTaskRepository lessonTaskRepository, IAnswerRepository answerRepository,
            IUserAndTaskRepository userAndTaskRepository)

        {
            _courseRepository = courseRepository;
            _userRepository = userRepository;
            _userAndCourseRepository = userAndCourseRepository;
            _userManager = userManager;
            _lessonRepository = lessonRepository;
            _userAndLessonRepository = userAndLessonRepository;
            _lessonTaskRepository = lessonTaskRepository;
            _answerRepository = answerRepository;
            _userAndTaskRepository = userAndTaskRepository;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = _userRepository.GetById(userId);

            if (user != null)
            {
                List<Course> courses = _courseRepository.GetAll();
                var userAndCourses = _userAndCourseRepository.GetAll().FindAll(item => item.UserId == user.Id).ToList();
                var usersCreatorOfCourses = _userAndCourseRepository.GetAll().Where(uc => uc.DateOfCreation != null).ToList();

                var coursesInfo = new CourseIndexViewModel
                {
                    Courses = courses,
                    UserAndCourses = userAndCourses,
                    UserCreatorsOfCourses = usersCreatorOfCourses
                };
                return View(coursesInfo);
            }

            return View("Error");
        }

        public IActionResult FilteredIndex(CourseIndexViewModel filters)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userRepository.GetById(userId);

            if (user != null)
            {
                List<Course> courses = _courseRepository.GetAll();
                var userAndCourses = _userAndCourseRepository.GetAll().FindAll(item => item.UserId == user.Id).ToList();
                var usersCreatorOfCourses = _userAndCourseRepository.GetAll().Where(uc => uc.DateOfCreation != null).ToList();

                if (filters.LanguageFilter != null)
                {
                    courses = courses.Where(c => c.Language == filters.LanguageFilter).ToList();
                }

                if (filters.DifficultyLevelFilter != null)
                {
                    courses = courses.Where(c => c.DifficultyLevel == filters.DifficultyLevelFilter).ToList();
                }

                if (!string.IsNullOrEmpty(filters.AlphabeticalOrder))
                {
                    if (filters.AlphabeticalOrder.ToLower() == "asc")
                    {
                        courses = courses.OrderBy(c => c.CourseName).ToList();
                    }
                    else if (filters.AlphabeticalOrder.ToLower() == "desc")
                    {
                        courses = courses.OrderByDescending(c => c.CourseName).ToList();
                    }

                }

                if (!string.IsNullOrEmpty(filters.DateOfCreationOrder))
                {
                    var sortedUsersCreatorOfCourses = _userAndCourseRepository
                            .GetAll()
                            .Where(uc => uc.DateOfCreation != null)
                            .ToList();
                    if (filters.DateOfCreationOrder.ToLower() == "newest")
                    {
                        sortedUsersCreatorOfCourses = sortedUsersCreatorOfCourses.OrderBy(uc => uc.DateOfCreation)
                           .ToList();
                    }
                    else if (filters.DateOfCreationOrder.ToLower() == "oldest")
                    {
                        sortedUsersCreatorOfCourses = sortedUsersCreatorOfCourses.OrderByDescending(uc => uc.DateOfCreation)
                           .ToList();
                    }

                    var sortedCourses = new List<Course>();
                    foreach (var userCourse in sortedUsersCreatorOfCourses)
                    {
                        var course = _courseRepository.GetById(userCourse.CourseId);
                        if (course != null)
                        {
                            sortedCourses.Add(course);
                        }
                    }
                    courses = sortedCourses;
                }

                if (!string.IsNullOrEmpty(filters.SearchQuery))
                {
                    courses = courses.Where(c =>
                        c.CourseName.Contains(filters.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                        c.Description.Contains(filters.SearchQuery, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                filters.Courses = courses;
                filters.UserAndCourses = userAndCourses;
                filters.UserCreatorsOfCourses = usersCreatorOfCourses;
                filters.LanguageFilter = filters.LanguageFilter;
                filters.DifficultyLevelFilter = filters.DifficultyLevelFilter;
                filters.AlphabeticalOrder = filters.AlphabeticalOrder;
                filters.DateOfCreationOrder = filters.DateOfCreationOrder;
                filters.SearchQuery = filters.SearchQuery;

                return View("Index", filters);
            }

            return View("Error");
        }

        public IActionResult Detail(int id)
        {
            Course course = _courseRepository.GetById(id);
            var usersAndCourse = _userAndCourseRepository.GetAll().FindAll(item => item.CourseId == id).ToList();
            var lessons = _lessonRepository.GetAll().FindAll(item => item.CourseId == id).ToList();
            var userAndLessons = _userAndLessonRepository.GetUserAndLessonsForCourse(id);

            var model = new CourseDetailsViewModel
            {
                Course = course,
                UsersAndCourse = usersAndCourse,
                Lessons = lessons,
                UserAndLessons = userAndLessons
            };
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Course course)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }
            _courseRepository.Add(course);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var dateOfCreation = DateTime.Now;

            UserAndCourse userAndCourse = new UserAndCourse
            {
                CourseId = course.CourseId,
                UserId = userId,
                DateOfCreation = dateOfCreation
            };

            _userAndCourseRepository.Add(userAndCourse);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var course = _courseRepository.GetById(id);
            if (course == null) return View("Error");

            var courseViewModel = new EditCourseViewModel
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Description = course.Description,
                Language = course.Language,
                DifficultyLevel = course.DifficultyLevel
            };
            return View(courseViewModel);
        }

        [HttpPost]
        public IActionResult Edit(int courseId, EditCourseViewModel courseViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit course");
                return View("Edit", courseViewModel);
            }

            var course = _courseRepository.GetById(courseId);

            if (course != null)
            {
                course.CourseName = courseViewModel.CourseName;
                course.Description = courseViewModel.Description;
                course.Language = courseViewModel.Language;
                course.DifficultyLevel = courseViewModel.DifficultyLevel;

                _courseRepository.Update(course);

                return RedirectToAction("Detail", new { id = courseId });
            }
            else
            {
                return View(courseViewModel);
            }

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var course = _courseRepository.GetById(id);
            if (course == null) return View("Error");
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteCourse(int id)
        {
            var course = _courseRepository.GetById(id);
            if (course == null) return View("Error");

            var usersAndCourse = _userAndCourseRepository.GetAll().Where(uc => uc.CourseId == id).ToList();
            var lessonsForCourse = _lessonRepository.GetAll().Where(lesson => lesson.CourseId == id).ToList();
            var userAndLessonsForCourse = _userAndLessonRepository
                .GetAll()
                .Where(userAndLesson => lessonsForCourse.Select(lesson => lesson.LessonId).Contains(userAndLesson.LessonId))
                .ToList();
            var tasksForCourse = _lessonTaskRepository
                .GetAll()
                .Where(task => lessonsForCourse
                .Select(lesson => lesson.LessonId).Contains(task.LessonId))
                .ToList();
            var userAndTasksForCourse = _userAndTaskRepository
                .GetAll()
                .Where(userAndTask => tasksForCourse.Select(task => task.TaskId).Contains(userAndTask.TaskId))
                .ToList();
            var allAnswersForCourse = tasksForCourse
                .SelectMany(task => _answerRepository.GetAllAnswersOfTask(task.TaskId))
                .ToList();

            if (allAnswersForCourse != null && allAnswersForCourse.Any())
            {
                _answerRepository.DeleteRange(allAnswersForCourse);
            }
            if (userAndTasksForCourse != null && userAndTasksForCourse.Any())
            {
                _userAndTaskRepository.DeleteRange(userAndTasksForCourse);
            }
            if (tasksForCourse != null && tasksForCourse.Any())
            {
                _lessonTaskRepository.DeleteRange(tasksForCourse);
            }
            if (userAndLessonsForCourse != null && userAndLessonsForCourse.Any())
            {
                _userAndLessonRepository.DeleteRange(userAndLessonsForCourse);
            }
            if (lessonsForCourse != null && lessonsForCourse.Any())
            {
                _lessonRepository.DeleteRange(lessonsForCourse);
            }
            if (usersAndCourse != null && usersAndCourse.Any())
            {
                _userAndCourseRepository.DeleteRange(usersAndCourse);
            }
            _courseRepository.Delete(course);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddUserToCourse(int courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            DateTime startDate = DateTime.Now;

            UserAndCourse userAndCourse = new UserAndCourse
            {
                CourseId = courseId,
                UserId = userId,
                StartDate = startDate
            };

            _userAndCourseRepository.Add(userAndCourse);

            return RedirectToAction("Detail", new { id = courseId });
        }

        [HttpPost]
        public IActionResult RemoveUserFromCourse(int courseId, string userId)
        {
            var lessonsForCourse = _lessonRepository.GetAll().Where(lesson => lesson.CourseId == courseId).ToList();
            var tasksForCourse = _lessonTaskRepository
                .GetAll()
                .Where(task => lessonsForCourse
                .Select(lesson => lesson.LessonId).Contains(task.LessonId))
                .ToList();

            var userAndTasksForCourse = _userAndTaskRepository
                .GetAll()
                .Where(userAndTask => tasksForCourse.Select(task => task.TaskId).Contains(userAndTask.TaskId)
                && userAndTask.UserId == userId)
                .ToList();
            _userAndTaskRepository.DeleteRange(userAndTasksForCourse);

            var userAndLessonsForCourse = _userAndLessonRepository
                .GetAll()
                .Where(userAndLesson => lessonsForCourse.Select(lesson => lesson.LessonId).Contains(userAndLesson.LessonId)
                && userAndLesson.UserId == userId)
                .ToList();
            _userAndLessonRepository.DeleteRange(userAndLessonsForCourse);

            var userAndCourse = _userAndCourseRepository.GetAll().Find(item => item.UserId == userId && item.CourseId == courseId);
            _userAndCourseRepository.Delete(userAndCourse);

            return RedirectToAction("Detail", new { id = courseId });
        }

        
        //[HttpPost]
        //public IActionResult GenerateCoursesDataReport(int courseId)
        //{
        //    List<Course> courses = _courseRepository.GetAll();

        //    Document.Create(container =>
        //    {
        //        container.Page(page =>
        //        {
        //            page.Size(PageSizes.A4);
        //            page.Header().Text($"Course report");

        //            foreach (var course in courses)
        //            {
        //                var lessonsForCourse = _lessonRepository.GetAll().Where(lesson => lesson.CourseId == course.CourseId).ToList();
        //                var lessonsForCourseCount = lessonsForCourse.Count;
        //                var c = _lessonTaskRepository
        //                    .GetAll()
        //                    .Where(task => lessonsForCourse
        //                        .Select(lesson => lesson.LessonId).Contains(task.LessonId))
        //                    .ToList().Count;
        //                var usersAndCourse = _userAndCourseRepository.GetAll().Where(item => item.CourseId == course.CourseId).ToList().Count;

        //                page.Content()
        //                .Text($"Course name: \n{course.CourseName}\n " +
        //                $"Description of course: \n{course.Description}\n" +
        //                $"Number of lessons on course:\n {lessonsForCourse}\n" +
        //                $"Number of task on course: \n{lessonsForCourse}\n" +
        //                $"Number of registered users on course: {usersAndCourse}\n");
        //            }
        //        });
        //    }).ShowInPreviewer();

            
        //    return View("Index");
        //}

        //[HttpPost]
        //public IActionResult GenerateCourseReport(int courseId)
        //{
        //    Course course = _courseRepository.GetById(courseId);

        //    var lessonsForCourse = _lessonRepository.GetAll().Where(lesson => lesson.CourseId == courseId).ToList();
        //    var tasksForCourse = _lessonTaskRepository
        //        .GetAll()
        //        .Where(task => lessonsForCourse
        //            .Select(lesson => lesson.LessonId).Contains(task.LessonId))
        //        .ToList();

        //    var userAndTasksForCourse = _userAndTaskRepository
        //        .GetAll()
        //        .Where(userAndTask => tasksForCourse.Select(task => task.TaskId).Contains(userAndTask.TaskId))
        //        .ToList();

        //    var userAndLessonsForCourse = _userAndLessonRepository
        //        .GetAll()
        //        .Where(userAndLesson => lessonsForCourse.Select(lesson => lesson.LessonId).Contains(userAndLesson.LessonId))
        //        .ToList();

        //    var usersAndCourse = _userAndCourseRepository.GetAll().Where(item => item.CourseId == courseId).ToList();

        //    Document.Create(container =>
        //    {
        //        container.Page(page =>
        //        {
        //            page.Size(PageSizes.A4);
        //            page.Header().Text($"{course.CourseName} course report");

        //            foreach (var userAndCourse in usersAndCourse)
        //            {
        //                var user = _userRepository.GetById(userAndCourse.UserId);
        //                var generalestimation = userAndCourse.GeneralEstimation;
        //                var userAndLessons = userAndLessonsForCourse.Where(ul => ul.UserId == userAndCourse.UserId).ToList();
        //                var completedLessonsCount = userAndLessons.Count();
        //                var totalLessonsCount = lessonsForCourse.Count();

        //                var userAverageGrade = 0;
        //                if (userAndTasksForCourse.Count() != 0)
        //                {
        //                    var totalScore = userAndTasksForCourse.Sum(ut => ut.UserAnswer == false ? 0 : 1);
        //                    userAverageGrade = totalScore / userAndTasksForCourse.Count();
        //                }

        //                var progress = completedLessonsCount / totalLessonsCount;
        //                if (user.UserRole == Enums.Roles.Student && userAndCourse.EndDate != null)
        //                {
        //                    page.Content()
        //                    .Text($"{user.Name} {user.MiddleName} {user.Surname}\n" +
        //                    $"General course estimation: {generalestimation}\n" +
        //                    $"Lessons completed: {completedLessonsCount} out of {totalLessonsCount}\n" +
        //                    $"Average lesson grade: {userAverageGrade}\n" +
        //                    $"Progress: {(progress * 100).ToString("F2")}%");
        //                }
        //            }
        //        });
        //    }).GeneratePdf();

        //    return RedirectToAction("Index");
        //}
        
        public IActionResult GenerateCourseReport(int courseId)
        {
            Course course = _courseRepository.GetById(courseId);

            var lessonsForCourse = _lessonRepository.GetAll().Where(lesson => lesson.CourseId == courseId).ToList();
            var tasksForCourse = _lessonTaskRepository
                .GetAll()
                .Where(task => lessonsForCourse
                    .Select(lesson => lesson.LessonId).Contains(task.LessonId))
                .ToList();

            var userAndTasksForCourse = _userAndTaskRepository
                .GetAll()
                .Where(userAndTask => tasksForCourse.Select(task => task.TaskId).Contains(userAndTask.TaskId))
                .ToList();

            var userAndLessonsForCourse = _userAndLessonRepository
                .GetAll()
                .Where(userAndLesson => lessonsForCourse.Select(lesson => lesson.LessonId).Contains(userAndLesson.LessonId))
                .ToList();

            var usersAndCourse = _userAndCourseRepository.GetAll().Where(item => item.CourseId == courseId).ToList();


            var doc = Document.Create(container => container.Page(page =>
            {

                page.Margin(50);
                page.Size(PageSizes.A4);

                page.Content().Padding(50).Column(column =>
                {
                    page.Size(PageSizes.A4);
                    page.Header().Text($"{course.CourseName} course report");

                    foreach (var userAndCourse in usersAndCourse)
                    {
                        var user = _userRepository.GetById(userAndCourse.UserId);
                        var generalestimation = userAndCourse.GeneralEstimation;
                        var userAndLessons = userAndLessonsForCourse.Where(ul => ul.UserId == userAndCourse.UserId).ToList();
                        var completedLessonsCount = userAndLessons.Count();
                        var totalLessonsCount = lessonsForCourse.Count();

                        var userAverageGrade = 0;
                        if (userAndTasksForCourse.Count() != 0)
                        {
                            var totalScore = userAndTasksForCourse.Sum(ut => ut.UserAnswer == false ? 0 : 1);
                            userAverageGrade = totalScore / userAndTasksForCourse.Count();
                        }

                        var progress = completedLessonsCount / totalLessonsCount;
                        if (user.UserRole == Enums.Roles.Student && userAndCourse.EndDate != null)
                        {

                            column.Item().Text($"{user.Name} {user.MiddleName} {user.Surname}\n" +
                            $"General course estimation: {generalestimation}\n" +
                            $"Lessons completed: {completedLessonsCount} out of {totalLessonsCount}\n"
                            //$"Average lesson grade: {userAverageGrade}\n" +
                            //$"Progress: {(progress * 100).ToString("F2")}%"
                            );
                        }
                    }
                });
            }));

            MemoryStream stream = new MemoryStream();
            doc.GeneratePdf(stream);

            stream.Position = 0;

            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");
            fileStreamResult.FileDownloadName = "courseReport.pdf";
            return fileStreamResult;
        }

        public IActionResult GenerateCoursesDataReport()
        {
            List<Course> courses = _courseRepository.GetAll();

            var doc = Document.Create(container => container.Page(page =>
            {

                page.Margin(50);
                page.Size(PageSizes.A4);

                page.Content().Padding(50).Column(column =>
                {
                    page.Size(PageSizes.A4);
                    page.Header().Text($"Courses report");

                    foreach (var course in courses)
                    {
                        var lessonsForCourse = _lessonRepository.GetAll().Where(lesson => lesson.CourseId == course.CourseId).ToList();
                        var lessonsForCourseCount = lessonsForCourse.Count;
                        var c = _lessonTaskRepository
                            .GetAll()
                            .Where(task => lessonsForCourse
                                .Select(lesson => lesson.LessonId).Contains(task.LessonId))
                            .ToList().Count;
                        var usersAndCourse = _userAndCourseRepository.GetAll().Where(item => item.CourseId == course.CourseId).ToList().Count;

                        column.Item().Text($"Course name: \n{course.CourseName}\n " +
                        $"Description of course: \n{course.Description}\n" +
                        $"Number of lessons on course:\n {lessonsForCourseCount}\n" +
                        $"Number of task on course: \n{c}\n" +
                        $"Number of registered users on course: {usersAndCourse}\n");
                    }
                });
            }));

            MemoryStream stream = new MemoryStream();
            doc.GeneratePdf(stream);

            stream.Position = 0;

            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");
            fileStreamResult.FileDownloadName = "coursesReport.pdf";
            return fileStreamResult;
        }
    }
}
