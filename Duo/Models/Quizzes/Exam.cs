using Duo.Models.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duo.Models.Quizzes;
public class Exam : BaseQuiz<Exercise>
{
    private const int MAX_EXERCISES = 25;
    private const double PASSING_THRESHOLD = 90;

    public Exam(int id) : base(id, MAX_EXERCISES, PASSING_THRESHOLD) { }
}
