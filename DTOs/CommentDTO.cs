namespace DTOs
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDTO author { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; } 
    }
}
