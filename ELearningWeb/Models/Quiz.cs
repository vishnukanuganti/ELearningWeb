﻿using System.ComponentModel.DataAnnotations;

namespace ELearningWeb.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Quiz title is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Quiz title must be between 2 and 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Class ID is required")]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        public List<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
        
    }
}
