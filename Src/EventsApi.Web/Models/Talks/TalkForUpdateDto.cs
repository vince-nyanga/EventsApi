﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EventsApi.Web.Models.Talks
{
    public class TalkForUpdateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTimeOffset ScheduledDateTime { get; set; }

    }
}
