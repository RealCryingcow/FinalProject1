using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestionnarieApp_.Models;
using QuestionnarieApp_.Repositories;
using QuestionnarieApp_.Repositories.RepositoryInterfaces;
using QuestionnarieApp_.ViewModels;

namespace QuestionnarieApp_.Controllers
{
    public class QuestionnaireController : Controller
    {
        private readonly IQuestionnaireRepository _questionnaireRepository;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IMapper _mapper;

        public QuestionnaireController(IQuestionnaireRepository questionnaireRepository, IMapper mapper, ISubmissionRepository submissionRepository)
        {
            _questionnaireRepository = questionnaireRepository;
            _mapper = mapper;
            _submissionRepository = submissionRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var questionnaires = await _questionnaireRepository.GetAllAsync();
            var submissionCounts = await _submissionRepository.GetQuestionnaireSubmissionCountsAsync();

            var viewModel = _mapper.Map<List<QuestionnaireViewModel>>(questionnaires);

            foreach (var questionnaire in viewModel)
            {
                if (submissionCounts.TryGetValue(questionnaire.Id, out var count))
                {
                    questionnaire.ParticipantCount = count;
                }
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new QuestionnaireViewModel());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionnaireViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var questionnaire = _mapper.Map<Questionnaire>(model);
            questionnaire.CreatedAt = DateTime.Now;

            await _questionnaireRepository.AddAsync(questionnaire);
            await _questionnaireRepository.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var questionnaire = await _questionnaireRepository.GetByIdAsync(id);
            if (questionnaire == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<QuestionnaireViewModel>(questionnaire);
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(QuestionnaireViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var questionnaire = await _questionnaireRepository.GetByIdAsync(model.Id);
            if (questionnaire == null)
            {
                return NotFound();
            }

            _mapper.Map(model, questionnaire);

            await _questionnaireRepository.UpdateAsync(questionnaire);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var questionnaire = await _questionnaireRepository.GetByIdAsync(id);
            if (questionnaire == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<QuestionnaireViewModel>(questionnaire);
            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _questionnaireRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var questionnaire = await _questionnaireRepository.GetDetailsWithParticipantCountsAsync(id);

            if (questionnaire == null)
            {
                return NotFound();
            }

            return View(questionnaire);
        }

    }
}
