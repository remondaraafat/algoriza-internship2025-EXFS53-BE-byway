using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.LectureDTOs
{
    public class GetLectureByCourseIdDto
    {
        public int Id { get; set; }              
        public string Title { get; set; }        
        public int Order { get; set; }           
        public int DurationMinutes { get; set; } 
        public int CourseId { get; set; }        
    }
}
