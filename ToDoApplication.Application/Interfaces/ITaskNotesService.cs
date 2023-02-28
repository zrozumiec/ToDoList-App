using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Application.Interfaces
{
    /// <summary>
    /// Interface for task notes services.
    /// </summary>
    public interface ITaskNotesService
    {
        /// <summary>
        /// Async method to add new note.
        /// </summary>
        /// <param name="taskNoteDto">Note Dto.</param>
        /// <returns>Added note id.</returns>
        /// <exception cref="ArgumentNullException">Throws when note to add is null.</exception>
        Task<int> AddAsync(TaskNotesDto taskNoteDto);

        /// <summary>
        /// Async method to update existing note.
        /// </summary>
        /// <param name="id">Id of note to update.</param>
        /// <param name="taskNoteDto">New note Dto data.</param>
        /// <returns>Updated note id.</returns>
        /// <exception cref="ArgumentNullException">Throws when new note data is null.</exception>
        /// <exception cref="ArgumentException">Throws when note with given id doesn't exist.</exception>
        Task<int> UpdateAsync(int id, TaskNotesDto taskNoteDto);

        /// <summary>
        /// Async method to delete existing note.
        /// </summary>
        /// <param name="id">Id of note to delete.</param>
        /// <returns>Deleted note id.</returns>
        /// <exception cref="ArgumentException">Throws when note with given id doesn't exist.</exception>
        Task<int> DeleteAsync(int id);

        /// <summary>
        /// Async method to return note with specified id.
        /// </summary>
        /// <param name="id">Note id.</param>
        /// <returns>Note with specified id.</returns>
        /// <exception cref="ArgumentException">Throws when note with given id doesn't exist.</exception>
        Task<TaskNotesDto> GetByIdAsync(int id);

        /// <summary>
        /// Returns all notes.
        /// </summary>
        /// <returns>All notes.</returns>
        IEnumerable<TaskNotesDto> GetAll(int id);
    }
}
