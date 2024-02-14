using System.ComponentModel.DataAnnotations.Schema;

namespace workshop.webapi.DataModels
{
    [Table("post")]
    public class Post
    {
        [Column("id")] public int Id { get; set; }
        [Column("author_id")] [ForeignKey(nameof(ApplicationUser))] public string AuthorId { get; set; }
        [Column("content")] public string Content { get; set; }
    }
}
