// Library.Domain/Entities/Category.cs
using Library.Domain.Entities;

public partial class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public int? ParentCategoryId { get; set; }
    public string? Description { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    // Đổi tên cho dễ hiểu
    public virtual Category? ParentCategory { get; set; }

    // Collection cho các category con
    public virtual ICollection<Category> ChildCategories { get; set; } = new List<Category>();  
}