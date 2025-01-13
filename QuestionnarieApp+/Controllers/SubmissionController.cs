using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using QuestionnarieApp_.Hubs;
using QuestionnarieApp_.Models;
using QuestionnarieApp_.Repositories.RepositoryInterfaces;
using QuestionnarieApp_.ViewModels;

[Authorize]
public class SubmissionController : Controller
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IQuestionnaireRepository _questionnaireRepository;
    private readonly IMapper _mapper;
    private readonly IHubContext<ParticipantHub> _hubContext;

    public SubmissionController(
        ISubmissionRepository submissionRepository,
        IQuestionnaireRepository questionnaireRepository,
        IMapper mapper,
        IHubContext<ParticipantHub> hubContext)
    {
        _submissionRepository = submissionRepository;
        _questionnaireRepository = questionnaireRepository;
        _mapper = mapper;
        _hubContext = hubContext;
    }

    [Authorize]
    public async Task<IActionResult> AddSubmission(int id)
    {
        var questionnaire = await _questionnaireRepository.GetByIdWithDetailsAsync(id);
        if (questionnaire == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<QuestionnaireViewModel>(questionnaire);
        return View(viewModel);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSubmission(int questionnaireId, List<int> selectedOptionIds)
    {
        if (selectedOptionIds == null || selectedOptionIds.Count == 0)
        {
            ModelState.AddModelError("", "Please select at least one option for each question.");
            var questionnaire = await _questionnaireRepository.GetByIdAsync(questionnaireId);
            if (questionnaire == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<QuestionnaireViewModel>(questionnaire);
            return View(viewModel);
        }

        var userId = User.Identity.Name;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var submission = new Submission
        {
            QuestionnaireId = questionnaireId,
            UserId = userId,
            SubmittedAt = DateTime.UtcNow,
            Answers = selectedOptionIds.Select(optionId => new Answer
            {
                OptionId = optionId,
                UserId = userId
            }).ToList()
        };

        await _submissionRepository.AddAsync(submission);
        await _submissionRepository.SaveChangesAsync();

        var updatedCount = await _submissionRepository.GetSubmissionCountByQuestionnaireIdAsync(questionnaireId);
        await _hubContext.Clients.All.SendAsync("UpdateParticipantCount", questionnaireId, updatedCount);

        return RedirectToAction("Index", "Questionnaire");
    }
}
