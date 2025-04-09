public record NoteQueryParams(
    string? Search = null,
    string? SortBy = "UpdatedAt",
    bool SortDesc = true,
    int Page = 1,
    int Limit = 10
);
