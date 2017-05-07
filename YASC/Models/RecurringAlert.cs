using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YASC.Models
{
    public class RecurringAlert
    {
        /// <summary>
        /// The Id of the recurring alert.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The email address to send reports to.
        /// </summary>
        [Display(Name = "Email Address"), DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// The date when this alert was created.
        /// </summary>
        [Display(Name = "Created On (local)")]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The URL to monitor.
        /// </summary>
        [Display(Name = "URL")]
        public string Url { get; set; }

        /// <summary>
        /// Specifies the user who owns the alert.
        /// </summary>
        [Display(Name = "User")]
        public ApplicationUser ApplicationUser { get; set; }

        /// <summary>
        /// The Id of the application user.
        /// </summary>
        [Display(Name = "User Name")]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }
    }
}
