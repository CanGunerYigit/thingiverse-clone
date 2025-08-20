using System.ComponentModel.DataAnnotations;

namespace Thingiverse.Application.Contracts.DTO.Comment
{
    public class CommentDto
    {

        [Required(ErrorMessage = "Item Id is Required.")]
        public int ItemId { get; set; }      // Yorumu yapılacak item

        [Required(ErrorMessage = "Message cannot be null")]
        [StringLength(200, ErrorMessage = "Comment can be 200 characters maximum")]

        public string Message { get; set; }  // Yorum metni
    }
}
