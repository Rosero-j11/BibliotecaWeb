namespace BibliotecaWeb.Models;

public class PageInfo
{
    public int TotalItems { get; set; }
    public int ItemsPerPage { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages => Math.Max(1, (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage));

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}

public class Paginado<T>
{
    public required List<T> Items { get; init; }
    public required PageInfo PageInfo { get; init; }
}
