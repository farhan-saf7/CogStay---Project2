using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CogStayMVC.DTOs;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Services.Manager;

/// <summary>
/// Service implementation managing customer feedbacks and ratings.
/// Enables guests to submit ratings/comments and hotel managers to moderate reviews.
/// </summary>
public class FeedbackService : IFeedbackService
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IGuestRepository _guestRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="FeedbackService"/> class.
    /// </summary>
    /// <param name="feedbackRepository">Repository handling Feedback models.</param>
    /// <param name="guestRepository">Repository managing Guest models.</param>
    public FeedbackService(
        IFeedbackRepository feedbackRepository,
        IGuestRepository guestRepository)
    {
        _feedbackRepository = feedbackRepository;
        _guestRepository = guestRepository;
    }

    /// <summary>
    /// Retrieves all guest feedback comments, populated with related Guest and Room details.
    /// </summary>
    /// <returns>Collection of feedback response DTOs.</returns>
    public async Task<IEnumerable<FeedbackResponseDTO>> GetAllFeedbacksAsync()
    {
        var feedbacks = await _feedbackRepository.GetFeedbacksWithDetailsAsync();
        return feedbacks.Select(MapToDTO);
    }

    /// <summary>
    /// Retrieves detailed feedback by its ID.
    /// </summary>
    /// <param name="id">Feedback identifier.</param>
    /// <returns>Feedback response DTO, or null if not found.</returns>
    public async Task<FeedbackResponseDTO?> GetFeedbackByIdAsync(int id)
    {
        var feedbacks = await _feedbackRepository.GetFeedbacksWithDetailsAsync();
        var feedback = feedbacks.FirstOrDefault(f => f.FeedbackId == id);
        return feedback != null ? MapToDTO(feedback) : null;
    }

    /// <summary>
    /// Submits a guest feedback review.
    /// </summary>
    /// <param name="dto">Create feedback DTO containing comments and rating stars.</param>
    /// <returns>Created feedback response details.</returns>
    public async Task<FeedbackResponseDTO> SubmitFeedbackAsync(CreateFeedbackDTO dto)
    {
        var guest = await _guestRepository.GetByIdAsync(dto.GuestId);
        if (guest == null)
            throw new InvalidOperationException("Guest account not found.");

        var feedback = new Feedback
        {
            GuestId = dto.GuestId,
            ReservationId = dto.ReservationId,
            Rating = dto.Rating,
            Comments = dto.Comments,
            CreatedAt = DateTime.Now
        };

        await _feedbackRepository.AddAsync(feedback);
        return MapToDTO(feedback);
    }

    /// <summary>
    /// Deletes a specific feedback entry by its ID.
    /// </summary>
    /// <param name="id">Feedback identifier.</param>
    public async Task DeleteFeedbackAsync(int id)
    {
        await _feedbackRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Maps a Feedback model to a FeedbackResponseDTO.
    /// </summary>
    private static FeedbackResponseDTO MapToDTO(Feedback f) => new()
    {
        FeedbackId = f.FeedbackId,
        GuestId = f.GuestId,
        GuestName = f.Guest?.FullName ?? "Guest",
        ReservationId = f.ReservationId,
        Rating = f.Rating,
        Comments = f.Comments,
        CreatedAt = f.CreatedAt
    };
}
