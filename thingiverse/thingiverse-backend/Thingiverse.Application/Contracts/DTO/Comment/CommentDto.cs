namespace Thingiverse.Application.Contracts.DTO.Comment
{
    public class CommentDto
    {
        public int ItemId { get; set; }      // Yorumu yapılacak item
        public string Message { get; set; }  // Yorum metni
    }
}
