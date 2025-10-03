using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.LectureDTOs
{
    public class UpdateLectureDto
    {
        [Required(ErrorMessage = "Lecture Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Order is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Order must be a positive number.")]
        public int Order { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [Range(1, 120, ErrorMessage = "Duration must be between 1 and 120 minutes.")]
        public int DurationMinutes { get; set; }

        [Required(ErrorMessage = "CourseId is required.")]
        public int CourseId { get; set; }
    }
}
