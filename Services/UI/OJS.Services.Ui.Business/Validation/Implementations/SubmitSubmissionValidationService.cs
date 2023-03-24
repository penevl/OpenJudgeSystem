﻿namespace OJS.Services.Ui.Business.Validation.Implementations;

using Infrastructure.Exceptions;
using OJS.Data.Models.Contests;
using OJS.Data.Models.Participants;
using OJS.Data.Models.Problems;
using OJS.Services.Common.Models;
using OJS.Services.Common.Models.Users;
using Models.Submissions;
using System.Linq;

public class SubmitSubmissionValidationService : ISubmitSubmissionValidationService
{
    public ValidationResult GetValidationResult(
        (Problem?, UserInfoModel, Participant?, ValidationResult, int, bool, SubmitSubmissionServiceModel)
            validationInput)
    {
        var (problem, user, participant, contestValidationResult,
                userSubmissionTimeLimit, hasUserNotProcessedSubmissionForProblem, submitSubmissionServiceModel) =
            validationInput;

        if (problem == null)
        {
            return ValidationResult.Invalid(ValidationMessages.Problem.NotFound);
        }

        if (string.IsNullOrWhiteSpace(submitSubmissionServiceModel.StringContent) &&
            (submitSubmissionServiceModel.ByteContent == null || submitSubmissionServiceModel.ByteContent.Length == 0))
        {
            throw new BusinessServiceException(string.Format(ValidationMessages.Submission.SubmissionEmpty, problem.Id));
        }

        if (participant == null && !user.IsAdminOrLecturer && submitSubmissionServiceModel.Official)
        {
            return ValidationResult.Invalid(string.Format(ValidationMessages.Submission.NotRegisteredForExam, problem.Id));
        }

        if (!contestValidationResult.IsValid)
        {
            return contestValidationResult;
        }

        if (submitSubmissionServiceModel.Official &&
            participant!.Contest.IsOnlineExam &&
            !IsUserAdminOrLecturerInContest(participant.Contest, user) &&
            participant.ProblemsForParticipants.All(p => p.ProblemId != problem.Id))
        {
            return ValidationResult.Invalid(string.Format(ValidationMessages.Submission.ProblemNotAssignedToUser, problem.Id));
        }

        var submissionType =
            problem.SubmissionTypesInProblems.FirstOrDefault(st =>
                st.SubmissionTypeId == submitSubmissionServiceModel.SubmissionTypeId);

        if (submissionType == null)
        {
            return ValidationResult.Invalid(string.Format(ValidationMessages.Submission.SubmissionTypeNotFound, problem.Id));
        }

        var isFileUpload = submitSubmissionServiceModel.StringContent == null ||
                           submitSubmissionServiceModel.ByteContent != null;

        if (isFileUpload && !submissionType.SubmissionType.AllowedFileExtensions!.Contains(
                submitSubmissionServiceModel.FileExtension!))
        {
            return ValidationResult.Invalid(string.Format(ValidationMessages.Submission.InvalidExtension, problem.Id));
        }

        if (isFileUpload && !submissionType.SubmissionType.AllowBinaryFilesUpload)
        {
            return ValidationResult.Invalid(string.Format(ValidationMessages.Submission.BinaryFilesNotAllowed, problem.Id));
        }

        if (!isFileUpload && submissionType.SubmissionType.AllowBinaryFilesUpload)
        {
            return ValidationResult.Invalid(string.Format(ValidationMessages.Submission.TextUploadNotAllowed, problem.Id));
        }

        if (hasUserNotProcessedSubmissionForProblem)
        {
            return ValidationResult.Invalid(string.Format(ValidationMessages.Submission.UserHasNotProcessedSubmissionForProblem, problem.Id));
        }

        if (submitSubmissionServiceModel.StringContent != null &&
            problem.SourceCodeSizeLimit < submitSubmissionServiceModel.StringContent.Length)
        {
            return ValidationResult.Invalid(string.Format(ValidationMessages.Submission.SubmissionTooLong, problem.Id));
        }

        if (!isFileUpload && submitSubmissionServiceModel.StringContent!.Length < 5)
        {
            return ValidationResult.Invalid(string.Format(ValidationMessages.Submission.SubmissionTooShort, problem.Id));
        }

        if (userSubmissionTimeLimit != 0)
        {
            return ValidationResult.Invalid(string.Format(ValidationMessages.Submission.SubmissionWasSentTooSoon, problem.Id));
        }

        return ValidationResult.Valid();
    }

    private static bool IsUserAdminOrLecturerInContest(Contest contest, UserInfoModel currentUser)
        => currentUser.IsAdmin ||
           contest.LecturersInContests.Any(c => c.LecturerId == currentUser.Id) ||
           contest.Category!.LecturersInContestCategories.Any(cl => cl.LecturerId == currentUser.Id);
}