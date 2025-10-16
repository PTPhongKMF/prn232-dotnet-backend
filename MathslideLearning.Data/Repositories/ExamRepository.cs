using MathslideLearning.Data.DbContext;
using MathslideLearning.Data.Entities;
using MathslideLearning.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathslideLearning.Data.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly MathslideLearningDbContext _context;

        public ExamRepository(MathslideLearningDbContext context)
        {
            _context = context;
        }

        public async Task<Exam> CreateAsync(Exam exam)
        {
            await _context.Exams.AddAsync(exam);
            await _context.SaveChangesAsync();
            return exam;
        }

        public async Task<Exam> GetByIdAsync(int id)
        {
            return await _context.Exams
                .Include(e => e.Teacher)
                .Include(e => e.ExamQuestions)
                    .ThenInclude(eq => eq.Question)
                        .ThenInclude(q => q.Answers)
                .Include(e => e.ExamQuestions)
                    .ThenInclude(eq => eq.Question)
                        .ThenInclude(q => q.QuestionTags)
                            .ThenInclude(qt => qt.Tag)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Exam>> GetByTeacherIdAsync(int teacherId)
        {
            return await _context.Exams
                .Where(e => e.TeacherId == teacherId)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<Exam> UpdateAsync(Exam exam)
        {
            _context.Exams.Update(exam);
            await _context.SaveChangesAsync();
            return exam;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null) return false;

            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Exam>> GetAllAsync()
        {
            return await _context.Exams
                .Include(e => e.Teacher)
                .Include(e => e.ExamQuestions)
                    .ThenInclude(eq => eq.Question)
                .ToListAsync();
        }

        public async Task<bool> AddQuestionsToExamAsync(int examId, List<int> questionIds)
        {
            var exam = await _context.Exams
       .Include(e => e.ExamQuestions)
       .FirstOrDefaultAsync(e => e.Id == examId);

            if (exam == null) return false;

            foreach (var qId in questionIds)
            {
                if (!exam.ExamQuestions.Any(eq => eq.QuestionId == qId))
                {
                    exam.ExamQuestions.Add(new ExamQuestion { ExamId = examId, QuestionId = qId });
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveQuestionFromExamAsync(int examId, int questionId)
        {
            var examQuestion = await _context.Set<ExamQuestion>()
        .FirstOrDefaultAsync(eq => eq.ExamId == examId && eq.QuestionId == questionId);

            if (examQuestion == null) return false;

            _context.Set<ExamQuestion>().Remove(examQuestion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
