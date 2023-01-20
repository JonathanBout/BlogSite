namespace BlogSite.Data.Enums
{
	public enum PreviewComponentVisibility
	{
		Hidden = 0,
		Title = 1,
		CreationDate = 2,
		ContentPreview = 4,
		TimeToRead = 8,
		All = ~Hidden
	}
}
