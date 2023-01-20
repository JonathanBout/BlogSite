namespace BlogSite.Data
{
	public class Paragraph
	{
		public string? Title { get; set; }
		public string Body { get; set; }
		public Paragraph() : this(null, "") { }
		public Paragraph(string body) : this(null, body) { }
		public Paragraph(string? title, string body)
		{
			Title = title;
			Body = body;
		}

		public override string ToString()
		{
			if (!string.IsNullOrWhiteSpace(Title))
			{
				return Title + "\n" + Body;
			}
			return Body;
		}
	}
}
