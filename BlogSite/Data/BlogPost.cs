using LiteDB;
using Microsoft.AspNetCore.Components;

namespace BlogSite.Data
{
	public class BlogPost : IId
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public DateTime PublishedOn { get; set; }
		public Paragraph Introduction { get; set; }
		public Paragraph[] Paragraphs { get; set; }

		public BlogPost() : this("", DateTime.MinValue, new(), Array.Empty<Paragraph>()) { }
		public BlogPost(string title, DateTime publishedOn, Paragraph introduction, params Paragraph[] paragraphs)
		{
			Title = title;
			PublishedOn = publishedOn;
			Introduction = introduction;
			Paragraphs = paragraphs;
		}

		public override string ToString()
		{
			return Title + "\n" + Introduction + "\n" + string.Join('\n', (IEnumerable<Paragraph>)Paragraphs);
		}
	}
}
