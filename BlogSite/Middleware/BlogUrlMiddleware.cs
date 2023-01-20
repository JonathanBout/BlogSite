using BlogSite.Data;
using BlogSite.Data.Services;

namespace BlogSite.Middleware
{
	public class BlogUrlMiddleware : IMiddleware
	{
		BlogPostLoader PostLoader { get; }
		public BlogUrlMiddleware(BlogPostLoader postLoader)
		{
			PostLoader = postLoader;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			string url = context.Request.Path;
			if (url.Contains("post"))
			{
				if (int.TryParse(url.Split('/').Last(), out int postId))
				{
					var post = PostLoader.GetBlogPost(postId);
					if (post != null)
					{
						context.Response.Redirect(post.GetUriString());
					}
				}
			}else if (url.ToLower() == "/archive")
			{
				context.Response.Redirect("/" + (DateTime.Now.Year - 1).ToString());
			}
			else
			{
				string[] splits = url.Split("/", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
				if (splits.Length == 4)
				{
					if (int.TryParse(splits[0], out int year )
					 && int.TryParse(splits[1], out int month)
					 && int.TryParse(splits[2], out int day  )
					 && int.TryParse(splits[3], out int id  ))
					{
						var post = PostLoader.GetBlogPost(id);
						if (post is not null && post.PublishedOn.Date != new DateTime(year, month, day))
						{
							context.Response.Redirect(post.GetUriString());
						}
					}
				}
			}
			await next.Invoke(context);
		}
	}
}
